using System;
using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using MultiConverter.DependencyInjection;
using Splat;

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

    private static void RegisterDependencies() => Bootstrapper.Register(Locator.CurrentMutable, Locator.Current);

    private static void RunBackgroundTasks() => BackgroundTasksRunner.Start(Locator.Current);

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
    {
        RegisterDependencies();
        RunBackgroundTasks();

        return AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .UseReactiveUI()
            .LogToTrace();
    }
}
