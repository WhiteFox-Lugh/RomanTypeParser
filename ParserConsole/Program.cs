using System.IO;
using System.Text;
using RomanTypingParser;

class Program
{
    static void Main(string[] args)
    {
        RomanTypingParserJp.ReadJsonFile();
        Console.WriteLine($"Type sentence: ");

        string? input = Console.ReadLine();
        if (string.IsNullOrEmpty(input))
        {
            return;
        }
        Console.WriteLine($"{Environment.NewLine}Input: {input}{Environment.NewLine}");
        var parseResult = RomanTypingParserJp.ConstructTypeSentence(input);

        for (int i = 0; i < parseResult.parsedSentence.Count; ++i)
        {
            var partStr = parseResult.parsedSentence[i];
            Console.WriteLine($"Part {i} : {partStr}");
            var strBuilder = new StringBuilder();
            for (int j = 0; j < parseResult.judgeAutomaton[i].Count; ++j)
            {
                if (j == parseResult.judgeAutomaton[i].Count - 1)
                {
                    strBuilder.Append(parseResult.judgeAutomaton[i][j]);
                }
                else
                {
                    strBuilder.Append($"{parseResult.judgeAutomaton[i][j]}, ");
                }
            }
            Console.WriteLine(strBuilder.ToString());
        }
        return;
    }
}