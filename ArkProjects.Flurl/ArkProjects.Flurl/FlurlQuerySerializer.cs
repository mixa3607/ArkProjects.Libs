using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ArkProjects.Flurl.Attributes;
using ArkProjects.Flurl.Converters;
using Flurl.Util;

namespace ArkProjects.Flurl;

public class FlurlQuerySerializer
{
    public FlurlQuerySerializerSettings Settings { get; set; }

    public FlurlQuerySerializer(FlurlQuerySerializerSettings settings)
    {
        Settings = settings;
    }

    public FlurlQuerySerializer() : this(new FlurlQuerySerializerSettings())
    {
    }

    public IReadOnlyDictionary<string, string?> Serialize(object? obj)
    {
        if (obj == null)
            throw new ArgumentNullException(nameof(obj));

        var kvs = obj switch
        {
            string s => SerializeString(s),
            IEnumerable e => SerializeEnumerable(e),
            _ => SerializeObject(obj)
        };

        var args = new Dictionary<string, string?>();
        foreach (var serMem in kvs)
        {
            var key = serMem.ValueMemberInfo?.GetCustomAttribute<FlurlQueryMemberAttribute>()?.Name ??
                      serMem.Key?.ToInvariantString();
            if (key == null)
                continue;

            //null
            if (serMem.Value == null)
            {
                args[key] = null;
                continue;
            }

            //with converter
            var converter = GetConverter(serMem.ValueMemberInfo, serMem.Value?.GetType());
            if (serMem.Value is IEnumerable e and not string)
            {
                if (converter?.CanConvert(serMem.Value!.GetType()) == true)
                {
                    args[key] = converter.Convert(serMem.Value, this);
                    continue;
                }

                var valVals = new List<string?>();
                foreach (var valObj in e)
                {
                    if (valObj == null)
                        valVals.Add(null);
                    else if (converter?.CanConvert(valObj.GetType()) == true)
                        valVals.Add(converter.Convert(valObj, this));
                    else
                        valVals.Add(valObj.ToInvariantString());
                }

                switch (Settings.ArrayStrategy)
                {
                    case FlurlQuerySerializerArrayStrategy.MultipleArgs:
                    {
                        for (int i = 0; i < valVals.Count; i++)
                            args[$"{key}[{i}]"] = valVals[i];
                        break;
                    }
                    case FlurlQuerySerializerArrayStrategy.SingleArgJoin:
                        args[key] = string.Join(',', valVals);
                        break;
                    default:
                        throw new NotImplementedException($"Strategy {Settings.ArrayStrategy} not implemented");
                }
            }
            else
            {
                var valObj = serMem.Value;
                if (valObj == null)
                    args[key] = null;
                else if (converter?.CanConvert(valObj.GetType()) == true)
                    args[key] = converter.Convert(valObj, this);
                else
                    args[key] = valObj.ToInvariantString();
            }
        }

        return args;
    }

    private static IEnumerable<SerMem> SerializeObject(object obj)
    {
        foreach (var prop in obj.GetType().GetProperties())
        {
            var getter = prop.GetGetMethod(false);
            if (getter == null) 
                continue;
            var val = getter.Invoke(obj, null);
            yield return new SerMem() { Key = prop.Name, Value = val, ValueMemberInfo = prop };
        }
        foreach (var prop in obj.GetType().GetFields())
        {
            var val = prop.GetValue(obj);
            yield return new SerMem() { Key = prop.Name, Value = val, ValueMemberInfo = prop };
        }
    }

    private IEnumerable<SerMem> SerializeString(string str)
    {
        if (string.IsNullOrWhiteSpace(str))
            return Array.Empty<SerMem>();

        var tuples =
            from p in str.Split('&')
            let pair = p.SplitOnFirstOccurence("=")
            let name = pair[0]
            let value = (pair.Length == 1) ? null : pair[1]
            select new SerMem() { Key = name, Value = value };
        return tuples.Where(x => !string.IsNullOrEmpty((string?)x.Key));
    }

    private static IEnumerable<SerMem> SerializeEnumerable(IEnumerable col)
    {
        bool TryGetProp(object obj, string name, out object? value, out MemberInfo? memberInfo)
        {
            var prop = obj.GetType().GetProperty(name);
            var field = obj.GetType().GetField(name);

            if (prop != null)
            {
                memberInfo = prop;
                value = prop.GetValue(obj, null);
                return true;
            }

            if (field != null)
            {
                memberInfo = field;
                value = field.GetValue(obj);
                return true;
            }

            memberInfo = null;
            value = null;
            return false;
        }

        bool IsTuple2(object item, out object? name, out object? val, out MemberInfo? valMember)
        {
            name = null;
            val = null;
            valMember = null;
            ;
            return item.GetType().Name.IndexOf("Tuple", StringComparison.Ordinal) >= 0 &&
                   TryGetProp(item, "Item1", out name, out _) &&
                   TryGetProp(item, "Item2", out val, out valMember) &&
                   !TryGetProp(item, "Item3", out _, out _);
        }

        bool LooksLikeKV(object item, out object? name, out object? val, out MemberInfo? valMember)
        {
            name = null;
            val = null;
            valMember = null;
            return (TryGetProp(item, "Key", out name, out _) || TryGetProp(item, "key", out name, out _) ||
                    TryGetProp(item, "Name", out name, out _) || TryGetProp(item, "name", out name, out _)) &&
                   (TryGetProp(item, "Value", out val, out valMember) ||
                    TryGetProp(item, "value", out val, out valMember));
        }

        foreach (var item in col)
        {
            if (item == null)
                continue;
            if (!IsTuple2(item, out var name, out var val, out var valMember) &&
                !LooksLikeKV(item, out name, out val, out valMember))
                yield return new SerMem() { Key = item };
            else if (name != null)
                yield return new SerMem() { Key = name, Value = val, ValueMemberInfo = valMember };
        }
    }


    private FlurlMemberConverter? GetConverter(MemberInfo? member, Type? type)
    {
        if (type == null)
            return null;

        // attribute
        if (member != null)
        {
            var converterAttr = member.GetCustomAttribute<FlurlQueryMemberConvertAttribute>();
            if (converterAttr != null)
            {
                if (!converterAttr.ConverterType.IsAssignableTo(typeof(FlurlMemberConverter)))
                    throw new FlurlMemberSerializationException($"Type {converterAttr.ConverterType} is not converter");
                return (FlurlMemberConverter)Activator.CreateInstance(converterAttr.ConverterType)!;
            }
        }

        // converters
        var converter = Settings.Converters.FirstOrDefault(x => x.CanConvert(type));
        if (converter != null)
        {
            return converter;
        }

        // miss
        return null;
    }
}