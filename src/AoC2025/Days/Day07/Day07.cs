namespace AoC2025.Days
{
    public class Day07(string file) : IDay
    {
        private readonly char[][] grid = LoadInput(file);

        private static char[][] LoadInput(string file)
        {
            return File.ReadAllLines(file).Select(s => s.ToCharArray()).ToArray();
        }

        public string PartOne()
        {
            int nSplits = 0;
            var beams = new HashSet<int>(); // x positions of beams in current row
            // find initial beam
            var x = grid[0].TakeWhile(x => x != 'S').Count();
            beams.Add(x);
            
            for (int row = 2; row < grid.Length; row++)
            {
                var newBeams = new HashSet<int>();
                foreach (var beamX in beams)
                {
                    if (grid[row][beamX] == '^')
                    {
                        nSplits += 1;
                        newBeams.Add(beamX-1);
                        newBeams.Add(beamX+1);
                    }
                    else
                        newBeams.Add(beamX);
                }
                beams = newBeams;
            }

            return nSplits.ToString();
        }

        private static void AddOrIncrement(Dictionary<int, long> dict, int key, long prevValue)
        {
            if (!dict.TryAdd(key, prevValue))
                dict[key] += prevValue;
        }

        public string PartTwo()
        {
            var beams = new Dictionary<int,long>(); // Key = x positions of beams in current row, 
                                                   // Value = n timelines at this position
            // find initial beam
            var x = grid[0].TakeWhile(x => x != 'S').Count();
            beams.Add(x, 1);
            
            for (int row = 2; row < grid.Length; row++)
            {
                var newBeams = new Dictionary<int,long>();
                foreach (var beamX in beams.Keys)
                {
                    if (grid[row][beamX] == '^')
                    {
                        AddOrIncrement(newBeams, beamX-1, beams[beamX]);
                        AddOrIncrement(newBeams, beamX+1, beams[beamX]);
                    }
                    else
                        AddOrIncrement(newBeams, beamX, beams[beamX]);
                }
                beams = newBeams;
            }

            return beams.Values.Sum().ToString();
        }
    }
}