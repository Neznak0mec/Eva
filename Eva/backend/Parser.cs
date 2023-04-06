using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Serilog;

namespace Eva;

class NaturalLanguageConverter
{
    private Dictionary<string, Dictionary<string,string>> patterns = new(){};
    
    Dictionary<string, int> numberMap;
    private string currentPattern = "";

    public NaturalLanguageConverter()
    {
        InitPatterns();
    }
    
    void InitPatterns()
    {
        using (StreamReader r = new StreamReader("patterns/numbers.json"))
        {
            string json = r.ReadToEnd();
            numberMap = JsonConvert.DeserializeObject<Dictionary<string, int>>(json) ?? throw new InvalidOperationException();
        }
        // var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
    }
    
    


    public string ConvertRussianNumbersToDigits(string input)
    {
        string resault = input;
        int currentNumberInDigits = 0;
        string currentNumberInWords = "";
        string[] words = input.Split(' ');
        foreach (var word in words)
        {
            if (numberMap.TryGetValue(word, out int temp))
            {
                currentNumberInWords += currentNumberInWords=="" ? "" : " "  + word;
                if (temp <1000)
                    currentNumberInDigits += temp;
                else
                    currentNumberInDigits *= temp;
            }
            else if (currentNumberInWords != "")
            {
                resault = resault.Replace(currentNumberInWords, currentNumberInDigits.ToString());
                currentNumberInDigits = 0;
                currentNumberInWords = "";
            }
            if (words.Last() == word)
            {
                resault = resault.Replace(currentNumberInWords, currentNumberInDigits.ToString());
                currentNumberInDigits = 0;
                currentNumberInWords = "";
            }
            Log.Debug(resault);
        }
        return resault;
    }



    
    public string Convert(string input)
    {
        // поиск команд по паттернам
        foreach (var pattern in patterns[currentPattern])
        {
            while (Regex.IsMatch(input, pattern.Key))
            {
                // замена команды на петтерн
                input = Regex.Replace(input, pattern.Key, pattern.Value);
            }
        }
        return input;
        // return ConvertRussianNumbersToDigits(input);
    }
}