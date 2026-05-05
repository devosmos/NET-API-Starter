using Devosmos.ApiStarter.Application.Common.Results;
using FluentAssertions;

namespace Devosmos.ApiStarter.UnitTests.Application.Common;

public sealed class ResultTests
{
    [Fact]
    public void Success_Should_Carry_Value()
    {
        var result = Result<string>.Success("ok");

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("ok");
        result.Error.Should().BeNull();
    }

    [Fact]
    public void Failure_Should_Carry_Error()
    {
        var error = ApplicationError.Conflict("duplicate", "Already exists.");

        var result = Result<string>.Failure(error);

        result.IsSuccess.Should().BeFalse();
        result.Value.Should().BeNull();
        result.Error.Should().Be(error);
    }

    [Fact]
    public void Validation_Error_Should_Carry_Property_Errors()
    {
        var errors = new Dictionary<string, string[]>
        {
            ["Name"] = ["Name is required."]
        };

        var error = ApplicationError.Validation(errors);

        error.Type.Should().Be(ApplicationErrorType.Validation);
        error.ValidationErrors.Should().ContainKey("Name");
    }
}
