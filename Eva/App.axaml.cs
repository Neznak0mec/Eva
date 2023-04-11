using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using window.ViewModels;
using window.Views;
using Eva;

namespace window;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        MainWindowViewModel window = new MainWindowViewModel();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop) // создание окна в классическом режиме WinForms
        {
            Task backEndTask = Task.Run(async () => await new BackEnd(window).Start()); // запуск BackEnd в отдельном потоке

            desktop.MainWindow = new MainWindow
            {
                DataContext = window,
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
