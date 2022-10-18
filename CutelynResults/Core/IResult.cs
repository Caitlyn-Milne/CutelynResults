using CutelynResults.Core;
using CutelynResults.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CutelynResults.Core;

public partial interface IResult
{
    public static IError Error(Exception exception)
    {
        return new Error(exception);
    }

    public static IError<T> Error<T>(Exception exception)
    {
        return new Error<T>(exception);
    }

    public static ISuccess Success()
    {
        return new Success();
    }

    public static ISuccess<T> Success<T>(T value)
    {
        return new Success<T>(value);
    }

    public static IResult<T> Try<T>(Func<T> tryBlock) {
        if (tryBlock is null) 
        {
            throw new ArgumentNullException(nameof(tryBlock));
        }
        try
        {
            var value = tryBlock!.Invoke();
            return Success(value);
        }
        catch (Exception e)
        {
            return Error<T>(e);
        }
    }

    public static IResult Try(Action tryBlock)
    {
        if (tryBlock is null)
        {
            throw new ArgumentNullException(nameof(tryBlock));
        }
        try
        {
            tryBlock!.Invoke();
            return Success();
        }
        catch (Exception e)
        {
            return Error(e);
        }
    }
}

public partial interface IResult 
{
    /// <summary>
    /// Declares that that result is an error, and gets the exception. Throws an exception is it is actually success
    /// </summary>
    /// <exception cref="NotErrorException"></exception>
    public Exception UnwrapException()
    {
        if (this is not IError)
        {
            throw new NotErrorException();
        }
        return ((IError)this).Exception;
    }


    public IResult OnError(Action<Exception> action) 
    {
        if (this is IError error) 
        {
            action?.Invoke(error.Exception);
        }
        return this;
    }

    public IResult OnSuccess(Action action)
    {
        if (this is ISuccess)
        {
            action?.Invoke();
        }
        return this;
    }
}

public interface IResult<out T> : IResult 
{

    /// <summary>
    /// Declares that that result is success, and gets the value. If it is Error, throws the errors exception
    /// </summary>
    /// <exception cref="Exception">if it is an error throughs that exception</exception>
    public T Unwrap()
    {
        if (this is IError error)
        {
            throw error.Exception;
        }
        return ((ISuccess<T>)this).Value;
    }

    public new IResult<T> OnError(Action<Exception> action)
    {
        OnError(action);
        return this;
    }

    public IResult<T> OnSuccess(Action<T> action)
    {
        if (this is ISuccess<T> success)
        {
            action?.Invoke(success.Value);
        }
        return this;
    }
}



