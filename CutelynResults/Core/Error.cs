using CutelynResults.Utils;
using System.Diagnostics;

namespace CutelynResults.Core;

public interface IError : IResult
{
    Exception Exception { get; }

    public IError<T> Cast<T>() 
    {
        return new Error<T>(Exception);
    }
}

public interface IError<out T> : IError, IResult<T> {}

internal class Error : IError
{
    private Exception _exception;
    public Exception Exception => _exception;
    internal Error(Exception exception) 
    {
        _exception = exception;
        try
        {
            exception.SetStackTrace(new StackTrace(5));
        }
        catch (Exception) 
        { 
            //ignored
        }
    }
}
internal class Error<T> : Error, IError<T>
{
    internal Error(Exception exception) : base(exception) { }
}


