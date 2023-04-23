using System;

namespace ArkProjects.Flurl.Converters;

public abstract class FlurlMemberConverter
{
    public abstract string? Convert(object? value, FlurlQuerySerializer serializer);
    public abstract bool CanConvert(Type objectType);
}

public abstract class FlurlMemberConverter<T> : FlurlMemberConverter
{
    public abstract string? Convert(T? value, FlurlQuerySerializer serializer);

    public sealed override string? Convert(object? value, FlurlQuerySerializer serializer)
    {
        //if (!(value != null
        //        ? value is T
        //        : typeof(T).IsValueType && typeof(T).IsGenericType &&
        //          typeof(T).GetGenericTypeDefinition() == typeof(Nullable<>)
        //    ))
        //{
        //    throw new JsonSerializationException(
        //        "Converter cannot write specified value to JSON. {0} is required.".FormatWith(
        //            CultureInfo.InvariantCulture, typeof(T)));
        //}

        return Convert((T?)value, serializer);
    }

    public sealed override bool CanConvert(Type objectType)
    {
        return typeof(T).IsAssignableFrom(objectType);
    }
}