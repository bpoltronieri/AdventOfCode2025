namespace AoC2025.Days
{
    public class Day01(string file) : IDay
    {
        private readonly string[] input = LoadInput(file);

        private static string[] LoadInput(string file)
        {
            return File.ReadAllLines(file);
        }

        private static (int, int) ApplyInstruction(int dial, string instruction)
        {
            // returns (newDial, nZeros), where nZeros is number of times a click causes the dial to point at 0 during the rotation
            int newDial = dial;
            var dist = Int32.Parse(instruction[1..]);
            if (instruction[0] == 'L') dist *= -1;
            newDial += dist;

            int nZeros = Math.Abs(newDial) / 100;
            if (newDial <= 0 && dial > 0) nZeros += 1;

            newDial %= 100;
            if (newDial < 0) newDial += 100;

            return (newDial, nZeros);
        }

        public string PartOne()
        {
            int answer = 0;
            int dial = 50;
            for (var i = 0; i < input.Length; i++)
            {
                dial = ApplyInstruction(dial, input[i]).Item1;
                if (dial == 0) answer += 1;
            }
            return answer.ToString();
        }

        public string PartTwo()
        {
            int answer = 0;
            int dial = 50;
            for (var i = 0; i < input.Length; i++)
            {
                var (newDial, nZeros) = ApplyInstruction(dial, input[i]);
                dial = newDial;
                answer += nZeros;
            }
            return answer.ToString();
        }
    }
}