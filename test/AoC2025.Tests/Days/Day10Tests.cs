using System.Reflection;
using AoC2025.Days;

namespace AoC2025.Tests.Days
{
    public class Day10Tests
    {
        [Fact]
        public void Day10Test1()
        {
            // arrange
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? throw new InvalidDataException();
            path = Path.GetFullPath(Path.Combine(path, "..", "..", "..")) ?? throw new InvalidDataException();

            var inputFile = Directory.GetFiles(path + @"/TestInput", "Day10_1.txt")[0];
            var day = new Day10(inputFile);

            // act
            var result1 = day.PartOne();
            // var result2 = day.PartTwo();

            // assert
            Assert.Equal("7", result1);
            // Assert.Equal("33", result2);
        }
    }
}