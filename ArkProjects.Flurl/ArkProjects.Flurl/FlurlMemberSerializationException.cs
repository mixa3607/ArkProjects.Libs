using System;

namespace ArkProjects.Flurl;

public class FlurlMemberSerializationException : Exception
{
    public FlurlMemberSerializationException()
    {
    }

    public FlurlMemberSerializationException(string message) : base(message)
    {
    }

    public FlurlMemberSerializationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}