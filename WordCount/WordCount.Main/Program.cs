using System.Diagnostics;

namespace WordCount // Note: actual namespace depends on the project name.
{
    public class Program
    {
        //Usage shows how the .exe should be executed 
        const string usage = "Usage: ./WordCount <directory-path> <file-extension>";

        //Lambda Checks if the given Argument is 2 long
        static bool IsArgumentsCountTwo(int argCount) => argCount == 2;
        //Lambda Checks if the fileextenstion start with a dot 
        static string ArgumentStart(string fileextension) => 
            fileextension.StartsWith('.') ? fileextension : "." + fileextension;

        // An Char-Array of seperators to filter them
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

        // Lambda-function to read all the lines from a file
        private static readonly Func<List<string>, IEnumerable<string>> GetAllLinesFromFiles = (files) => // lambda expression
        {
            List<string> lines = new();
            files.ForEach(file =>
            {
                lines.AddRange(File.ReadAllLines(file).ToList());
            });
            return lines;
        };

        // Lambda-function to split the lines of a file and count the different words
        public static readonly Func<IEnumerable<string>, Dictionary<string, int>> MapReduceWordsFromFiles = (lines) => // lambda expression
        {
            var stats = lines
                .AsParallel() // Parallel execution
                .SelectMany(line => line.Split(separators, StringSplitOptions.RemoveEmptyEntries)) // map to single words
                .GroupBy(word => word.ToLower()) // group the words
                .ToDictionary(group => group.Key, group => group.Count()); // create dictionary

            return stats;
        };

        static void Main(string[] args)
        {
            var sw = Stopwatch.StartNew();

            if (IsArgumentsCountTwo(args.Length))
            {
                string path =  args[0]; //Path
                string fileExtension = ArgumentStart(args[1]); // Checking File-Extension

                if (!Directory.Exists(path)) //Checking if directory exists
                {
                    PrintLinesToConsole(new List<string> { $"Path \"{path}\" doesn't exist", usage });
                    return;
                }

                // Getting all file paths 
                List<string> files = Directory.EnumerateFiles(path, "*" + fileExtension, SearchOption.AllDirectories).ToList();

                // Getting every line from files
                IEnumerable<string> lines = GetAllLinesFromFiles(files);

                // Getting all counted words
                var stats = MapReduceWordsFromFiles(lines);

                // Printing to the console
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