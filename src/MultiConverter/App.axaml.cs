using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using MultiConverter.DependencyInjection;
using MultiConverter.Services.Abstractions;
using MultiConverter.ViewModels;
using MultiConverter.Views;
using Splat;

namespace MultiConverter
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            var mainViewModel = GetRequiredService<MainViewModel>();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var mainWindow = new MainWindow
                {
                    DataContext = mainViewModel
                };

                desktop.MainWindow = mainWindow;

                MainWindow = mainWindow;
            }
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
            {
                singleViewPlatform.MainView = new MainView
                {
                    DataContext = mainViewModel
                };
            }

            Init();

            base.OnFrameworkInitializationCompleted();
        }

        public static MainWindow MainWindow { get; private set; }

        private static void Init() => _ = GetRequiredService<ISystemSetterJob>();

        private static T GetRequiredService<T>() => Locator.Current.GetRequiredService<T>();
    }
}
