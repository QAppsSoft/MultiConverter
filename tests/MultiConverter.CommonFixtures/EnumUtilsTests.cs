using AwesomeAssertions;
using MultiConverter.Common.Utils;
using MultiConverter.Models.Settings.General;

namespace MultiConverter.CommonFixtures;

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
