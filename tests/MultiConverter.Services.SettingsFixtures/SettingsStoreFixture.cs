using System.Text.Json;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using MultiConverter.Services.Abstractions.Settings;
using MultiConverter.Services.Settings;
using NUnit.Framework;

namespace MultiConverter.Services.SettingsFixtures;

public class SettingsStoreFixture
{
    [Test]
    public void Write_Custom_State_Value_Should_Succeed()
    {
        const string key = "WriteCustomStateTestFile";
        const string testJsonValue = "{\"TestValue\" : 1}";

        using TemporalFile settingFile = TemporalFile.Create($"{key}.setting");
        var state = new State(1, testJsonValue);
        var store = new FileSettingsStore(NullLogger.Instance, settingFile);

        store.Save(key, state);
        var restored = store.Load(key);

        restored.Should().BeEquivalentTo(state, o => o.ComparingByMembers<State>());
    }

    [Test]
    public void Write_Custom_Record_Struct_Should_Succeed()
    {
        const string key = "WriteCustomRecordStructTestFile";
        var value = new TestStruct("None", 10);

        var jsonValue = JsonSerializer.Serialize(value, new JsonSerializerOptions { WriteIndented = true });
        var state = new State(1, jsonValue);

        using TemporalFile settingFile = TemporalFile.Create($"{key}.setting");
        var store = new FileSettingsStore(NullLogger.Instance, settingFile);

        store.Save(key, state);
        var restored = store.Load(key);

        restored.Should().BeEquivalentTo(state, o => o.ComparingByMembers<State>());
    }
}

public readonly record struct TestStruct(string Name, int Age);
