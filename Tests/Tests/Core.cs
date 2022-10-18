using CutelynResults.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests;

internal class Core
{
    [Test]
    public void Success() 
    {
        var result = ReturnsSuccess();
        Assert.That(result, Is.InstanceOf<ISuccess>());
    }

    [Test]
    public void SuccessGenerics()
    {
        const int testValue = 42;
        var result = ReturnsSuccessOfInt(testValue);
        Assert.That(result, Is.InstanceOf<ISuccess<int>>());
        var successValue = ((ISuccess<int>)result).Value;
        Assert.That(successValue, Is.EqualTo(testValue));
    }

    [Test]
    public void Error()
    {
        var result = ReturnsError();
        Assert.That(result, Is.InstanceOf<IError>());
        var exception = ((IError)result).Exception;
        Assert.That(exception, Is.TypeOf<TestException>());
    }

    [Test]
    public void ErrorChain()
    {
        var  result = ReturnsErrorFromChain();
        Assert.That(result, Is.InstanceOf<IError<string>>());
        var exception = ((IError)result).Exception;
        Assert.That(exception, Is.TypeOf<TestException>());
    }

    [Test]
    public void Covariance() 
    {
        const string testValue = "hello world";
        IResult<string> resultString = IResult.Success(testValue);
        IResult<object> resultObject = resultString;
        object unwrap = resultObject.Unwrap();
        Assert.That(unwrap, Is.InstanceOf<string>());
        string str = (string)unwrap;
        Assert.That(str, Is.EqualTo(testValue));
    }

    [Test]
    public void ErrorCast() 
    {
        IError nonGenericError = IResult.Error(new TestException());
        IResult<int>? intResult;
        IResult<String>? boxedStringResult;
        Assert.DoesNotThrow(() => {
            intResult = nonGenericError.Cast<int>();
        });
        Assert.DoesNotThrow(() => {
            boxedStringResult = nonGenericError.Cast<String>();
        });
    }

    private IResult ReturnsError() =>
        IResult.Error(new TestException());
    
    private IResult ReturnsSuccess() =>
        IResult.Success();

    private IResult<int> ReturnsSuccessOfInt(int val) =>
        IResult.Success(val);


    private IResult<int> ReturnsErrorOfInt() =>
       IResult.Error<int>(new TestException());


    private IResult<string> ReturnsErrorFromChain() =>
       ((IError) ReturnsErrorOfInt()).Cast<string>();

}
