using System.Text.RegularExpressions;
using Eva.Parsers;
using Newtonsoft.Json;
using Serilog;
using window.ViewModels;

namespace Eva;

class NaturalLanguageConverter
{
    private Dictionary<string, string> templates = new();

    NumbersConvertr numberMap = new NumbersConvertr();
    private string currentPattern = "";


    public NaturalLanguageConverter()
    {
        InitPatterns();
    }

    void InitPatterns()
    {
        try
        {
//            numberMap = ReadJsonSI("patterns/standard/numbers.json");
            templates = ReadJsonSS("patterns/python.json");
        }
        catch (Exception e)
        {
            Log.Error("parser\tFailed to read one of the standard pattern files. Error: {Exeption}", e);
            throw;
        }

        // var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
    }

    Dictionary<string, int> ReadJsonSI(string path)
    {
        using StreamReader r = new StreamReader(path);
        string json = r.ReadToEnd();
        return JsonConvert.DeserializeObject<Dictionary<string, int>>(json) ?? throw new InvalidOperationException();
    }

    Dictionary<string, string> ReadJsonSS(string path)
    {
        using StreamReader r = new StreamReader(path);
        string json = r.ReadToEnd();
        return JsonConvert.DeserializeObject<Dictionary<string, string>>(json) ?? throw new InvalidOperationException();
    }


    public string Convert(string input, MainWindowViewModel window)
    {
//         поиск команд по паттернам
        foreach (var pattern in templates)
        {
            while (Regex.IsMatch(input, pattern.Key))
            {
                window.ShowLastCommand = input;
//                 замена команды на петтерн
                input = Regex.Replace(input, pattern.Key, pattern.Value);
            }
        }
        Log.Information("parser\t{input}", input);
        return numberMap.ConvertRussianNumbersToDigits(input);
    }
}
