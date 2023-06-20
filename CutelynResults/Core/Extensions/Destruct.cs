using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CutelynResults.Core;

public static class _Destruct
{
    public static bool Destruct<T>(this IResult<T> self, out T? value, out IError? error)
    {
        switch (self)
        {
            case ISuccess<T> asSuccess:
                {
                    value = asSuccess.Value;
                    error = null;
                    return true;
                }
            case IError<T> asError:
                {
                    value = default;
                    error = asError;
                    return false;
                }
            default:
                {
                    throw new InvalidOperationException();
                }
        }
    }
}