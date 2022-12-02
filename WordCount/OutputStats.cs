using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordCount
{
    public class OutputStats
    {
        public static void WriteToTextFile(in Dictionary<string, int> stats)
        {
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
                using StreamWriter sw = new StreamWriter(fs);

                sw.WriteLine($"Total word count: {stats.Count}");

                foreach (var pair in stats.OrderByDescending(x => x.Value).ThenByDescending(x => x.Key))
                {
                    sw.WriteLine($"  {pair.Value} {pair.Key}");
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
        }

        public static void PrintToConsole(in Dictionary<string, int> stats)
        {
            Console.WriteLine($"Total word count: {stats.Count}");
            foreach (var pair in stats.OrderByDescending(x => x.Value).ThenByDescending(x => x.Key))
            {
                Console.WriteLine($"  {pair.Value} {pair.Key}");
            }
        }
    }
}
