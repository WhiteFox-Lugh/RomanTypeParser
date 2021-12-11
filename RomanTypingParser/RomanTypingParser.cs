namespace RomanTypingParser;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

public static class RomanTypingParserJp
{
  private static readonly string dictionaryJsonFilePath = @"D:/Arthur-Lugh/develop/RomanTypingParserJapanese/JsonData/romanTypingParseDictionary.json";
  private static readonly Dictionary<string, string[]> mappingDict = new();

  /// <summary>
  /// json ファイルを読み込んで、
  /// 「ひらがな」から「ローマ字での打ち方」へのマッピングデータを
  /// mappingDict に代入する
  /// </summary>
  public static void ReadJsonFile()
  {
    if (mappingDict.Count != 0) { return; }
    var sr = new StreamReader(dictionaryJsonFilePath, Encoding.GetEncoding("utf-8"));
    var jsonStr = sr.ReadToEnd();
    sr.Close();

    var jsonData = JsonSerializer.Deserialize<RomanMapping[]>(jsonStr);
    if (jsonData != null)
    {
      foreach (var mapData in jsonData)
      {
        mappingDict.Add(mapData.Pattern, mapData.TypePattern);
      }
    }
    return;
  }

  /// <summary>
  /// ひらがな文をパースして、判定を作成
  /// <param name="sentenceHiragana">パースされるひらがな文字列</param>
  /// <returns>判定器</returns>
  /// </summary>
  public static (List<string> parsedSentence, List<List<string>> judgeAutomaton) ConstructTypeSentence(string sentenceHiragana)
  {
    int idx = 0;
    string uni, bi, tri;
    var judge = new List<List<string>>();
    var parsedStr = new List<string>();
    while (idx < sentenceHiragana.Length)
    {
      List<string> validTypeList;
      uni = sentenceHiragana[idx].ToString();
      bi = (idx + 1 < sentenceHiragana.Length) ? sentenceHiragana.Substring(idx, 2) : "";
      tri = (idx + 2 < sentenceHiragana.Length) ? sentenceHiragana.Substring(idx, 3) : "";
      if (mappingDict.ContainsKey(tri))
      {
        validTypeList = mappingDict[tri].ToList();
        idx += 3;
        parsedStr.Add(tri);
      }
      else if (mappingDict.ContainsKey(bi))
      {
        validTypeList = mappingDict[bi].ToList();
        idx += 2;
        parsedStr.Add(bi);
      }
      else
      {
        validTypeList = mappingDict[uni].ToList();
        // 文末「ん」の処理
        if (uni.Equals("ん") && sentenceHiragana.Length - 1 == idx)
        {
          validTypeList.Remove("n");
        }
        idx++;
        parsedStr.Add(uni);
      }
      judge.Add(validTypeList);
    }
    return (parsedStr, judge);
  }
}

class RomanMapping
{
  // パースされるときのパターン
  public string Pattern { get; set; } = "";
  // パターンに対するローマ字入力の打ち方
  public string[] TypePattern { get; set; } = new string[1] { "" };
}

