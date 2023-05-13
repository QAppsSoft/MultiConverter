using MultiConverter.Models.Presets.Base;

namespace MultiConverter.ViewModelsFixtures.Helper;

public static class OptionsHelper
{
    public static IEnumerable<Type> GetOptionsSubclasses()
    {
        return typeof(OptionBase).Assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(OptionBase)));
    }
}
