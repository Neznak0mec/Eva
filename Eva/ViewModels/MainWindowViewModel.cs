using ReactiveUI;
using Serilog;

namespace window.ViewModels;

public class MainWindowViewModel : ViewModelBase
{

    public MainWindowViewModel()
    {
        ShowLast = "последняя команда";
        
    }
    private string _lastCommand;

    public string ShowLast
    {
        get => _lastCommand;
        set => this.RaiseAndSetIfChanged(ref _lastCommand, value);
    }
}
