using Devosmos.ApiStarter.Domain.Abstractions;
using FluentAssertions;

namespace Devosmos.ApiStarter.UnitTests.Domain;

public sealed class ValueObjectTests
{
    [Fact]
    public void Equals_Should_Use_Equality_Components()
    {
        var first = new TestValueObject("api", 10);
        var second = new TestValueObject("api", 10);
        var different = new TestValueObject("worker", 10);

        first.Should().Be(second);
        first.Should().NotBe(different);
    }

    private sealed class TestValueObject(string name, int size) : ValueObject
    {
        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return name;
            yield return size;
        }
    }
}
