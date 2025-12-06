using System.Buffers;
using System.Data;

namespace AoC2025.Days
{
    public class Day06(string file) : IDay
    {
        private readonly string[] input = LoadInput(file);

        private static string[] LoadInput(string file)
        {
            return File.ReadAllLines(file);
        }

        private (int[][], char[]) LoadPartOneValues()
        {
            // converts input into grid of ints and list of operations, to be used for part 1
            int[][] values = input
                        .Take(input.Length - 1)
                        .Select(s => s.Split(new char[0], StringSplitOptions.RemoveEmptyEntries)
                                      .Select(int.Parse).ToArray())
                        .ToArray();
            char[] operations = input
                            .Last()
                            .Split(new char[0], StringSplitOptions.RemoveEmptyEntries)
                            .Select(s => s[0])
                            .ToArray();
            return (values, operations);
        }

        public string PartOne()
        {
            var (values, operations) = LoadPartOneValues();

            long answer = 0;
            for (var i = 0; i < operations.Length; i++)
            {
                long value = operations[i] == '+' ? 0 : 1;
                for (var j = 0; j < values.Length; j++)
                {
                    if (operations[i] == '+')
                        value += values[j][i];
                    else
                        value *= values[j][i];
                }
                answer += value;          
            }
            return answer.ToString();
        }

        public string PartTwo()
        {
            long answer = 0;

            var operations = input.Last();
            var currOpIndex = 0;
            while (currOpIndex < operations.Length)
            {
                var currOp = operations[currOpIndex];
                if (currOp == ' ')
                    throw new InvalidProgramException();

                var nextOpIndex = operations
                            .TakeLast(operations.Length - currOpIndex - 1)
                            .TakeWhile(x => x == ' ')
                            .Count()
                            + currOpIndex + 1;
                var stCol = nextOpIndex - 1;
                if (nextOpIndex < operations.Length)
                    stCol -= 1; // because of blank columns

                long value = currOp == '+' ? 0 : 1;
                for (var col = stCol; col >= currOpIndex; col--)
                {
                    var number = "";
                    for (var row = 0; row < input.Length - 1; row++)
                    {
                        if (input[row][col] != ' ')
                            number += input[row][col];
                    }
                    if (currOp == '+')
                        value += long.Parse(number);
                    else
                        value *= long.Parse(number);
                }
                
                answer += value;
                currOpIndex = nextOpIndex;
            }

            return answer.ToString();
        }
    }
}