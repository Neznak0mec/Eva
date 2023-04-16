using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace window.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        this.Topmost = true;
        // Установка свойств StartPosition и Bounds
        this.WindowStartupLocation = WindowStartupLocation.Manual; // Устанавливаем стартовую позицию вручную
        this.Bounds = new Rect(
            // Устанавливаем координаты и размеры окна так, чтобы оно находилось в правом верхнем углу экрана
            Screens.Primary.WorkingArea.Width -Width,
            0, // Верхняя граница экрана
            this.Width, // Ширина окна
            this.Height // Высота окна
        );


        this.Closing += (sender, e) =>
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
            {
                e.Cancel = true; // Отменяем закрытие окна
                this.Hide(); // Скрываем окно
                this.WindowState = WindowState.Minimized; // Минимизируем окно
            }
        };
    }

    public void ReturnToShow()
    {
        this.Show();
    }

    public void CloseWindow()
    {
        Close();
    }
}
