using System;
using System.IO;
using System.Linq;
using WordCount;

namespace MyApp // Note: actual namespace depends on the project name.
{
    public class Program
    {
        
        static void Main(string[] args)
        {
            const string usage = "Usage: ./WordCount <directory-path> <file-extension>";

            static bool ArgumentsCount(int a, int b) => a == b;
            static string ArgumentStart(string fileextension) => (fileextension.StartsWith('.') ? fileextension : (fileextension = "." + fileextension));


            if (ArgumentsCount(args.Length, 2))
            {
                string path =  args[0];
                string fileExtension = ArgumentStart(args[1]);


                if (!Directory.Exists(path))
                {
                    OutputStats.PrintLinesToConsole(new string[] { $"Path \"{path}\" doesn't exist",usage });
                    return;
                }

                var stats = WordCounter.MapReduceWordsFromFiles(path, fileExtension);

                OutputStats.WriteToTextFile(stats);
                OutputStats.PrintToConsole(stats);
            }
            else
            {
                OutputStats.PrintLinesToConsole(new string[] {$"Invalid count of arguments: {args.Length}", usage});
                return;
            }
        }
    }
}