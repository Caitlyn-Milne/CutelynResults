using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests;

internal class TryAsync
{
    private const string TestString = "Hello World";
    private const int WaitTime = 500;

    [Test]
    public async Task ForStandard_WhenThrowing_ShouldReturnIError()
    {
        var result = await StandardThrowsAsync();
        Assert.That(result, Is.InstanceOf<IError>());
        Assert.That(result.UnwrapException(), Is.TypeOf<TestException>());
    }

    [Test]
    public async Task ForStandard_WhenTaskCompletes_ShouldReturnISuccess()
    {
        var result = await StandardNotThrow();
        Assert.That(result, Is.InstanceOf<ISuccess>());
    }

    [Test]
    public async Task ForGenerics_WhenThrowing_ShouldReturnIError()
    {

        var result = await GenericsThrowsAsync();
        Assert.That(result, Is.InstanceOf<IError>());
        Assert.That(result.UnwrapException(), Is.TypeOf<TestException>());
    }

    [Test]
    public async Task ForGenerics_WhenReturningValue_ShouldReturnISuccess()
    {
        var result = await GenericsReturnValueAsync();
        Assert.That(result, Is.InstanceOf<ISuccess>());
        Assert.That(result, Is.InstanceOf<ISuccess<string>>());
    }

    [Test]
    public async Task ForGenerics_WhenReturningValue_ShouldContainValue()
    {
        var result = await GenericsReturnValueAsync();
        Assert.That(result.Unwrap(), Is.EqualTo(TestString));
    }

    [Test]
    public async Task ForStandard_ShouldAwait()
    {
        var stopWatch = Stopwatch.StartNew();
        await StandardNotThrow();
        stopWatch.Stop();
        Assert.That(stopWatch.ElapsedMilliseconds, Is.EqualTo(WaitTime).Within(50));
    }


    [Test]
    public async Task ForGenerics_ShouldAwait()
    {
        var stopWatch = Stopwatch.StartNew();
        await GenericsReturnValueAsync();
        stopWatch.Stop();
        Assert.That(stopWatch.ElapsedMilliseconds, Is.EqualTo(WaitTime).Within(50));
    }

    private Task<IResult> StandardThrowsAsync() => IResult.TryAsync(async () =>
    {
        await Task.Delay(WaitTime);
        throw new TestException();
    });


    private Task<IResult> StandardNotThrow() => IResult.TryAsync(async () =>
    {
        await Task.Delay(WaitTime);
    });

    private Task<IResult<string>> GenericsThrowsAsync() => IResult.TryAsync(async () =>
    {
        await Task.Delay(WaitTime);
        throw new TestException();
        return TestString;
    });

    private Task<IResult<string>> GenericsReturnValueAsync() => IResult.TryAsync(async () =>
    {
        await Task.Delay(WaitTime);
        return TestString;
    });
}

