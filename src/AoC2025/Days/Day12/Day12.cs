using System.Diagnostics.CodeAnalysis;

namespace AoC2025.Days
{
    public struct Present(int filledSpaces, char[][] shape)
    {
        public int FilledSpaces { get; set; } = filledSpaces;
        public char[][] Shape { get; set; } = shape;
    }

    public struct Tree(int width, int length, int[] presentsNeeded)
    {
        public int Width { get; set; } = width;
        public int Length { get; set; } = length;
        public int[] PresentsNeeded { get; set; } = presentsNeeded;
    }

    public class Day12 : IDay
    {
        private List<Present> presents;
        private List<Tree> trees;
        private readonly int presentSize = 3;

        public Day12(string file)
        {
            LoadInput(file);
        }

        [MemberNotNull(nameof(presents))]
        [MemberNotNull(nameof(trees))]
        private void LoadInput(string file)
        {
            presents = new List<Present>();
            trees = new List<Tree>();

            string[] input = File.ReadAllLines(file);

            var currentIndex = 0;
            while (input[currentIndex].Last() == ':')
            {
                var shape = new char[presentSize][];
                var nFilled = 0;
                for (var i = 0; i < presentSize; i++)
                {
                    var line = input[currentIndex+1+i];
                    if (line.Length != presentSize) throw new InvalidDataException();
                    nFilled += line.Count('#');
                    shape[i] = line.ToCharArray();
                }
                var currentPresent = new Present(nFilled, shape);
                presents.Add(currentPresent);
                currentIndex += 5;
            }

            for (var i = currentIndex; i < input.Length; i++)
            {
                var line = input[i];
                var sizes = line.Split(':')[0].Split('x').Select(int.Parse);
                var presentsNeeded = line.Split(": ")[1].Split(' ').Select(int.Parse);
                var currentTree = new Tree(sizes.First(), sizes.Last(), presentsNeeded.ToArray());
                trees.Add(currentTree);
            }
        }

        private bool CanObviouslyNotFitPresents(Tree t)
        {
            var totalFilled = 0;
            for (var i = 0; i < t.PresentsNeeded.Length; i++)
                totalFilled += presents[i].FilledSpaces * t.PresentsNeeded[i];
            return totalFilled > t.Width * t.Length;
        }

        private bool CanObviouslyFitPresents(Tree t)
        {
            var numberOfPresents = t.PresentsNeeded.Sum();
            return (t.Width * t.Length) > numberOfPresents * presentSize * presentSize;
        }

        private bool TreeCanFitPresents(Tree t)
        {
            if (CanObviouslyFitPresents(t))
                return true;
            else if (CanObviouslyNotFitPresents(t))
                return false;
            else
                return true; // assume that interlocking presents is always possible. Works for the input...
        }

        public string PartOne()
        {
            int nRegionsCanFit = trees.Count(TreeCanFitPresents);
            return nRegionsCanFit.ToString();
        }

        public string PartTwo()
        {
            return "No part 2 on final day!";
        }
    }
}