using FluentAssertions;

namespace Juggor.Core.Process;

public class ProcessTest
{
    [Fact]
    public void Success_returns_success_result()
    {
        var result = ProcessResult<int, string>.Success(1);
        result.IsSucceeded.Should().BeTrue();
        result.IsError.Should().BeFalse();
        result.SuccessValue.Should().Be(1);
    }

    [Fact]
    public void Error_returns_error_result()
    {
        var result = ProcessResult<int, string>.Error("test");
        result.IsSucceeded.Should().BeFalse();
        result.IsError.Should().BeTrue();
        result.ErrorValue.Should().Be("test");
    }

    [Fact]
    public void SuccessValue_throws_exception_if_result_is_error()
    {
        Action act = () =>
        {
            _ = ProcessResult<int, string>.Error("test").SuccessValue;
        };

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Can't get success value because result is error.");
    }

    [Fact]
    public void ErrorValue_throws_exception_if_result_is_succeeded()
    {
        Action act = () =>
        {
            _ = ProcessResult<int, string>.Success(1).ErrorValue;
        };

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Can't get error value because result is succeeded.");
    }
}
