using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests;

internal class AggregateResults
{
    [Test]
    public void Empty()
    {
        var results = new IResult[] { };
        var result = results.AggregateResults();
        Assert.That(result, Is.InstanceOf<ISuccess>());
    }

    [Test]
    public void OneError()
    {
        const string testMessage = "One Error";

        var results = new IResult[]
        {
            IResult.Error(new TestException(testMessage)),
        };

        var result = results.AggregateResults();

        Assert.That(result, Is.InstanceOf<IError>());
        var exception = ((IError)result).Exception;

        Assert.That(exception, Is.InstanceOf<TestException>());
        var testException = (TestException)exception;

        Assert.That(testException.Message, Is.EqualTo(testMessage));
    }

    [Test]
    public void AllErrors()
    {
        var results = new IResult[]
        {
            IResult.Error(new TestException("0")),
            IResult.Error(new TestException("1")),
            IResult.Error(new AnotherTestException("2")),
        };

        var result = results.AggregateResults();

        Assert.That(result, Is.InstanceOf<IError>());
        var exception = ((IError)result).Exception;

        Assert.That(exception, Is.InstanceOf<AggregateException>());
        var innerExceptions = ((AggregateException)exception).InnerExceptions;

        Assert.That(innerExceptions.Count, Is.EqualTo(3));

        Assert.That(innerExceptions[0], Is.InstanceOf<TestException>());
        Assert.That(innerExceptions[0].Message, Is.EqualTo("0"));

        Assert.That(innerExceptions[1], Is.InstanceOf<TestException>());
        Assert.That(innerExceptions[1].Message, Is.EqualTo("1"));

        Assert.That(innerExceptions[2], Is.InstanceOf<AnotherTestException>());
        Assert.That(innerExceptions[2].Message, Is.EqualTo("2"));
    }

    [Test]
    public void OneSuccess()
    {
        var results = new IResult[]
        {
            IResult.Success(),
        };

        var result = results.AggregateResults();
        Assert.That(result, Is.InstanceOf<ISuccess>());
    }

    [Test]
    public void OneSuccessGeneric()
    {
        const int testValue = 5;
        var results = new IResult<int>[]
        {
            IResult.Success(testValue),
        };

        var result = results.Aggregate();
        Assert.That(result, Is.InstanceOf<ISuccess<int[]>>());
        var values = ((ISuccess<int[]>)result).Value;

        Assert.That(values, Is.EquivalentTo(new int[] { testValue }));
    }

    [Test]
    public void AllSuccesses()
    {
        var results = new IResult[]
        {
            IResult.Success(),
            IResult.Success(),
            IResult.Success(),
        };

        var result = results.AggregateResults();
        Assert.That(result, Is.InstanceOf<ISuccess>());
    }


    [Test]
    public void AllSuccessesGenerics()
    {
        var results = new IResult<int>[]
        {
            IResult.Success(1),
            IResult.Success(2),
            IResult.Success(3),
        };

        var result = results.Aggregate();
        Assert.That(result, Is.InstanceOf<ISuccess<int[]>>());
        var values = ((ISuccess<int[]>)result).Value;

        Assert.That(values, Is.EqualTo(new int[] { 1,2,3 }));
    }



    [Test]
    public void ManyMixed()
    {
        var results = new IResult[]
        {
            IResult.Error(new TestException("0")),
            IResult.Error(new AnotherTestException("1")),
            IResult.Success(2),
        };

        var result = results.AggregateResults();

        Assert.That(result, Is.InstanceOf<IError>());
        var exception = ((IError)result).Exception;

        Assert.That(exception, Is.InstanceOf<AggregateException>());
        var innerExceptions = ((AggregateException)exception).InnerExceptions;

        Assert.That(innerExceptions.Count, Is.EqualTo(2));

        Assert.That(innerExceptions[0], Is.InstanceOf<TestException>());
        Assert.That(innerExceptions[0].Message, Is.EqualTo("0"));

        Assert.That(innerExceptions[1], Is.InstanceOf<AnotherTestException>());
        Assert.That(innerExceptions[1].Message, Is.EqualTo("1"));
    }
}
