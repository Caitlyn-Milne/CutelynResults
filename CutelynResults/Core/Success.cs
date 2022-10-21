namespace CutelynResults.Core;

public interface ISuccess : IResult { }

public interface ISuccess<out T> : ISuccess, IResult<T>
{
    T Value { get; }
}

internal class Success : ISuccess { }

internal sealed class Success<T> : Success, ISuccess<T>
{
    private T _value;
    public T Value => _value;
    internal Success(T value)
    {
        _value = value;
    }
}

