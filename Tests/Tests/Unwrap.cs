using CutelynResults.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests;

internal class Unwrap
{
    [Test]
    public void Success()
    {
        const int testValue = 42;
        IResult<int> result = IResult.Success(testValue);
        Assert.DoesNotThrow(() => result.Unwrap());
        Assert.That(result.Unwrap(), Is.EqualTo(42));
    }


    [Test]
    public void Error()
    {
        IResult<int> result = IResult.Error<int>(new TestException());
        Assert.Throws<TestException>(() => result.Unwrap());
    }

    [Test]
    public void SuccessUnwrapException()
    {
        IResult<int> result = IResult.Success(42);
        Assert.Throws<NotErrorException>(() => result.UnwrapException());
    }

    [Test]
    public void ErrorUnwrapException()
    {
        IResult<int> result = IResult.Error<int>(new TestException());
        Assert.DoesNotThrow(() => result.UnwrapException());
        var exception = result.UnwrapException();
        Assert.That(exception, Is.InstanceOf<TestException>());
    }
}
