using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CutelynResults.Core;

public static class _Destruct
{
    public static bool Destruct<T>(this IResult<T> self, out T? value, out Exception? exception)
    {
        switch (self)
        {
            case ISuccess<T> asSuccess:
            {
                value = asSuccess.Value;
                exception = null;
                return true;
            }
            case IError<T> asError:
            {
                value = default;
                exception = asError.Exception;
                return false;
            }
            default:
            {
                throw new InvalidOperationException();
            }
        }
    }
}