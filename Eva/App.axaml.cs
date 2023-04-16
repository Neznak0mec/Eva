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

            var BackEnd = new BackEnd(window);
            Task backEndTask = Task.Run(async () => await BackEnd.Start()); // запуск BackEnd в отдельном потоке

            desktop.MainWindow = new MainWindow
            {
                DataContext = window,
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    public void CloseWindow()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop) // создание окна в классическом режиме WinForms
        {
            desktop.MainWindow.Close();
        }
    }
}
