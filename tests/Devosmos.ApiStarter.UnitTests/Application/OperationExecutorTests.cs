using Devosmos.ApiStarter.Application;
using Devosmos.ApiStarter.Application.Abstractions.Operations;
using Devosmos.ApiStarter.Application.Abstractions.Time;
using Devosmos.ApiStarter.Application.Common.Results;
using Devosmos.ApiStarter.Application.System;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace Devosmos.ApiStarter.UnitTests.Application;

public sealed class OperationExecutorTests
{
    [Fact]
    public async Task ExecuteAsync_Should_Run_Registered_Operation()
    {
        var now = new DateTimeOffset(2026, 5, 4, 12, 0, 0, TimeSpan.Zero);
        var time = Substitute.For<IDateTimeProvider>();
        time.UtcNow.Returns(now);

        var services = new ServiceCollection()
            .AddApplication()
            .AddSingleton(time)
            .BuildServiceProvider();

        var executor = services.GetRequiredService<IOperationExecutor>();

        var result = await executor.ExecuteAsync<GetApiInfoRequest, ApiInfoResponse>(
            new GetApiInfoRequest(),
            TestContext.Current.CancellationToken);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Name.Should().Be("Devosmos.ApiStarter");
        result.Value.ServerTimeUtc.Should().Be(now);
    }

    [Fact]
    public async Task ExecuteAsync_Should_Return_Validation_Error_When_Request_Is_Invalid()
    {
        var services = new ServiceCollection()
            .AddApplication()
            .AddScoped<IOperationHandler<TestRequest, string>, TestHandler>()
            .AddScoped<IValidator<TestRequest>, TestRequestValidator>()
            .BuildServiceProvider();

        var executor = services.GetRequiredService<IOperationExecutor>();

        var result = await executor.ExecuteAsync<TestRequest, string>(
            new TestRequest(""),
            TestContext.Current.CancellationToken);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error!.Type.Should().Be(ApplicationErrorType.Validation);
        result.Error.ValidationErrors.Should().ContainKey(nameof(TestRequest.Name));
    }

    private sealed record TestRequest(string Name);

    private sealed class TestRequestValidator : AbstractValidator<TestRequest>
    {
        public TestRequestValidator()
        {
            RuleFor(request => request.Name).NotEmpty();
        }
    }

    private sealed class TestHandler : IOperationHandler<TestRequest, string>
    {
        public Task<Result<string>> HandleAsync(TestRequest request, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Result<string>.Success(request.Name));
        }
    }
}
