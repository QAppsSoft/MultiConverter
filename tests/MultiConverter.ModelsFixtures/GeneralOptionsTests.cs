using FluentAssertions;
using MultiConverter.Models.Settings.General;
using NUnit.Framework;

namespace MultiConverter.ModelsFixtures;

public class GeneralOptionsTests
{
    [Test]
    public void Comparing_two_default_copies_should_be_equal()
    {
        GeneralOptions first = GeneralOptions.Default();
        GeneralOptions second = first with { };

        int firstHashCode = first.GetHashCode();
        int secondHashCode = second.GetHashCode();

        bool equal = first == second;

        equal.Should().BeTrue();
        firstHashCode.Should().Be(secondHashCode);
    }

    [Test]
    public void Comparing_two_equals_copies_should_be_equal()
    {
        GeneralOptions first = GeneralOptions.Default() with { SupportedFilesExtensions = new[] { "1", "2", "3", "3" } };
        GeneralOptions second = first with { SupportedFilesExtensions = new[] { "1", "2", "3", "3" } };

        int firstHashCode = first.GetHashCode();
        int secondHashCode = second.GetHashCode();

        bool equal = first == second;

        equal.Should().BeTrue();
        firstHashCode.Should().Be(secondHashCode);
    }

    [Test]
    public void Comparing_two_similar_copies_should_be_equal()
    {
        GeneralOptions first = GeneralOptions.Default() with { SupportedFilesExtensions = new[] { "1", "2", "3", "1" } };
        GeneralOptions second = first with { SupportedFilesExtensions = new[] { "2", "3", "1", "1" } };

        int firstHashCode = first.GetHashCode();
        int secondHashCode = second.GetHashCode();

        bool equal = first == second;

        equal.Should().BeTrue();
        firstHashCode.Should().Be(secondHashCode);
    }

    [Test]
    public void Comparing_two_not_equal_copies_should_be_equal()
    {
        GeneralOptions first = GeneralOptions.Default() with { SupportedFilesExtensions = new[] { "1", "2", "3" } };
        GeneralOptions second = first with { SupportedFilesExtensions = new[] { "4", "5", "6" } };

        int firstHashCode = first.GetHashCode();
        int secondHashCode = second.GetHashCode();

        bool equal = first == second;

        equal.Should().BeFalse();
        firstHashCode.Should().NotBe(secondHashCode);
    }

    [Test]
    public void Comparing_to_null_should_not_be_equal()
    {
        GeneralOptions first = GeneralOptions.Default() with { SupportedFilesExtensions = new[] { "1", "2", "3" } };
        GeneralOptions? second = null;

        bool equal = first == second;

        equal.Should().BeFalse();
    }
}
