using System.Reflection.PortableExecutable;

namespace AoC2025.Days
{
    public class Machine
    {
        private readonly string lightDiagram; // without square brackets
        private readonly char[] currentState;
        private readonly int[] joltageRequirements;
        private readonly int[] currentJoltages;
        private readonly List<List<int>> buttons = [];
        public int NButtons {get; private set;} = 0;
        public int NButtonPresses {get; private set;} = 0;
        public bool JoltageMode {get; set;} = false;

        public Machine(string inputLightDiagram, string joltages)
        {
            lightDiagram = inputLightDiagram.TrimStart('[').TrimEnd(']');
            currentState = new char[lightDiagram.Length];
            currentJoltages = new int[lightDiagram.Length];
            Reset();
            joltageRequirements = joltages.TrimStart('{').TrimEnd('}').Split(',').Select(int.Parse).ToArray();
        }

        public void AddButton(string button)
        {
            var newButton = button.TrimStart('(').TrimEnd(')').Split(',').Select(int.Parse).ToList();
            if (newButton.Any(i => i < 0 || i >= lightDiagram.Length))
                throw new InvalidDataException();
            buttons.Add(newButton);
            NButtons += 1;
        }

        public void Reset()
        {
            NButtonPresses = 0;
            for (int i = 0; i < currentState.Length; i++)
                {
                currentState[i] = '.';
                currentJoltages[i] = 0;
                }
        }

        public bool IsConfigured()
        {
            if (JoltageMode)
                return joltageRequirements.SequenceEqual(currentJoltages);
            else
                return lightDiagram.SequenceEqual(currentState);
        }

        public void PressButton(int buttonId)
        {
            if (buttonId < 0 || buttonId >= buttons.Count)
                throw new InvalidOperationException();
            foreach (var i in buttons[buttonId])
            {  
                if (JoltageMode)
                    currentJoltages[i] += 1;
                else
                {
                    if (currentState[i] == '.')
                        currentState[i] = '#';
                    else if (currentState[i] == '#')
                        currentState[i] = '.';
                    else
                        throw new InvalidProgramException();
                }
            }
            NButtonPresses += 1;
        }

    }
}