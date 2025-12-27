namespace AoC2025.Days
{
    public class Machine
    {
        private readonly string lightDiagram; // without square brackets
        private readonly char[] currentState;
        public List<List<int>> Buttons {get; private set;} = [];
        public int[] JoltageRequirements {get; private set;}
        public int NButtons {get; private set;} = 0;
        public int NButtonPresses {get; private set;} = 0;

        public Machine(string inputLightDiagram, string joltages)
        {
            lightDiagram = inputLightDiagram.TrimStart('[').TrimEnd(']');
            currentState = new char[lightDiagram.Length];
            Reset();
            JoltageRequirements = joltages.TrimStart('{').TrimEnd('}').Split(',').Select(int.Parse).ToArray();
        }

        public void AddButton(string button)
        {
            var newButton = button.TrimStart('(').TrimEnd(')').Split(',').Select(int.Parse).ToList();
            if (newButton.Any(i => i < 0 || i >= lightDiagram.Length))
                throw new InvalidDataException();
            Buttons.Add(newButton);
            NButtons += 1;
        }

        public void Reset()
        {
            NButtonPresses = 0;
            for (int i = 0; i < currentState.Length; i++)
                currentState[i] = '.';
        }

        public bool IsConfigured()
        {
            return lightDiagram.SequenceEqual(currentState);
        }

        public void PressButton(int buttonId)
        {
            if (buttonId < 0 || buttonId >= Buttons.Count)
                throw new InvalidOperationException();
            foreach (var i in Buttons[buttonId])
            {  
                if (currentState[i] == '.')
                    currentState[i] = '#';
                else if (currentState[i] == '#')
                    currentState[i] = '.';
                else
                    throw new InvalidProgramException();
            }
            NButtonPresses += 1;
        }
    }
}