using CutelynResults.Utils;
using System.Diagnostics;

namespace CutelynResults.Core;

public interface IError : IResult
{
    Exception Exception { get; }

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
    internal Error(Exception exception) 
    {
        _exception = exception;
        try
        {
            exception.SetStackTrace(new StackTrace(1));
        }
        catch (Exception) 
        { 
            //ignored
        }
    }
}
internal sealed class Error<T> : Error, IError<T>
{
    internal Error(Exception exception) : base(exception) { }
}


