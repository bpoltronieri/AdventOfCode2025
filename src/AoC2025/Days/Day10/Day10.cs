using System.Diagnostics.CodeAnalysis;

namespace AoC2025.Days
{
    public class Day10 : IDay
    {
        private Machine[] machines;

        public Day10(string file)
        {
            LoadInput(file);
        }

        [MemberNotNull(nameof(machines))]
        private void LoadInput(string file)
        {
            string[] input = File.ReadAllLines(file);
            machines = new Machine[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                var split = input[i].Split(' ');
                var newMachine = new Machine(split.First(), split.Last());
                for (int j = 1; j < split.Length-1; j++)
                    newMachine.AddButton(split[j]);
                machines[i] = newMachine;
            }
        }

        private static bool ButtonCombinationWorks(Machine machine, string combination)
        {
            for (int i = 0; i < combination.Length; i++)
                if (combination[i] == '1')
                    machine.PressButton(i);
            return machine.IsConfigured();
        }

        private static int GetMachineMinPressesLights(Machine machine)
        {
            if (machine.IsConfigured()) return 0; // already configured. 0 presses

            var sortedCombinations // combinations of button presses (0 or 1 press of each button) as strings of 0s and 1s, sorted by number of button presses
                = Enumerable.Range(0, (int)Math.Pow(2, machine.NButtons))
                .Select(i => Convert.ToString(i, 2).PadLeft(machine.NButtons, '0'))
                .OrderBy(s => s.Count(c => c == '1'));

            foreach (var combination in sortedCombinations)
            {
                if (ButtonCombinationWorks(machine, combination))
                    return machine.NButtonPresses;
                else
                    machine.Reset();
            }
            throw new InvalidDataException();
        }

        public string PartOne()
        {
            var answer = 0;
            foreach (var machine in machines)
                answer += GetMachineMinPressesLights(machine);
            return answer.ToString();
        }

        public string PartTwo()
        {
            throw new NotImplementedException();
        }
    }
}