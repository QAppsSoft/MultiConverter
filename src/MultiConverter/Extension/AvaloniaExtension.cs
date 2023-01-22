using Avalonia.ReactiveUI;
using Avalonia.Threading;
using ReactiveUI;
using Splat;

namespace MultiConverter.Extension;

public static class AvaloniaExtension
{
    public static void InitializeAvalonia(this IMutableDependencyResolver resolver)
    {
        resolver.RegisterConstant(new AvaloniaActivationForViewFetcher(), typeof(IActivationForViewFetcher));
        resolver.RegisterConstant(new AutoDataTemplateBindingHook(), typeof(IPropertyBindingHook));
        RxApp.MainThreadScheduler = AvaloniaScheduler.Instance;
    }
}
