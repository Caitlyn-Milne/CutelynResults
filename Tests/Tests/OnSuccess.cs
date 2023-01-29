namespace Tests;

internal class OnSuccess
{
    [Test]
    public void GivenIError_WhenOnSuccess_ShouldDoNothing()
    {
        IResult result = IResult.Error(new TestException());
        var executed = false;
        result.OnSuccess(() => executed = true);
        Assert.That(executed, Is.False);
    }

    [Test]
    public void GivenISuccess_WhenOnSuccess_ShouldExecute()
    {
        IResult result = IResult.Success();
        var executed = false;
        result.OnSuccess(() => executed = true);
        Assert.That(executed, Is.True);
    }

    [Test]
    public void GivenIErrorOfValue_WhenOnSuccess_ShouldDoNothing()
    {
        IResult<Guid> result = IResult.Error<Guid>(new TestException());
        var executed = false;
        result.OnSuccess(v => executed = true);
        Assert.That(executed, Is.False);
    }

    [Test]
    public void GivenISuccessOfValue_WhenOnSuccess_ShouldExecute()
    {
        IResult<Guid> result = IResult.Success(Guid.NewGuid());
        var executed = false;
        result.OnSuccess(v => executed = true);
        Assert.That(executed, Is.True);
    }

    [Test]
    public void GivenISuccessOfValue_WhenOnSuccess_ShouldExecuteWithValue()
    {
        var value = Guid.NewGuid();
        IResult<Guid> result = IResult.Success(value);
        result.OnSuccess(v =>
        {
            Assert.That(v, Is.EqualTo(value));
        });
    }
}