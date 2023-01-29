namespace Tests.Tests;

internal class OnError
{
    [Test]
    public void GivenISuccess_WhenOnError_ShouldDoNothing()
    {
        IResult result = IResult.Success();
        var executed = false;
        result.OnError(_ => executed = true);
        Assert.That(executed, Is.False);
    }

    [Test]
    public void GivenIError_WhenOnError_ShouldExecute()
    {
        IResult result = IResult.Error(new TestException());
        var executed = false;
        result.OnError(_ => executed = true);
        Assert.That(executed, Is.True);
    }

    [Test]
    public void GivenIErrorException_WhenOnError_ShouldExecuteWithSameException()
    {
        var exception = new TestException("" + Guid.NewGuid());
        IResult result = IResult.Error(exception);
        result.OnError(e =>
        {
            Assert.That(e, Is.EqualTo(exception));
        });
    }

    [Test]
    public void GivenIErrorWithException_WhenOnErrorOfSameExceptionType_ShouldExecuteWithSameException()
    {
        var exception = new TestException("" + Guid.NewGuid());
        IResult result = IResult.Error(exception);
        result.OnError<TestException>(e =>
        {
            Assert.That(e, Is.EqualTo(exception));
        });
    }

    [Test]
    public void GivenIErrorWithException_WhenOnErrorOfSameExceptionType_ShouldExecute()
    {
        var exception = new TestException("" + Guid.NewGuid());
        IResult result = IResult.Error(exception);
        var executed = false;
        result.OnError<TestException>(e => executed = true);
        Assert.That(executed, Is.True);
    }

    [Test]
    public void GivenIErrorWithException_WhenOnErrorOfDifferentExceptionType_ShouldDoNothing()
    {
        var exception = new TestException("" + Guid.NewGuid());
        IResult result = IResult.Error(exception);
        var executed = false;
        result.OnError<AnotherTestException>(e => executed = true);
        Assert.That(executed, Is.False);
    }
}