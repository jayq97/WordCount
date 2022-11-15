using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;

namespace WordCount
{
    static class WordCounter
    {
        public static void Print(string path, string fileExtension)
        {
            Dictionary<string, int> stats = new Dictionary<string, int>();
            string[] files = Directory.GetFiles(path, "*" + fileExtension, SearchOption.AllDirectories);

            foreach (string file in files)
            {
                string text;

                using (StreamReader reader = new StreamReader(file, Encoding.UTF8))
                {
                    text = reader.ReadToEnd();
                }

                char[] separators = { ' ', '.', ',', ';', ':', '(', ')', '-', '?', '!', 
                    '\n', '\r', '\t', '\"', '\'', '\\', '*', '/', '<', '@', '#', '[', ']', '_',
                    '$', '~', '=', '<', '>', '%', '+', ';', '{', '}' };

                // split words
                string[] words = text.Split(separators);

                // iterate over the words collection to count occurrences
                foreach (string word in words)
                {
                    string wordToLower = word.ToLower();
                    if (wordToLower.Length > 0)
                    {
                        if (!stats.ContainsKey(wordToLower))
                        {
                            stats.Add(wordToLower, 1); // add new word to collection
                        }
                        else
                        {
                            stats[wordToLower] += 1; // update word occurrence count
                        }
                    }
                }
            }

            // order the collection by word count, and if word count is equal, then order alphabetically
            var orderedStats = stats.OrderByDescending(x => x.Value).ThenByDescending(x => x.Key);

            try
            {
                string fileName = @"../../../../result.txt";

                // Check if file already exists. If exists, delete it.     
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                // Create new file     
                using FileStream fs = File.Create(fileName);
                byte[] totalWords = new UTF8Encoding(true).GetBytes($"Total word count: {stats.Count}{Environment.NewLine}");
                fs.Write(totalWords, 0, totalWords.Length);

                foreach (var pair in orderedStats)
                {
                    byte[] countWithWordByte = new UTF8Encoding(true).GetBytes($"  {pair.Value} {pair.Key}{Environment.NewLine}");
                    fs.Write(countWithWordByte, 0, countWithWordByte.Length);
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
        }
    }
}
