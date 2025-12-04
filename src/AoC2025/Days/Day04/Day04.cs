namespace AoC2025.Days
{
    public class Day04(string file) : IDay
    {
        private readonly char[][] grid = LoadInput(file);

        private static char[][] LoadInput(string file)
        {
            return File.ReadAllLines(file).Select(s => s.ToCharArray()).ToArray();
        }

        private bool Accessible(int x, int y)
        {
            var nAdjacent = 0;
            for (int i = -1; i <= 1; i++)
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue;
                    var xx = x + i;
                    var yy = y + j;
                    if (yy < 0 || yy >= grid.Length) continue;
                    if (xx < 0 || xx >= grid[yy].Length) continue;
                    if (grid[yy][xx] == '@')
                        nAdjacent += 1;
                    if (nAdjacent >= 4)
                        return false;
                }
            return true;
        }

        public string PartOne()
        {
            int answer = 0;
            for (var y = 0; y < grid.Length; y++)
                for (var x = 0; x < grid.Length; x++)
                {
                    if (grid[y][x] == '@' && Accessible(x,y))
                        answer += 1;
                }
            return answer.ToString();
        }

        public string PartTwo()
        {
            int answer = 0;
            var someAccessible = false;
            do
            {
                someAccessible = false;
                for (var y = 0; y < grid.Length; y++)
                    for (var x = 0; x < grid.Length; x++)
                    {
                        if (grid[y][x] == '@' && Accessible(x,y))
                        {
                            grid[y][x] = '.';
                            someAccessible = true;
                            answer += 1;
                        }
                    }
            }
            while (someAccessible);
            return answer.ToString();
        }
    }
}