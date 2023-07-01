using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests;

internal class IsError
{
    [Test]
    public void GivenISuccess_WhenIsError_ShouldBeFalse()
    {
        IResult<int> result = IResult.Success(42);
        Assert.That(result.IsError(out _, out _), Is.False);
        Assert.That(result.IsError(out _), Is.False);
        Assert.That(result.IsError(), Is.False);
    }

    [Test]
    public void GivenIError_WhenIsError_ShouldBeTrue()
    {
        IResult<int> result = IResult.Error<int>(new TestException());
        Assert.That(result.IsError(out _, out _), Is.True);
        Assert.That(result.IsError(out _), Is.True);
        Assert.That(result.IsError(), Is.True);
    }

    [Test]
    public void GivenISuccess_WhenIsError_ShouldHaveOutSuccess()
    {
        IResult<int> result = IResult.Success(42);
        result.IsError(out _, out var value);
        Assert.That(value, Is.EqualTo(result.Unwrap()));
    }

    public void GivenIError_WhenIsError_ShouldNotHaveOutSuccess()
    {
        IResult<int> result = IResult.Error<int>(new TestException());
        result.IsError(out _, out var value);
        Assert.That(value, Is.EqualTo(default(int)));
    }

    [Test]
    public void GivenISuccess_WhenIsError_ShouldNotHaveOutError()
    {
        IResult<int> genericResult = IResult.Success(42);

        genericResult.IsError(out IError<int>? genericError, out _);
        Assert.That(genericError, Is.Null);

        genericResult.IsError(out genericError);
        Assert.That(genericError, Is.Null);

        IResult basicResult = IResult.Success();
        basicResult.IsError(out IError? basicError);
        Assert.That(basicError, Is.Null);
    }

    [Test]
    public void GivenIError_WhenIsError_ShouldHaveOutError()
    {
        IResult<int> genericResult = IResult.Error<int>(new TestException());

        genericResult.IsError(out IError<int>? error, out _);
        Assert.That(error, Is.EqualTo(genericResult));

        genericResult.IsError(out error);
        Assert.That(error, Is.EqualTo(genericResult));

        IResult basicResult = IResult.Error(new TestException());
        basicResult.IsError(out IError? basicError);
        Assert.That(basicError, Is.EqualTo(basicResult));
    }


    [Test]
    public void IsError_Integration()
    {
        const int testValue = 42;
        const string testExceptionMessage = "test message";

        IResult<int> result = IResult.Success(testValue);
        if (result.IsError(out var _, out int value))
        {
            Assert.Fail("This should not of ran");           
        }
        else
        {
            Assert.That(value, Is.EqualTo(testValue));
        }

        result = IResult.Error<int>(new TestException(testExceptionMessage));
        if (result.IsError(out IError<int>? error, out _))
        {
            Assert.NotNull(error);
            Assert.That(error!.Exception.Message, Is.EqualTo(testExceptionMessage));
        }
        else
        {
            Assert.Fail("This should not of ran");
        }

    }
}

