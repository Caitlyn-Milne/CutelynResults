using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CutelynResults.Core;

public static class _IsError
{
    public static bool IsError(this IResult self) //todo test overload
    {
        return self is IError;
    }

    public static bool IsError(this IResult self, out IError? error) //todo test overload
    {
        if (self is IError e)
        {
            error = e;
            return true;
        }
        error = default;
        return false;
    }

    public static bool IsError<T>(this IResult<T> self, out IError<T>? error) //todo test overload
    {
        if (self is IError<T> e)
        {
            error = e;
            return true;
        }
        error = default;
        return false;
    }

    public static bool IsError<T>(this IResult<T> self, out IError<T>? error, out T? value)
    {
        switch (self)
        {
            case ISuccess<T> asSuccess:
                {
                    value = asSuccess.Value;
                    error = default;
                    return false;
                }
            case IError<T> asError:
                {
                    value = default;
                    error = asError;
                    return true;
                }
            default:
                {
                    throw new InvalidOperationException();
                }
        }
    }
}