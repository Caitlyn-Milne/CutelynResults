using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests;

internal class On
{
    [Test]
    public void Success() 
    {
        var result = IResult.Success();
        var wasOnSuccessCalled = false;
        result.OnSuccess(() =>
        {
            wasOnSuccessCalled = true;
        }
        ).OnError(e => 
        { 
            Assert.Fail($"Expected '{nameof(IResult.OnError)}' to not be invoked");
        });
        Assert.That(wasOnSuccessCalled, Is.True);
    }

    [Test]
    public void Error()
    {
        var result = IResult.Error(new TestException());
        var wasErrorCalled = false;
        result.OnSuccess(() =>
        {
            Assert.Fail($"Expected '{nameof(IResult.OnSuccess)}' to not be invoked");
        }
        ).OnError(exception =>
        {
            Assert.That(exception, Is.InstanceOf<TestException>());
            wasErrorCalled = true;
        });
        Assert.That(wasErrorCalled, Is.True);
    }
}
