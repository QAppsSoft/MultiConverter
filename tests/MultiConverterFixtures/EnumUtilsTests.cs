using System.Linq;
using FluentAssertions;
using MultiConverter.Models.Settings.General;
using MultiConverter.Utils;
using NUnit.Framework;

namespace MultiConverterFixtures;

public class EnumUtilsTests
{
    [Test]
    public void Get_Enum_list_using_GetValues()
    {
        var themes = EnumUtils.GetValues<Theme>().ToArray();

        themes.Should().NotBeEmpty();
        themes.Length.Should().Be(2);
        themes.Should().BeEquivalentTo(new[] { Theme.Dark, Theme.Light });
        themes.First().GetType().Should().Be(typeof(Theme));
    }
}
