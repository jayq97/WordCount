using System.Diagnostics;

namespace WordCount // Note: actual namespace depends on the project name.
{
    public class Program
    {
        const string usage = "Usage: ./WordCount <directory-path> <file-extension>";
        static bool IsArgumentsCountTwo(int argCount) => argCount == 2;
        static string ArgumentStart(string fileextension) => 
            fileextension.StartsWith('.') ? fileextension : "." + fileextension;

        static readonly char[] separators = new[] { ' ', '.', ',', ';', ':', '(', ')', '-', '?', '!',
                    '\n', '\r', '\t', '\"', '\'', '\\', '*', '/', '<', '@', '#', '[', ']', '_',
                    '$', '~', '=', '<', '>', '%', '+', ';', '{', '}' };

        private static void PrintLinesToConsole(in List<string> lines) // not pure due to void return value and I/O side effect
        {
            lines.ForEach(x => Console.WriteLine(x));
        }

        private static void PrintToConsole(in Dictionary<string, int> stats, in Stopwatch sw) // not pure due to void return value and I/O side effect
        {
            Console.WriteLine($"Total word count: {stats.Count}");
            foreach (var pair in stats.OrderByDescending(x => x.Value).ThenByDescending(x => x.Key))
            {
                Console.WriteLine($"  {pair.Value} {pair.Key}");
            }
            Console.WriteLine($"Execution time = {sw.Elapsed.TotalSeconds} seconds");
        }

        private static readonly Func<List<string>, IEnumerable<string>> GetAllLinesFromFiles = (files) => // lambda expression
        {
            List<string> lines = new();
            files.ForEach(file =>
            {
                lines.AddRange(File.ReadAllLines(file).ToList());
            });
            return lines;
        };

        public static readonly Func<IEnumerable<string>, Dictionary<string, int>> MapReduceWordsFromFiles = (lines) => // lambda expression
        {
            var stats = lines
                .AsParallel()
                .SelectMany(line => line.Split(separators, StringSplitOptions.RemoveEmptyEntries))
                .GroupBy(word => word.ToLower())
                .ToDictionary(group => group.Key, group => group.Count());

            return stats;
        };

        static void Main(string[] args)
        {
            var sw = Stopwatch.StartNew();

            if (IsArgumentsCountTwo(args.Length))
            {
                string path =  args[0];
                string fileExtension = ArgumentStart(args[1]);

                if (!Directory.Exists(path))
                {
                    PrintLinesToConsole(new List<string> { $"Path \"{path}\" doesn't exist", usage });
                    return;
                }

                var files = Directory.EnumerateFiles(path, "*" + fileExtension, SearchOption.AllDirectories).ToList();
                var lines = GetAllLinesFromFiles(files);
                var stats = MapReduceWordsFromFiles(lines);

                PrintToConsole(stats, sw);
            }
            else
            {
                PrintLinesToConsole(new List<string> { $"Invalid count of arguments: {args.Length}", usage });
                return;
            }
        }
    }
}