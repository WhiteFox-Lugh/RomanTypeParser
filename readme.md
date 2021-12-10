# RomanTypeParser

## 概要

ローマ字入力（日本語）のタイピングゲームを作るときに必要な複数の入力パターンに対応した判定機を作成する。

## 構成

- JsonData:
  - romanTypingParseDictionary.json: 文章のパターンからローマ字入力での入力パターンへのマッピングを記述したファイル
- RomanTypingParser: 判定機を作成するクラスライブラリ
- ParserConsole: 文章（ひらがな、半角英数字）を入力すると区切りを表示してくれる簡易コンソールアプリ
- RomanTypingParserTest: xUnit.net で書かれたユニットテスト

## ライセンス

[MIT ライセンス](https://en.wikipedia.org/wiki/MIT_License) です。
