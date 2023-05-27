using Avalonia.Controls;

namespace window.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    public void CloseWindow()
    {
        this.Close();
    }
}
