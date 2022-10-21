using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests;

internal class Convert
{
    [Test]
    public void Success()
    {
        const string testValue = "Hello world";
        var expectedBytes = Encoding.Unicode.GetBytes(testValue);

        IResult<string> stringResult = IResult.Success(testValue);
        IResult<byte[]> bytesResult = stringResult.Convert(str => Encoding.Unicode.GetBytes(str));

        Assert.That(bytesResult, Is.InstanceOf<ISuccess<byte[]>>());
        var actualBytes = bytesResult.Unwrap();

        Assert.That(actualBytes, Is.EqualTo(expectedBytes));
    }

    [Test]
    public void Error()
    {
        IResult<string> stringResult = IResult.Error<string>(new TestException());
        IResult<byte[]> bytesResult = stringResult.Convert(str => Encoding.Unicode.GetBytes(str));

        Assert.That(bytesResult, Is.InstanceOf<IError<byte[]>>());
        var exception = bytesResult.UnwrapException();
        Assert.That(exception, Is.InstanceOf<TestException>());
    }

    [Test]
    public void Throw()
    {
        const string testValue = "Hello world";

        IResult<string> stringResult = IResult.Success(testValue);
        IResult<byte[]> bytesResult = stringResult.Convert<byte[]>(str => throw new TestException());
        Assert.That(bytesResult, Is.InstanceOf<IError<byte[]>>());
        var exception = bytesResult.UnwrapException();
        Assert.That(exception, Is.InstanceOf<TestException>());
    }


    [Test]
    public void IError()
    {
        IError nonGenericError = IResult.Error(new TestException());
        IResult<int>? intResult;
        IResult<String>? boxedStringResult;
        Assert.DoesNotThrow(() =>
        {
            intResult = nonGenericError.Convert<int>();
        });
        Assert.DoesNotThrow(() =>
        {
            boxedStringResult = nonGenericError.Convert<String>();
        });
    }

    [Test]
    public void ShakeySuccess()
    {
        const string testValue = "Hello world";
        var expectedBytes = Encoding.Unicode.GetBytes(testValue);

        IResult<string> stringResult = IResult.Success(testValue);
        IResult<byte[]> bytesResult = stringResult.ConvertShaky(str => IResult.Success(Encoding.Unicode.GetBytes(str)));

        Assert.That(bytesResult, Is.InstanceOf<ISuccess<byte[]>>());
        var actualBytes = bytesResult.Unwrap();

        Assert.That(actualBytes, Is.EqualTo(expectedBytes));
    }

    [Test]
    public void ShakeyConvertionError()
    {
        const string testValue = "Hello world";

        IResult<string> stringResult = IResult.Success(testValue);
        IResult<byte[]> bytesResult = stringResult.ConvertShaky(str => IResult.Error<byte[]>(new TestException()));

        Assert.That(bytesResult, Is.InstanceOf<IError<byte[]>>());
        var exception = bytesResult.UnwrapException();

        Assert.That(exception, Is.InstanceOf<TestException>());
    }

    [Test]
    public void ShakeyStartedError()
    {
        IResult<string> stringResult = IResult.Error<string>(new TestException());
        IResult<byte[]> bytesResult = stringResult.ConvertShaky(str => IResult.Error<byte[]>(new AnotherTestException()));

        Assert.That(bytesResult, Is.InstanceOf<IError<byte[]>>());
        var exception = bytesResult.UnwrapException();

        Assert.That(exception, Is.InstanceOf<TestException>());
    }


    [Test]
    public void NonGenericSuccess()
    {
        const int testValue = 24;
        IResult result = IResult.Success();
        IResult<int> intResult = result.Convert(() => testValue);

        Assert.That(intResult, Is.InstanceOf<ISuccess<int>>());
        var int1 = intResult.Unwrap();

        Assert.That(int1, Is.EqualTo(testValue));


        IResult<int> int2 = result.Convert(testValue);

        Assert.That(int2, Is.InstanceOf<ISuccess<int>>());
        var interger2 = int2.Unwrap();

        Assert.That(interger2, Is.EqualTo(testValue));
    }

    [Test]
    public void NonGenericError()
    {
        const int testValue = 24;
        IResult result = IResult.Error(new TestException());
        IResult<int> intResult = result.Convert(() => testValue);

        Assert.That(intResult, Is.InstanceOf<IError<int>>());
        var exception = intResult.UnwrapException();

        Assert.That(exception, Is.InstanceOf<TestException>());


        IResult<int> intResult2 = result.Convert(testValue);

        Assert.That(intResult2, Is.InstanceOf<IError<int>>());
        var exception2 = intResult2.UnwrapException();

        Assert.That(exception2, Is.InstanceOf<TestException>());
    }

    [Test]
    public void NonGenericShakeySuccess()
    {
        const int testValue = 6;
        IResult result = IResult.Success(testValue);
        IResult<int> intResult = result.ConvertShakey(() => IResult.Success(testValue));

        Assert.That(intResult, Is.InstanceOf<ISuccess<int>>());
        var @int = intResult.Unwrap();

        Assert.That(@int, Is.EqualTo(testValue));
    }

    [Test]
    public void NonGenericShakeyError()
    {
        const int testValue = 6;
        IResult result = IResult.Success(testValue);
        IResult<int> intResult = result.ConvertShakey(() => IResult.Error<int>(new TestException()));

        Assert.That(intResult, Is.InstanceOf<IError<int>>());
        var exception = intResult.UnwrapException();

        Assert.That(exception, Is.InstanceOf<TestException>());
    }
}
