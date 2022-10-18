using CutelynResults.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests;

internal class Try
{

    [Test]
    public void TrySuccess() 
    {
        var result = IResult.Try(() =>
        {
            Assert.True(true);
        });

        Assert.That(result, Is.InstanceOf<ISuccess>());
    }

    [Test]
    public void TrySuccessGenerics()
    {
        const int testValue = 24;
        var result = IResult.Try(() =>
        {
            Assert.True(true);
            return testValue;
        });

        Assert.That(result, Is.InstanceOf<ISuccess<int>>());
        var value = ((ISuccess<int>)result).Value;
        Assert.That(value, Is.EqualTo(testValue));
    }

    [Test]
    public void TryError()
    {
        var result = IResult.Try(() =>
        {
            throw new TestException();
        });

        Assert.That(result, Is.InstanceOf<IError>());
        var exception = ((IError)result).Exception;
        Assert.That(exception, Is.TypeOf<TestException>());
    }

    [Test]
    public void TryErrorGenerics()
    {
        var result = IResult.Try(() =>
        {
            throw new TestException();
            return 2;
        });

        Assert.That(result, Is.InstanceOf<IError<int>>());
        var exception = ((IError)result).Exception;
        Assert.That(exception, Is.TypeOf<TestException>());
    }
}
