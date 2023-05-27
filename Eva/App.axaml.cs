using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using window.ViewModels;
using window.Views;
using Eva;

namespace window;

public partial class App : Application
{
    Window mainWindow;
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

            mainWindow = desktop.MainWindow;
            
            desktop.MainWindow = new MainWindow
            {
                DataContext = window,
            };

            desktop.MainWindow.Closing += (sender, e) =>
            {
                e.Cancel = true;
                desktop.MainWindow.Hide();
            };
            
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void Show_OnClick(object? sender, EventArgs e)
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow.Show();
            desktop.MainWindow.Activate();
        }
    }

    private void Close_OnClick(object? sender, EventArgs e)
    {
       if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
           desktop.Shutdown();
    }
}
