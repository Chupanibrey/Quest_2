using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Quest_2
{
    class Program
    {
        static void Main(string[] args)
        {
            var text = File.ReadAllText("WarAndPeace_1.txt");
            var file = new StreamWriter("UniqueWords.txt", false, Encoding.UTF8);
            var sentences = ParseSentences(text);
            var uniqueWords = sentences.SelectMany(word => word)
                .GroupBy(word => word)
                .Select(g => new { Word = g.Key, Count = g.Count() })
                .OrderByDescending(w => w.Count)
                .ThenBy(w => w.Word);

            foreach (var w in uniqueWords)
                file.WriteLine("{0} - {1}", w.Word, w.Count);
        }

        public static List<List<string>> ParseSentences(string text)
        {
            var textList = new List<List<string>>();
            var sentencesList = new List<string>();

            var sentences = text.Split('.', '!', '?', ';', ':', '(', ')');
            string[] stringSeparators = new string[]
            { " ", ":", "-", ",", "\t", "\r", "\n", "^", "#", "$", "+", "=", "1", "\"", "—", "…" };

            SplitSentence(sentences, stringSeparators, textList);

            return textList;
        }

        public static void SplitSentence(string[] sentences, string[] sentencesSeparators, List<List<string>> textList)
        {
            StringBuilder correctWord = new StringBuilder();
            foreach (var moreWord in sentences)
            {
                var words = moreWord.Split(sentencesSeparators, StringSplitOptions.RemoveEmptyEntries);
                var wordList = new List<string>();
                foreach (var oneWord in words)
                {
                    correctWord.Clear();
                    foreach (var simbol in oneWord)
                    {
                        if (char.IsLetter(simbol) || simbol == '\'') correctWord.Append(simbol);
                    }
                    if (correctWord.Length > 0) wordList.Add(correctWord.ToString().ToLower());
                }
                if (wordList.Count > 0) textList.Add(wordList);
            }
        }
    }
}
