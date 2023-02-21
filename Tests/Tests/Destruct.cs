using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests;

internal class Destruct
{
    [Test]
    public void GivenISuccess_WhenDestruct_ShouldBeTrue()
    {
        IResult<int> result = IResult.Success(42);
        Assert.That(result.Destruct(out _, out _), Is.True);
    }

    [Test]
    public void GivenIError_WhenDestruct_ShouldBeFalse()
    {
        IResult<int> result = IResult.Error<int>(new TestException());
        Assert.That(result.Destruct(out _, out _), Is.False);
    }

    [Test]
    public void GivenISuccess_WhenDestruct_ShouldHaveOutSuccess()
    {
        IResult<int> result = IResult.Success(42);
        result.Destruct(out var value, out _);
        Assert.That(value, Is.EqualTo(result.Unwrap()));
    }

    public void GivenIError_WhenDestruct_ShouldNotHaveOutSuccess()
    {
        IResult<int> result = IResult.Error<int>(new TestException());
        result.Destruct(out var value, out _);
        Assert.That(value, Is.EqualTo(default(int)));
    }

    [Test]
    public void GivenISuccess_WhenDestruct_ShouldNotHaveOutError()
    {
        IResult<int> result = IResult.Success(42);
        result.Destruct(out _, out var exception);
        Assert.That(exception, Is.Null);
    }

    [Test]
    public void GivenIError_WhenDestruct_ShouldHaveOutError()
    {
        IResult<int> result = IResult.Error<int>(new TestException());
        result.Destruct(out _, out var error);
        Assert.That(error, Is.EqualTo(result.UnwrapException()));
    }


    [Test]
    public void Destruct_Integration()
    {
        const int testValue = 42;
        const string testExceptionMessage = "test message";

        IResult<int> result = IResult.Success(testValue);
        if (result.Destruct(out var value, out var error))
        {
            Assert.That(value, Is.EqualTo(testValue));
        }
        else
        {
            Assert.Fail("This should not of ran");
        }

        result = IResult.Error<int>(new TestException(testExceptionMessage));
        if (result.Destruct(out value, out error))
        {
            Assert.Fail("This should not of ran");
        }
        else
        {
            Assert.NotNull(error);
            Assert.That(error!.Message, Is.EqualTo(testExceptionMessage));
        }

    }
}

