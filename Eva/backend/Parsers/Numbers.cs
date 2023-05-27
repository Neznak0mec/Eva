using Serilog;

namespace Eva.Parsers;

public class NumbersConvertr
{
    
    Dictionary<string, int> numberMap = new();

    public NumbersConvertr(Dictionary<string, int> map)
    {
        numberMap = map;
    }

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
            if (numberMap.TryGetValue(word, out int temp))
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
            if (words.Last() == word && currentNumberInWords != "")
            {
                resault = input.Replace(currentNumberInWords, currentNumberInDigits.ToString());
            }
        }
        return resault;
    }

}