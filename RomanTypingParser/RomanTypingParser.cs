namespace RomanTypingParser;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using System.Text.Json;

public static class RomanTypingParserJp
{
    private static readonly string _dictionaryJsonFilePath = @"D:/Arthur-Lugh/develop/RomanTypingParserJapanese/JsonData/romanTypingParseDictionary.json";
    private static readonly Dictionary<string, string[]> _mappingDictionary = new();

    /// <summary>
    /// json ファイルを読み込んで、
    /// 「ひらがな」から「ローマ字での打ち方」へのマッピングデータを
    /// mappingDict に代入する
    /// </summary>
    public static void ReadJsonFile()
    {
        if (_mappingDictionary.Count > 0) { return; }

        var sr = new StreamReader(_dictionaryJsonFilePath, Encoding.GetEncoding("utf-8"));
        var jsonStr = sr.ReadToEnd();
        sr.Close();

        var jsonData = JsonSerializer.Deserialize<RomanMapping[]>(jsonStr);
        if (jsonData == null) { throw new InvalidDataException("Error: JsonData が null です"); }

        foreach (var mapData in jsonData)
        {
            _mappingDictionary.Add(mapData.Pattern, mapData.TypePattern);
        }
        return;
    }

    /// <summary>
    /// ひらがな文をパースして、判定を作成
    /// <param name="sentenceHiragana">パースされるひらがな文字列</param>
    /// <returns>parsedSentence は区切った文字列、judgeAutomaton は判定オートマトン</returns>
    /// </summary>
    public static (ImmutableList<string> parsedSentence, ImmutableList<ImmutableList<string>> judgeAutomaton) ConstructTypeSentence(string sentenceHiragana)
    {
        var idx = 0;
        var judge = new List<ImmutableList<string>>();
        var parsedStr = new List<string>();

        while (idx < sentenceHiragana.Length)
        {
            List<string> validTypeList;

            var uni = sentenceHiragana[idx].ToString();
            var bi = (idx + 1 < sentenceHiragana.Length) ? sentenceHiragana.Substring(idx, 2) : "";
            var tri = (idx + 2 < sentenceHiragana.Length) ? sentenceHiragana.Substring(idx, 3) : "";

            if (_mappingDictionary.ContainsKey(tri))
            {
                validTypeList = _mappingDictionary[tri].ToList();
                idx += 3;
                parsedStr.Add(tri);
            }
            else if (_mappingDictionary.ContainsKey(bi))
            {
                validTypeList = _mappingDictionary[bi].ToList();
                idx += 2;
                parsedStr.Add(bi);
            }
            else if (_mappingDictionary.ContainsKey(uni))
            {
                validTypeList = _mappingDictionary[uni].ToList();
                idx++;
                parsedStr.Add(uni);
            }
            else
            {
                throw new InvalidDataException($"Error: マッピングデータに入っていない文字列やパターンを検知しました / uni-gram => {uni}, bi-gram => {bi}, tri-gram => {tri}");
            }

            judge.Add(validTypeList.ToImmutableList());
        }

        return (parsedStr.ToImmutableList(), judge.ToImmutableList());
    }
}

public class RomanMapping
{
    // パースされるときのパターン
    public string Pattern { get; set; } = "";
    // パターンに対するローマ字入力の打ち方
    public string[] TypePattern { get; set; } = new string[1] { "" };
}

