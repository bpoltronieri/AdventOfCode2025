using System.Diagnostics;
using System.Reflection;

namespace AoC2025
{
    class Program
    {
        static void Main(string[] args)
        {
            var day = 0;
            while (day == 0)
            {
                Console.WriteLine();
                Console.WriteLine("Enter day: ");
                var command = Console.ReadLine();

                if (!int.TryParse(command, out day) || day < 1 || day > 12)
                    {
                        day = 0;
                        Console.WriteLine("Invalid Day");
                    }
            }

            IDay? solution;
            try
            {
                var dayName = $"Day{day:d2}";
                var typeName = "AoC2025.Days." + dayName;
                Type dayType = Type.GetType(typeName) ?? throw new InvalidDataException("Could not find dayType");

                string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? throw new InvalidOperationException("Could not find path");
                path = Path.GetFullPath(Path.Combine(path, "..", "..", "..", "..", ".."));
                
                string inputFile = Directory.GetFiles(path + @"\Input", dayName + ".txt")[0] ?? throw new InvalidOperationException("Could not find input file");

                Stopwatch stopwatch1 = Stopwatch.StartNew(); 
                solution = (IDay?)Activator.CreateInstance(dayType, inputFile);
                stopwatch1.Stop();
                if (solution == null) 
                    throw new InvalidOperationException("Could not load solution");
                else
                    Console.WriteLine("Time to load input: {0}", stopwatch1.Elapsed);
            }
            catch
            {
                Console.WriteLine("Error loading solution");
                return;
            }

            var part = 0;
            while (part == 0)
            {
                Console.WriteLine();
                Console.WriteLine("Which part would you like to solve?");
                var command = Console.ReadLine();

                if (!int.TryParse(command, out part) || (part != 1 && part != 2))
                    {
                        part = 0;
                        Console.WriteLine("Invalid Part");
                    }
            }

            string answer = "";
            Stopwatch stopwatch2 = Stopwatch.StartNew(); 
            switch (part)
            {
                case 1:
                    answer = solution.PartOne();
                    break;
                case 2:
                    answer = solution.PartTwo();
                    break;
            }
            stopwatch2.Stop();
            Console.WriteLine("Answer: " + answer);
            Console.WriteLine("Time elapsed: {0}", stopwatch2.Elapsed);
        }
    }
}