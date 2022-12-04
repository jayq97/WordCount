using System.Diagnostics;

namespace WordCount // Note: actual namespace depends on the project name.
{
    public class Program
    {
        //Usage shows how the exe should be executed 
        const string usage = "Usage: ./WordCount <directory-path> <file-extension>";

        //Lambda Checks if the given Argument is 2 Long
        static bool IsArgumentsCountTwo(int argCount) => argCount == 2;
        //Lambda Checks if the fileextenstion start with a dot 
        static string ArgumentStart(string fileextension) => 
            fileextension.StartsWith('.') ? fileextension : "." + fileextension;

        // A Array of seperators to filter them
        static readonly char[] separators = new[] { ' ', '.', ',', ';', ':', '(', ')', '-', '?', '!',
                    '\n', '\r', '\t', '\"', '\'', '\\', '*', '/', '<', '@', '#', '[', ']', '_',
                    '$', '~', '=', '<', '>', '%', '+', ';', '{', '}' };

        // Non Functional Function to print to the Console
        private static void PrintLinesToConsole(in List<string> lines) // not pure due to void return value and I/O side effect
        {
            lines.ForEach(x => Console.WriteLine(x));
        }
        // Non Functional Function to print the Word Counts to the Console
        private static void PrintToConsole(in Dictionary<string, int> stats, in Stopwatch sw) // not pure due to void return value and I/O side effect
        {
            Console.WriteLine($"Total word count: {stats.Count}");
            foreach (var pair in stats.OrderByDescending(x => x.Value).ThenByDescending(x => x.Key))
            {
                Console.WriteLine($"  {pair.Value} {pair.Key}");
            }
            Console.WriteLine($"Execution time = {sw.Elapsed.TotalSeconds} seconds");
        }

        // Lambda Function to read all the lines from a File
        private static readonly Func<List<string>, IEnumerable<string>> GetAllLinesFromFiles = (files) => // lambda expression
        {
            List<string> lines = new();
            files.ForEach(file =>
            {
                lines.AddRange(File.ReadAllLines(file).ToList());
            });
            return lines;
        };

        // Lambda Function to Split the Lines of a File and Count the diffrent words
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
                //Path
                string path =  args[0];
                // Checking Fileextension
                string fileExtension = ArgumentStart(args[1]);

                //Checking if Directory Exists
                if (!Directory.Exists(path))
                {
                    PrintLinesToConsole(new List<string> { $"Path \"{path}\" doesn't exist", usage });
                    return;
                }
                // Getting all File Paths 
                List<string> files = Directory.EnumerateFiles(path, "*" + fileExtension, SearchOption.AllDirectories).ToList();
                // Getting every Line from Files
                IEnumerable<string> lines = GetAllLinesFromFiles(files);
                // Getting all countet words
                var stats = MapReduceWordsFromFiles(lines);
                // Printing to the Console
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