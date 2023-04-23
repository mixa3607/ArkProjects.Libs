using System.Reflection;

namespace ArkProjects.Flurl;

public class SerMem
{
    public object? Key { get; set; }
    public object? Value { get; set; }
    public MemberInfo? ValueMemberInfo { get; set; }
}