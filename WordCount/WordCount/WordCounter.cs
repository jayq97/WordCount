using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

namespace WordCount
{
    public class WordCounter
    {
        public static Dictionary<string, int> MapReduceWordsFromFiles(in string path, in string fileExtension)
        {

            char[] separators = new[] { ' ', '.', ',', ';', ':', '(', ')', '-', '?', '!',
                    '\n', '\r', '\t', '\"', '\'', '\\', '*', '/', '<', '@', '#', '[', ']', '_',
                    '$', '~', '=', '<', '>', '%', '+', ';', '{', '}' };

            var files = Directory.EnumerateFiles(path, "*" + fileExtension, SearchOption.AllDirectories);

            var stats = 
                GetAllLinesFromFiles(files.ToList())
                .AsParallel()
                .SelectMany(line => line.Split(separators, StringSplitOptions.RemoveEmptyEntries))
                .GroupBy(word => word)
                .ToDictionary(group => group.Key, group => group.Count());

            return stats;
        }

        private static IEnumerable<string> GetAllLinesFromFiles(in List<string> files)
        {
            List<string> lines = new();
            files.ForEach(f =>
            {
                lines.AddRange(File.ReadAllLines(f).ToList());
            });
            return lines;
        }
    }
}
