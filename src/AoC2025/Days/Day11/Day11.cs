using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace AoC2025.Days
{
    public class Day11 : IDay
    {
        private Dictionary<string, int> nameToIndex;
        private List<string>[] parents; // parents of each node

        public Day11(string file)
        {
            LoadInput(file);
        }

        [MemberNotNull(nameof(nameToIndex))]
        [MemberNotNull(nameof(parents))]
        private void LoadInput(string file)
        {
            var input = File.ReadAllLines(file);
            var deviceNames = input.Select(s => s.Split(':')[0]).ToArray();
            nameToIndex = new Dictionary<string, int>(deviceNames.Length + 1); // +1 for "out" which has parents but no children
            parents = new List<string>[deviceNames.Length + 1];

            for (int i = 0; i < deviceNames.Length; i++)
            {
                nameToIndex.Add(deviceNames[i], i);
                parents[i] = new List<string>();
            }
            nameToIndex.Add("out", deviceNames.Length);
            parents[deviceNames.Length] = new List<string>();
 
            for (int i = 0; i < deviceNames.Length; i++)
            {
                foreach (var output in input[i].Split(": ")[1].Split(' '))
                    parents[nameToIndex[output]].Add(deviceNames[i]);
            }
        }

        private long CountPathsUp(string start, string end, Dictionary<string, long>? cache = null) 
        {
            // counting paths from start to end moving up via parents
            if (start == end)
                return 1;

            if (cache == null) 
                cache = new Dictionary<string, long>();

            if (cache.ContainsKey(start))
                return cache[start];

            long nPaths = 0;
            foreach (var parent in parents[nameToIndex[start]]) 
                nPaths += CountPathsUp(parent, end, cache);
            cache[start] = nPaths;
            return nPaths;
        }

        public string PartOne()
        {
            return CountPathsUp("out", "you").ToString();
        }

        public string PartTwo()
        {
            var nSvrToDac = CountPathsUp("dac", "svr");
            var nSvrToFft = CountPathsUp("fft", "svr");
            var nDacToFft = CountPathsUp("fft", "dac");
            var nFftToDac = CountPathsUp("dac", "fft");
            var nDacToOut = CountPathsUp("out", "dac");
            var nFftToOut = CountPathsUp("out", "fft");

            var answer = (nSvrToDac * nDacToFft * nFftToOut) + (nSvrToFft * nFftToDac * nDacToOut);
            return answer.ToString();
        }
    }
}