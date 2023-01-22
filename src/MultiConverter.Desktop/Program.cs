using System;
using System.Reflection;
using System.Threading;
using Autofac;
using Avalonia;
using Avalonia.Controls;
using MultiConverter.DependencyInjection;
using MultiConverter.Extension;
using ReactiveUI;
using Splat;
using Splat.Autofac;

namespace MultiConverter.Desktop;

internal static class Program
{
    private const int TimeoutSeconds = 3;

    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        Mutex mutex = new Mutex(false, typeof(Program).FullName);

        try
        {
            if (!mutex.WaitOne(TimeSpan.FromSeconds(TimeoutSeconds), true))
            {
                return;
            }

            BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args, ShutdownMode.OnMainWindowClose);
        }
        finally
        {
            mutex.ReleaseMutex();
        }
    }

    private static void InitializeAutofac()
    {
        // Build a new Autofac container.
        var builder = new ContainerBuilder();
        builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).AsImplementedInterfaces();

        // Use Autofac for ReactiveUI dependency resolution.
        // After we call the method below, Locator.Current and
        // Locator.CurrentMutable start using Autofac locator.
        AutofacDependencyResolver resolver = new(builder);
        Locator.SetLocator(resolver);

        // These .InitializeX() methods will add ReactiveUI platform
        // registrations to your container. They MUST be present if
        // you *override* the default Locator.
        Locator.CurrentMutable.InitializeSplat();
        Locator.CurrentMutable.InitializeReactiveUI();
        Locator.CurrentMutable.InitializeAvalonia();

        RegisterDependencies();

        var container = builder.Build();
        resolver.SetLifetimeScope(container);
    }

    private static void RegisterDependencies() => Bootstrapper.Register(Locator.CurrentMutable, Locator.Current);

    private static void RunBackgroundTasks() => BackgroundTasksRunner.Start(Locator.Current);

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
    {
        InitializeAutofac();
        RunBackgroundTasks();

        return AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace();
    }
}
