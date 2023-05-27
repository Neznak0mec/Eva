using System.Text.RegularExpressions;
using DynamicData;
using Eva.DataBase;
using Eva.Parsers;
using Newtonsoft.Json;
using Serilog;
using window.ViewModels;

namespace Eva;

class NaturalLanguageConverter
{
    private Dictionary<string, string> templates = new();

    private Dictionary<string, int> numberMap = new();
    List<Pattern> patterns = new();
    private NumbersConvertr NumbersConvertr;
    private string currentPattern = "";


    public NaturalLanguageConverter()
    {
        try
        {
            LoadPatterns();
        }
        catch (Exception e)
        {
            Log.Error("parser\tFailed to read one of the standard pattern files. Error: {Exeption}", e);
            throw;
        }

        NumbersConvertr = new NumbersConvertr(numberMap);
    }

    void LoadPatterns()
    {
        DataBase.DataBase dataBase = new DataBase.DataBase();
        patterns = dataBase.LoadPatterns();

        try
        {
            numberMap = ReadJsonSI("patterns/standart/numbers.json");
        }
        catch (Exception e)
        {
            Pattern? pattern = patterns.FindLast(x => x.name == "numbers");
            if (pattern == null)
            {
                Log.Error("parser\tFailed to load pattern for numbers");
            }
            else
            {
                patterns.Remove(pattern);
                Dictionary<string, int> tempDictionary = pattern.patterns.Keys.ToDictionary(key => key, key => System.Convert.ToInt32(pattern.patterns[key]));
                numberMap = tempDictionary;
            }
           
        }

        try
        {
            string path = @"patterns/";
            string[] files = Directory.GetFiles(path, "*.json");

            foreach (string file in files)
            {
                string json = File.ReadAllText(file);
                var obj = JsonConvert.DeserializeObject<dynamic>(json);
                var innerDict = new Dictionary<string, string>();
            
                string name = obj.name;
        
            
                foreach (var prop in obj)
                    if (prop.Name != "name")
                        innerDict.Add(prop.Name, prop.Value.ToString());

                if (patterns.Find(x => x.name == name) != null)
                    patterns!.Replace(patterns.Find(x => x.name == name), new Pattern(){name = name, patterns = innerDict} );
                else
                    patterns.Add(new Pattern(){name = name, patterns = innerDict});
            }
        }
        catch (Exception e)
        {
            Log.Warning("parser\tFolder with patterns not found");
        }
        
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
        if (input.StartsWith("изменить паттерн на "))
        {
            string newPatternName = input.Split().Last();
            var  res = patterns.Find(x => x.name == newPatternName);
            if (res == null)
            {
                return $"Не удалось найти паттерн {newPatternName}";
            }
            else
            {
                templates = res.patterns;
                return $"Патерн изменён на {newPatternName}";
            }
        }
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
        return NumbersConvertr.ConvertRussianNumbersToDigits(input);
    }
}