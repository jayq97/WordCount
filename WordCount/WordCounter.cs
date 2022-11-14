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

            List<string> files = AddFilesRecursive(new List<string>(), path, fileExtension);

            foreach (string file in files)
            {
                byte[] byteArray = File.ReadAllBytes(file);
                string text = Encoding.Default.GetString(byteArray);
                char[] chars = { ' ', '.', ',', ';', ':', '(', ')', '-', '?', '!', 
                    '\n', '\r', '\"', '\'', '*', '/', '<', '@', '#', '[', ']', '_', 
                    '$', '~', '=', '<', '>', '%', '+' };

                // split words
                string[] words = text.Split(chars);

                // iterate over the words collection to count occurrences
                foreach (string word in words)
                {
                    if (word.Length > 0)
                    {
                        if (!stats.ContainsKey(word))
                        {
                            stats.Add(word, 1); // add new word to collection
                        }
                        else
                        {
                            stats[word] += 1; // update word occurrence count
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
                    string countWithWordString = $"  {pair.Value} {pair.Key}{Environment.NewLine}";
                    byte[] countWithWordByte = new UTF8Encoding(true).GetBytes(countWithWordString);

                    fs.Write(countWithWordByte, 0, countWithWordByte.Length);
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
        }

        private static List<string> AddFilesRecursive(List<string> files, string path, string fileExtension)
        {
            try
            {
                foreach (string f in Directory.GetFiles(path, "*" + fileExtension))
                {
                    files.Add(f);
                }

                foreach (string d in Directory.GetDirectories(path))
                {
                    files = AddFilesRecursive(files, d, fileExtension);
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }

            return files;
        }
    }
}
