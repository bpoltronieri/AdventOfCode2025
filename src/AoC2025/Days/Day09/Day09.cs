namespace AoC2025.Days
{
    public class Day09(string file) : IDay
    {
        // next time, use or write a class for a 2D vector.
        // was lazy and kept using tuples, but would have been worth it today!
        private readonly (int,int)[] positions = LoadInput(file);

        private static (int,int)[] LoadInput(string file)
        {
            return File.ReadAllLines(file).Select(L => 
            {
                var x = L.Split(',').Select(int.Parse).ToArray();
                return (x[0], x[1]);
            }
            ).ToArray();
        }

        private static long RectangleArea((int, int) posn1, (int, int) posn2)
        {
            long dx = Math.Abs(posn2.Item1 - posn1.Item1) + 1;
            long dy = Math.Abs(posn2.Item2 - posn1.Item2) + 1;
            return dx * dy;
        }

        public string PartOne()
        {
            long maxArea = 0;
            for (int i = 0; i < positions.Length; i++)
                for (int j = i+1; j < positions.Length; j++)
                    maxArea = Math.Max(maxArea, RectangleArea(positions[i], positions[j]));
            return maxArea.ToString();
        }

        private List<(int,int,long)> GetSortedRectangles()
        {
            var distances = new List<(int,int,long)>(); // List of (corner1Index, corner2Index, distance)
            for (int i = 0; i < positions.Length; i++)
                for (int j = i+1; j < positions.Length; j++)
                    distances.Add((i, j, RectangleArea(positions[i], positions[j])));
            return distances.OrderByDescending(d=>d.Item3).ToList();
        }

        private static (int,int) LineDirection((int, int) posn1, (int, int) posn2)
        {
            if (posn1 == posn2)
                throw new InvalidDataException();

            var delta = (posn2.Item1 - posn1.Item1, posn2.Item2 - posn1.Item2);
            if (delta.Item1 == 0)
                return delta.Item2 > 0 ? (0,1) : (0,-1);
            else if (delta.Item2 == 0)
                return delta.Item1 > 0 ? (1,0) : (-1,0);
            else
                throw new InvalidDataException();
        }

        private (int, int) DirectionBefore(int posnIndex)
        {
            var prevPosn = posnIndex == 0 ? positions.Last() : positions[posnIndex-1];
            var posn = positions[posnIndex];
            return LineDirection(prevPosn, posn);
        }

        private (int, int) DirectionAfter(int posnIndex)
        {
            var posn = positions[posnIndex];
            var nextPosn = positions[(posnIndex+1)%positions.Length];
            return LineDirection(posn, nextPosn);
        }

        private static int CROSS((int,int) dirn1, (int,int) dirn2)
        {
            return (dirn1.Item1 * dirn2.Item2) - (dirn1.Item2 * dirn2.Item1);
        }

        private bool PolygonIsClockwise()
        {
            // returns whether vertices of polygon in input go clockwise around polygon
            var turnCount = 0;
            for (var i = 0; i < positions.Length; i++)
                turnCount += CROSS(DirectionBefore(i), DirectionAfter(i));

            if (turnCount == -4)
                return true;
            else if (turnCount == 4)
                return false;

            throw new InvalidDataException();
        }

        private static bool PointWithinRectangle((int, int) p, (int, int) bottomLeft, (int, int) topRight)
        {
            return bottomLeft.Item1 < p.Item1 && p.Item1 < topRight.Item1
                && bottomLeft.Item2 < p.Item2 && p.Item2 < topRight.Item2;
        }

        private bool IsInPolygon(int corner0Index, int corner1Index, Dictionary<(int, int), int> positionDict, HashSet<(int, int)> polygonEdgePts)
        {
            // tests whether rectangle with given opposite corners is fully within the polygon
            // method: walk anti-clockwise round the rectangle boundary, checking whether we ever leave the polygon
            var posn1 = positions[corner0Index];
            var posn2 = positions[corner1Index];
            var bottomLeft = (Math.Min(posn1.Item1, posn2.Item1), Math.Min(posn1.Item2, posn2.Item2));
            var topRight = (Math.Max(posn1.Item1, posn2.Item1), Math.Max(posn1.Item2, posn2.Item2));

            // quick test first: are there any polygon vertices within the rectangle?
            foreach (var p in positions)
                if (PointWithinRectangle(p, bottomLeft, topRight))
                    return false;

            var corners = new (int,int)[4]; // anti-clockwise round rectangle starting at bottom left
            corners[0] = bottomLeft;
            corners[2] = topRight;
            corners[1] = (corners[2].Item1, corners[0].Item2);
            corners[3] = (corners[0].Item1, corners[2].Item2);

            for (var side = 0; side < 4; side++)
            {
                var lineSt = corners[side];
                var lineNd = corners[(side+1)%4];
                var lineLength = RectangleArea(lineSt, lineNd);
                var lineDirn = LineDirection(lineSt, lineNd);

                var currPosn = lineSt;
                for (var j = 0; j < lineLength-1; j++)
                {
                    var nextPosn = (currPosn.Item1 + lineDirn.Item1, currPosn.Item2 + lineDirn.Item2);

                    if (positionDict.ContainsKey(currPosn) && !positionDict.ContainsKey(nextPosn))
                    {   // need to check if we left polygon through a vertex
                        var exited = false;

                        var currDirn = LineDirection(currPosn, nextPosn);
                        var vertexIndex = positionDict[currPosn];
                        var polygonDirnBefore = DirectionBefore(vertexIndex);
                        var polygonDirnAfter = DirectionAfter(vertexIndex);

                        if (currDirn == polygonDirnAfter)
                            exited = false; // this is always fine as both rectangle and polygon are anti-clockwise
                        else if (polygonDirnBefore == polygonDirnAfter) // don't think there are co-linear segments in the polygon but just in case
                            exited = CROSS(currDirn, polygonDirnAfter) != 1;
                        else if (CROSS(polygonDirnBefore, polygonDirnAfter) == 1) // convex corner
                            exited = true; // since already ruled out currDirn == polygonDirnAfter
                        else // concave corner
                            exited = currDirn == (-polygonDirnBefore.Item1, -polygonDirnBefore.Item2);

                        if (exited)
                            return false;
                    }
                    else if (polygonEdgePts.Contains(currPosn) && !polygonEdgePts.Contains(nextPosn)
                            && !positionDict.ContainsKey(nextPosn))
                        return false; // cross polygon boundary not at vertex
                    currPosn = nextPosn;
                }
            }

            return true;
        }

        private HashSet<(int, int)> GetPolygonEdgePts()
        {
            // returns set of points that are on polyline edges but not on vertices
            var set = new HashSet<(int,int)>();
            for (var i = 0; i < positions.Length; i++)
            {
                var lineSt = positions[i];
                var lineNd = positions[(i+1)%positions.Length];
                var lineLength = RectangleArea(lineNd, lineSt);
                var lineDirn = LineDirection(lineSt, lineNd);
                for (var j = 1; j < lineLength-1; j++)
                {
                    var posn = (lineSt.Item1 + lineDirn.Item1 * j, lineSt.Item2 + lineDirn.Item2 * j);
                    set.Add(posn);
                }
            }
            return set;
        }

        public string PartTwo()
        {          
            var positionDict = new Dictionary<(int,int), int>(); // maps from position to index into positions array
            for (var i = 0; i < positions.Length; i++)
                positionDict.Add(positions[i], i);  // will also flag if there are any repeat positions in input

            if (PolygonIsClockwise())
                throw new NotImplementedException(); // checked my input is anti-clockwise

            var polygonEdgePts = GetPolygonEdgePts();
            var rectangles = GetSortedRectangles();
            var maxRectangle = rectangles.First(R => IsInPolygon(R.Item1, R.Item2, positionDict, polygonEdgePts));

            return maxRectangle.Item3.ToString();
        }
    }
}