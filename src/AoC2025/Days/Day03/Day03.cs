namespace AoC2025.Days
{
    public class Day03(string file) : IDay
    {
        private readonly string[] banks = LoadInput(file);

        private static string[] LoadInput(string file)
        {
            return File.ReadAllLines(file);
        }

        public string PartOne()
        {
            int answer = 0;
            for (var i = 0; i < banks.Length; i++)
            {
                var nBatteries = banks[i].Length;
                char digit1 = banks[i].Take(nBatteries-1).MaxBy(b => Int16.Parse(b.ToString()));
                char digit2 = banks[i].TakeLast(nBatteries - banks[i].IndexOf(digit1) - 1).MaxBy(b => Int16.Parse(b.ToString()));
                var value = Int16.Parse(digit1.ToString() + digit2.ToString());
                answer += value;
            }
            return answer.ToString();
        }

        public string PartTwo()
        {
            Int64 answer = 0;
            for (var i = 0; i < banks.Length; i++)
            {
                var digits = new char[12];
                var nBatteries = banks[i].Length;

                var prevIndex = -1;
                digits[0] = banks[i].Take(nBatteries-11).MaxBy(b => Int16.Parse(b.ToString()));
                for (var j = 1; j < 12; j++)
                {
                    prevIndex += banks[i][(prevIndex+1)..].IndexOf(digits[j-1]) + 1;
                    digits[j] = banks[i][(prevIndex+1)..(nBatteries-11+j)].MaxBy(b => Int16.Parse(b.ToString()));
                }

                var value = Int64.Parse(String.Concat(digits));
                answer += value;
            }
            return answer.ToString();
        }
    }
}