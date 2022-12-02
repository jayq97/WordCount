using System;
using System.IO;
using System.Linq;
using WordCount;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string usage = "Usage: ./WordCount <directory-path> <file-extension>";

            static bool ArgumentsCount(int a, int b) => a == b;
            static bool ArgumentStartsWith(string a, string b) => a.StartsWith(b);

            if (ArgumentsCount(args.Length, 2))
            {
                string path = "../../../../" + args[0];
                string fileExtension = args[1];

                if (!Directory.Exists(path))
                {
                    Console.WriteLine($"Path \"{path}\" doesn't exist");
                    Console.WriteLine(usage);
                    return;
                }

                if (!ArgumentStartsWith(fileExtension, "."))
                {
                    Console.WriteLine($"Invalid file extension \"{fileExtension}\"");
                    Console.WriteLine(usage);
                    return;
                }

                var stats = WordCounter.MapReduceWordsFromFiles(path, fileExtension);

                //OutputStats.WriteToTextFile(stats);
                OutputStats.PrintToConsole(stats);
            }
            else
            {
                Console.WriteLine($"Invalid count of arguments: {args.Length}");
                Console.WriteLine(usage);
                return;
            }
        }
    }
}