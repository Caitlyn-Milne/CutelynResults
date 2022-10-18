using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Tests;

internal class TestException : Exception
{
    public TestException()
    {
    }

    public TestException(string? message) : base(message)
    {
    }

    public TestException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected TestException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}

internal class AnotherTestException : Exception
{
    public AnotherTestException()
    {
    }

    public AnotherTestException(string? message) : base(message)
    {
    }

    public AnotherTestException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected AnotherTestException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
