using System.Linq;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {

            if (args.Length == 0)
            {
                Console.WriteLine("Missing Argukments");
                return;
            }

            if (args.Length == 1)
            {
                if (args[0].ToLower() == "-r")
                {
                    Console.WriteLine("Missing Path");
                    return;
                }
                if (!Directory.Exists(args[0])){
                    Console.WriteLine("Path doesn't exist");
                    return ;
                }

                // nicht rekusiv

            }
            if (args.Length == 2)
            {
                if(!args.Any(x => x.ToLower() == "-r"))
                {
                    Console.WriteLine("Not correct Arguments");
                    return;
                }

                string[] path = Array.FindAll(args, x => x.ToLower() != "-r" );

                if (!Directory.Exists(path[0]))
                {
                    Console.WriteLine("Path doesn't exist");
                    return;
                }

                // Rekursiv
            }
            if (args.Length > 2)
            {
                Console.WriteLine("To Many Arguments");
                return;
            }
            Console.ReadKey();
        }
    }
}