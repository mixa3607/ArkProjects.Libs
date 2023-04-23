namespace ArkProjects.Flurl.Converters;

public class FlurlMemberConvertBoolToInt : FlurlMemberConverter<bool?>
{
    public int? TrueValue { get; set; } = 1;
    public int? FalseValue { get; set; } = 0;
    public int? NullValue { get; set; }

    public override string? Convert(bool? value, FlurlQuerySerializer serializer)
    {
        return value switch
        {
            null => NullValue?.ToString(),
            true => TrueValue?.ToString(),
            false => FalseValue?.ToString()
        };
    }
}