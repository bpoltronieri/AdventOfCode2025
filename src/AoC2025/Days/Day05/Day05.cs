using System.Diagnostics.CodeAnalysis;

namespace AoC2025.Days
{
    public struct Interval(long start, long end)
    {
        public long Start { get; set; } = start;
        public long End { get; set; } = end;
    }

    public class Day05 : IDay
    {
        private Interval[] freshRanges;
        private long[] ingredients;

        public Day05(string file)
        {
            LoadInput(file);
        }

        [MemberNotNull(nameof(freshRanges))]
        [MemberNotNull(nameof(ingredients))]
        private void LoadInput(string file)
        {
            string[] input = File.ReadAllLines(file);
            var blankLineIndex = input.IndexOf("");

            freshRanges = input.Take(blankLineIndex).Select(s => 
                    {
                        var split = s.Split('-').Select(long.Parse);
                        return new Interval(split.First(), split.Last());
                    }
                ).OrderBy(interval => interval.Start).ToArray();
            ingredients = input.TakeLast(input.Length - blankLineIndex - 1).Select(long.Parse).ToArray();
        }

        public string PartOne()
        {
            long answer = 0;
            for (var i = 0; i < ingredients.Length; i++)
                for (var j = 0; j < freshRanges.Length; j++)
                {
                    if (freshRanges[j].Start > ingredients[i]) 
                        break;   // freshRanges are ordered by start
                    else if (ingredients[i] >= freshRanges[j].Start && ingredients[i] <= freshRanges[j].End)
                    {
                        answer += 1;
                        break;
                    }
                }
            return answer.ToString();
        }

        private void CleanUpOverlaps()
        {
            // assumes freshRanges already sorted by Start
            var prevRange = freshRanges[0];
            for (var i = 1; i < freshRanges.Length; i++)
            {
                if (freshRanges[i].End <= prevRange.End)
                    freshRanges[i] = new Interval(0, -1); // nullify this range
                else
                {
                    freshRanges[i].Start = Math.Max(freshRanges[i].Start, prevRange.End + 1);
                    prevRange = freshRanges[i];
                }
            }
        }
        public string PartTwo()
        {
            CleanUpOverlaps();
            long answer = freshRanges.Sum(range => range.End - range.Start + 1);
            return answer.ToString();
        }
    }
}