namespace AoC2025.Days
{
    public class Day08(string file) : IDay
    {
        private readonly (int,int,int)[] positions = LoadInput(file);

        private static (int,int,int)[] LoadInput(string file)
        {
            return File.ReadAllLines(file).Select(L => 
            {
                var x = L.Split(',').Select(int.Parse).ToArray();
                return (x[0], x[1], x[2]);
            }
            ).ToArray();
        }

        private static double EuclideanDistance((int, int, int) posn1, (int, int, int) posn2)
        {
            var dx = posn2.Item1 - posn1.Item1;
            var dy = posn2.Item2 - posn1.Item2;
            var dz = posn2.Item3 - posn1.Item3;
            return Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2) + Math.Pow(dz, 2));
        }

        private List<(int,int,double)> GetSortedDistances()
        {
            var distances = new List<(int,int,double)>(); // List of (posn1Index, posn2Index, distance)
            for (int i = 0; i < positions.Length; i++)
                for (int j = i+1; j < positions.Length; j++)
                    distances.Add((i, j, EuclideanDistance(positions[i], positions[j])));
            return distances.OrderBy(d=>d.Item3).ToList();
        }

        public List<int>?[] InitPosnCircuits()
        {
            var posnCircuits = new List<int>?[positions.Length];
            for (int i = 0; i < positions.Length; i++)
                posnCircuits[i] = null; // null means position is in circuit of 1
            return posnCircuits; 
        }

        private static void MergeCircuits(int posn1Index, int posn2Index, List<List<int>> circuits, List<int>?[] posnCircuits)
        {
            if (posnCircuits[posn1Index] == null && posnCircuits[posn2Index] == null) // new circuit
            {
                var newCircuit = new List<int> {posn1Index, posn2Index};
                circuits.Add(newCircuit);
                posnCircuits[posn1Index] = posnCircuits[posn2Index] = newCircuit;
            }
            else if (posnCircuits[posn1Index] == null)
            {
                var circuit = posnCircuits[posn2Index];
                circuit!.Add(posn1Index);
                posnCircuits[posn1Index] = circuit;
            }
            else if (posnCircuits[posn2Index] == null)
            {
                var circuit = posnCircuits[posn1Index];
                circuit!.Add(posn2Index);
                posnCircuits[posn2Index] = circuit;
            }
            else if (posnCircuits[posn1Index] != posnCircuits[posn2Index]) // both in circuits but not same one
            {
                var circuit1 = posnCircuits[posn1Index]!;
                var circuit2 = posnCircuits[posn2Index]!;
                var smallerCircuitId = circuit1.Count < circuit2.Count ? posn1Index : posn2Index;
                var biggerCircuitId = smallerCircuitId == posn1Index ? posn2Index : posn1Index;
                var smallerCircuit = posnCircuits[smallerCircuitId]!;
                var biggerCircuit = posnCircuits[biggerCircuitId]!;

                foreach (var posnId in smallerCircuit)
                {
                    biggerCircuit.Add(posnId);
                    posnCircuits[posnId] = biggerCircuit;
                }
                circuits.Remove(smallerCircuit);
            }
        }

        public string PartOne()
        {
            var distances = GetSortedDistances();
            var posnCircuits = InitPosnCircuits();
            var circuits = new List<List<int>>();
            
            var nConnections = positions.Length == 20 ? 10 : 1000; // test input tests fewer connections...
            for (int i = 0; i < nConnections; i++)
            {
                var posn1Index = distances[i].Item1;
                var posn2Index = distances[i].Item2;
                MergeCircuits(posn1Index, posn2Index, circuits, posnCircuits);   
            }

            var top3 = circuits.Select(c => c.Count)
                               .OrderByDescending(c=>c)
                               .Take(3).ToArray();        
            var answer = top3[0] * top3[1] * top3[2];
            return answer.ToString();
        }

        public string PartTwo()
        {
            var distances = GetSortedDistances();
            var posnCircuits = InitPosnCircuits();
            var circuits = new List<List<int>>();
            
            var connectionId = -1;
            var containsNull = true; // i.e. whether any boxes still in individual circuit
            while (containsNull || circuits.Count > 1)
                {
                connectionId += 1;
                var posn1Index = distances[connectionId].Item1;
                var posn2Index = distances[connectionId].Item2;
                MergeCircuits(posn1Index, posn2Index, circuits, posnCircuits);
                containsNull = containsNull && posnCircuits.Any(x => x == null);
                }
            
            var lastX1 = positions[distances[connectionId].Item1].Item1;
            var lastX2 = positions[distances[connectionId].Item2].Item1;
            var answer = lastX1 * lastX2;
            return answer.ToString();
        }
    }
}