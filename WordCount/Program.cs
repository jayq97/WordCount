using System.Linq;
using WordCount;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string usage = "Usage: ./WordCount <directory-path> <file-extension>";

            if (args.Length < 2)
            {
                Console.WriteLine($"Too few Arguments{Environment.NewLine}{usage}");
                return;
            }

            if (args.Length > 2)
            {
                Console.WriteLine($"Too many Arguments{Environment.NewLine}{usage}");
                return;
            }

            if (args.Length == 2)
            {
                string path = "../../../../" + args[0];
                string fileExtension = args[1];

                if (!Directory.Exists(path))
                {
                    Console.WriteLine($"Path doesn't exist{Environment.NewLine}{usage}");
                    return;
                }

                if (!fileExtension.Trim().StartsWith("."))
                {
                    Console.WriteLine($"Invalid file extension{Environment.NewLine}{usage}");
                    return;
                }

                WordCounter.Print(path, fileExtension);
            }
        }
    }
}