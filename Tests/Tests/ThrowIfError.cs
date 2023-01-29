using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests;

internal class ThrowIfError
{
    [Test]
    public void WhenError_ShouldThrow()
    {
        IResult result = IResult.Error(new TestException());
        Assert.Throws<TestException>(() => result.ThrowIfError());
    }

    [Test]
    public void WhenSuccess_ShouldNotThrow()
    {
        IResult result = IResult.Success();
        Assert.DoesNotThrow(() => result.ThrowIfError());
    }
}

