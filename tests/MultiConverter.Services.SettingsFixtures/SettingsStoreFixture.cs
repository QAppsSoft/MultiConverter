using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using MultiConverter.Services.Abstractions.Settings;
using MultiConverter.Services.Settings;
using NUnit.Framework;

namespace MultiConverter.Services.SettingsFixtures;


public class SettingsStoreFixture
{
    [Test]
    public void WriteState()
    {
        var state = new State(1, "Test");

        var store = new FileSettingsStore(NullLogger.Instance);
        store.Save("testfile", state);

        var restored = store.Load("testfile");
        restored.Should().Be(state);
    }

    [Test]
    public void WriteComplexState()
    {
        var state = new State(1, "<<something weird<> which breaks xml {}");

        var store = new FileSettingsStore(NullLogger.Instance);
        store.Save("wierdfile", state);

        var restored = store.Load("wierdfile");
        restored.Should().Be(state);
    }
}