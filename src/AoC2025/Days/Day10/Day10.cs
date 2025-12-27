using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Google.OrTools.Init;
using Google.OrTools.LinearSolver;

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
            if (machine.IsConfigured()) return 0; // already configured. 0 presses. don't waste time generating combinations

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
            return machines.Sum(m => GetMachineMinPressesLights(m)).ToString();
        }

        private static int[,] GetCoefficientMatrix(Machine machine)
        {
            // build matrix for system of linear equations defined by machine's buttons:
            var coeffMatrix = new int[machine.JoltageRequirements.Length, machine.NButtons];
            for (int i = 0; i < machine.NButtons; i++)
            {                
                var button = machine.Buttons[i];
                foreach (var index in button)
                    coeffMatrix[index, i] = 1;
            }
            return coeffMatrix;
        }

        private static int GetMachineMinPressesJoltages(Machine machine)
        {
            var upperBound = machine.JoltageRequirements.Sum();
            Solver solver = Solver.CreateSolver("SCIP"); // Google Linear Optimization Package
            
            var variables = new Variable[machine.NButtons];
            for (int i = 0; i < machine.NButtons; i++)
                variables[i] = solver.MakeIntVar(0, upperBound, "b" + i.ToString());

            var coefficientMatrix = GetCoefficientMatrix(machine);
            for (int i = 0; i < machine.JoltageRequirements.Length; i++)
            {
                Constraint constraint = solver.MakeConstraint(machine.JoltageRequirements[i], machine.JoltageRequirements[i]);
                for (int j = 0; j < machine.NButtons; j++)
                    constraint.SetCoefficient(variables[j], coefficientMatrix[i,j]);
            }

            Objective objective = solver.Objective();
            for (int i = 0; i < machine.NButtons; i++)
                objective.SetCoefficient(variables[i], 1);
            objective.SetMinimization();

            Solver.ResultStatus resultStatus = solver.Solve();
            if (resultStatus != Solver.ResultStatus.OPTIMAL)
                throw new InvalidDataException();

            return (int)solver.Objective().Value();
        }

        public string PartTwo()
        {
            return machines.Sum(m => GetMachineMinPressesJoltages(m)).ToString();
        }

    }
}