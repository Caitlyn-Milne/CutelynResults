using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CutelynResults.Core;

public static class _Aggregate
{
    /// <inheritdoc cref="Aggregate{T}"/>
    public static IResult AggregateResults(this IEnumerable<IResult> results)
    {
        return results.Select<IResult,IResult<object?>>
        (result =>
            (result is IError e) 
            ? e.Cast<IResult<object?>>() 
            : IResult.Success<object?>(null)
        ).Aggregate();
    }

    /// <summary>
    /// Combines an enumerable of results into a single result.
    /// </summary>
    public static IResult<T[]> Aggregate<T>(this IEnumerable<IResult<T>> results)
    {
        var exceptions = new List<Exception>();
        var successValues = new List<T>();
        foreach (var result in results)
        {
            if (result is IError<T> error)
            {
                exceptions.Add(error.Exception);
                continue;
            }
            successValues.Add(result.Unwrap());
        }
        return exceptions.Count switch
        {
            0 => IResult.Success(successValues.ToArray()),
            1 => IResult.Error<T[]>(exceptions.First()),
            _ => IResult.Error<T[]>(new AggregateException(exceptions))
        };
    }

}
