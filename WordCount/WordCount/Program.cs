using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using WordCount;
using static System.Net.WebRequestMethods;

namespace MyApp // Note: actual namespace depends on the project name.
{
    public class Program
    {
        const string usage = "Usage: ./WordCount <directory-path> <file-extension>";
        static bool ArgumentsCount(int a, int b) => a == b;
        static string ArgumentStart(string fileextension) => (fileextension.StartsWith('.') ? fileextension : (fileextension = "." + fileextension));
        public static void PrintLinesToConsole(in string[] lines)
        {
            lines.ToList().ForEach(x => Console.WriteLine(x));
        }
        public static void PrintToConsole(in Dictionary<string, int> stats,Stopwatch sw)
        {
            
            Console.WriteLine($"Total word count: {stats.Count}");
            foreach (var pair in stats.OrderByDescending(x => x.Value).ThenByDescending(x => x.Key))
            {
                Console.WriteLine($"  {pair.Value} {pair.Key}");
            }
            Console.WriteLine("Execution time = {0} seconds", sw.Elapsed.TotalSeconds);
        }

        public static Func<List<string>,IEnumerable<string>> GetAllLinesFromFiles = (files) =>
        {
            List<string> lines = new();
            files.ForEach(f =>
            {
                lines.AddRange(System.IO.File.ReadAllLines(f).ToList());
            });
            return lines;
        };


        public static Func<string,string,Dictionary<string, int>> MapReduceWordsFromFiles = (string path, string fileExtension) =>
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
        };

        static void Main(string[] args)
        {
            var sw = Stopwatch.StartNew();
            if (ArgumentsCount(args.Length, 2))
            {
                string path =  args[0];
                string fileExtension = ArgumentStart(args[1]);


                if (!Directory.Exists(path))
                {
                    PrintLinesToConsole(new string[] { $"Path \"{path}\" doesn't exist",usage });
                    return;
                }

                var stats = MapReduceWordsFromFiles(path, fileExtension);

                //OutputStats.WriteToTextFile(stats);
                PrintToConsole(stats,sw);
            }
            else
            {
                PrintLinesToConsole(new string[] {$"Invalid count of arguments: {args.Length}", usage});
                return;
            }
        }
    }
}