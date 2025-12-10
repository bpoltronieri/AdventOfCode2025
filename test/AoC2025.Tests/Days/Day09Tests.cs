using System.Reflection;
using AoC2025.Days;

namespace AoC2025.Tests.Days
{
    public class Day09Tests
    {
        [Fact]
        public void Day09Test1()
        {
            // arrange
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? throw new InvalidDataException();
            path = Path.GetFullPath(Path.Combine(path, "..", "..", "..")) ?? throw new InvalidDataException();

            var inputFile = Directory.GetFiles(path + @"/TestInput", "Day09_1.txt")[0];
            var day = new Day09(inputFile);

            // act
            var result1 = day.PartOne();
            var result2 = day.PartTwo();

            // assert
            Assert.Equal("50", result1);
            Assert.Equal("24", result2);
        }

        [Fact]
        public void Day09Test2()
        {
            // arrange
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? throw new InvalidDataException();
            path = Path.GetFullPath(Path.Combine(path, "..", "..", "..")) ?? throw new InvalidDataException();

            var inputFile = Directory.GetFiles(path + @"/TestInput", "Day09_2.txt")[0];
            var day = new Day09(inputFile);

            // act
            var result1 = day.PartOne();
            var result2 = day.PartTwo();

            // assert
            Assert.Equal("35", result1);
            Assert.Equal("15", result2);
        }
    }
}