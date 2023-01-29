using System.Diagnostics;

namespace CutelynResults.Core;

public interface IError : IResult
{
    Exception Exception { get; }
    public string StackTrace { get; }

    public IError<A> Convert<A>() 
    {
        return new Error<A>(Exception);
    }
}

public interface IError<out T> : IError, IResult<T> 
{

}

internal class Error : IError
{
    private Exception _exception;
    public Exception Exception => _exception;
    public string StackTrace { get; }

    internal Error(Exception exception)
    {
        _exception = exception;
        StackTrace = Environment.StackTrace;
    }
}
internal sealed class Error<T> : Error, IError<T>
{
    internal Error(Exception exception) :base(exception) { }
}


