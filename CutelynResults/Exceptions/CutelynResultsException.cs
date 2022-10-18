using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CutelynResults.Exceptions;
public class CutelynResultsException : Exception
{
    public CutelynResultsException() { }

    public CutelynResultsException(string? message) : base(message) { }

    public CutelynResultsException(string? message, Exception? innerException) : base(message, innerException) { }

    protected CutelynResultsException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
