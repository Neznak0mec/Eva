using ReactiveUI;
using Serilog;

namespace window.ViewModels;

public class MainWindowViewModel : ViewModelBase
{

    public MainWindowViewModel()
    {
        ShowLastCommand = "последняя распознаная команда";
        ShowLastMessage = "привет, ГОВОРИ -_-";
        ShowLastCode = "последний распознаный код";
    }
    private string _lastCommand;
    public string ShowLastCommand
    {
        get => _lastCommand;
        set => this.RaiseAndSetIfChanged(ref _lastCommand, value);
    }
    private string _lastMessage;
    public string ShowLastMessage
    {
        get => _lastMessage;
        set => this.RaiseAndSetIfChanged(ref _lastMessage, value);
    }
    private string _lastCode;
    public string ShowLastCode
    {
        get => _lastCode;
        set => this.RaiseAndSetIfChanged(ref _lastCode, value);
    }
    public void CloseWindow()
    {
        System.Console.WriteLine("хуй");
    }

}
