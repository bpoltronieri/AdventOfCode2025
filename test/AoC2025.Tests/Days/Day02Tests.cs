using System.Reflection;
using AoC2025.Days;

namespace AoC2025.Tests.Days
{
    public class Day02Tests
    {
        [Fact]
        public void Day02Test2()
        {
            // arrange
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? throw new InvalidDataException();
            path = Path.GetFullPath(Path.Combine(path, "..", "..", "..")) ?? throw new InvalidDataException();

            var inputFile = Directory.GetFiles(path + @"/TestInput", "Day02_1.txt")[0];
            var day = new Day02(inputFile);

            // act
            var result1 = day.PartOne();
            var result2 = day.PartTwo();

            // assert
            Assert.Equal("1227775554", result1);
            Assert.Equal("4174379265", result2);
        }
    }
}