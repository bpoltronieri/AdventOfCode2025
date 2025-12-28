using System.Reflection;
using AoC2025.Days;

namespace AoC2025.Tests.Days
{
    public class Day11Tests
    {
        [Fact]
        public void Day11Test1()
        {
            // arrange
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? throw new InvalidDataException();
            path = Path.GetFullPath(Path.Combine(path, "..", "..", "..")) ?? throw new InvalidDataException();

            var inputFile = Directory.GetFiles(path + @"/TestInput", "Day11_1.txt")[0];
            var day = new Day11(inputFile);

            // act
            var result1 = day.PartOne();

            // assert
            Assert.Equal("5", result1);
        }

        [Fact]
        public void Day11Test2()
        {
            // arrange
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? throw new InvalidDataException();
            path = Path.GetFullPath(Path.Combine(path, "..", "..", "..")) ?? throw new InvalidDataException();

            var inputFile = Directory.GetFiles(path + @"/TestInput", "Day11_2.txt")[0];
            var day = new Day11(inputFile);

            // act
            var result2 = day.PartTwo();

            // assert
            Assert.Equal("2", result2);
        }
    }
}