using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CutelynResults.Exceptions;

public class NotErrorException : CutelynResultsException
{
    public NotErrorException() : this("Result was not a IError") { }

    public NotErrorException(string? message) : base(message) { }
}
