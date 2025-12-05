using System.Reflection;
using AoC2025.Days;

namespace AoC2025.Tests.Days
{
    public class Day05Tests
    {
        [Fact]
        public void Day05Test1()
        {
            // arrange
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? throw new InvalidDataException();
            path = Path.GetFullPath(Path.Combine(path, "..", "..", "..")) ?? throw new InvalidDataException();

            var inputFile = Directory.GetFiles(path + @"/TestInput", "Day05_1.txt")[0];
            var day = new Day05(inputFile);

            // act
            var result1 = day.PartOne();
            var result2 = day.PartTwo();

            // assert
            Assert.Equal("3", result1);
            Assert.Equal("14", result2);
        }
    }
}