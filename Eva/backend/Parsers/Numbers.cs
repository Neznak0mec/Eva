using Serilog;

namespace Eva.Parsers;

public class NumbersConvertr
{
    
    private readonly Dictionary<string, long> numberMap = new Dictionary<string, long>()
    {
        {"ноль", 0},
        {"один", 1},
        {"одна",1},
        {"два", 2},
        {"две", 2},
        {"три", 3},
        {"четыре", 4},
        {"пять", 5},
        {"шесть", 6},
        {"семь", 7},
        {"восемь", 8},
        {"девять", 9},
        {"десять", 10},
        {"одиннадцать", 11},
        {"двенадцать", 12},
        {"тринадцать", 13},
        {"четырнадцать", 14},
        {"пятнадцать", 15},
        {"шестнадцать", 16},
        {"семнадцать", 17},
        {"восемнадцать", 18},
        {"девятнадцать", 19},
        {"двадцать", 20},
        {"тридцать", 30},
        {"сорок", 40},
        {"пятьдесят", 50},
        {"шестьдесят", 60},
        {"семьдесят", 70},
        {"восемьдесят", 80},
        {"девяносто", 90},
        {"сто", 100},
        {"двести", 200},
        {"триста", 300},
        {"четыреста", 400},
        {"пятьсот", 500},
        {"шестьсот", 600},
        {"семьсот", 700},
        {"восемьсот", 800},
        {"девятсот", 900},
        {"тысяча", 1000},
        {"тысяч", 1000},
        {"тысячи", 1000},
        {"миллион", 1000000},
        {"миллиона", 1000000},
        {"миллионов", 1000000},
        {"миллиард", 1000000000},
    };

    public string ConvertRussianNumbersToDigits(string input)
    {
        if (!numberMap.Keys.Any(input.Contains))
            return input;
        
        string resault = input;
        long currentNumberInDigits = 0;
        string currentNumberInWords = "";
        string[] words = input.Split(' ');
        foreach (var word in words)
        {
            if (numberMap.TryGetValue(word, out long temp))
            {
                currentNumberInWords += (currentNumberInWords == "" ? "" : " ")  + word;
                if (temp < 1000)
                    currentNumberInDigits += temp;
                else
                    currentNumberInDigits *= temp;
            }
            else if (currentNumberInWords != "")
            {
                resault = input.Replace(currentNumberInWords, currentNumberInDigits.ToString());
                input = resault;
                currentNumberInDigits = 0;
                currentNumberInWords = "";
            }
            if (words.Last() == word)
            {
                resault = input.Replace(currentNumberInWords, currentNumberInDigits.ToString());
                input = resault;
                currentNumberInDigits = 0;
                currentNumberInWords = "";
            }
        }
        return resault;
    }

}