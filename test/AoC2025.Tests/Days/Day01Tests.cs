using System.Reflection;
using AoC2025.Days;

namespace AoC2025.Tests.Days
{
    public class Day01Tests
    {
        [Fact]
        public void Day01Test1()
        {
            // arrange
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? throw new InvalidDataException();
            path = Path.GetFullPath(Path.Combine(path, "..", "..", "..")) ?? throw new InvalidDataException();

            var inputFile = Directory.GetFiles(path + @"/TestInput", "Day01_1.txt")[0];
            var day01 = new Day01(inputFile);

            // act
            var result1 = day01.PartOne();
            var result2 = day01.PartTwo();

            // assert
            Assert.Equal("3", result1);
            Assert.Equal("6", result2);
        }
    }
}