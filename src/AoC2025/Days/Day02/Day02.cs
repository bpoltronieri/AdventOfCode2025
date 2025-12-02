namespace AoC2025.Days
{
    public class Day02(string file) : IDay
    {
        private readonly string[] ranges = LoadInput(file);

        private static string[] LoadInput(string file)
        {
            return File.ReadAllLines(file)[0].Split(',');
        }

        public string PartOne()
        {
            long answer = 0;
            for (var i = 0; i < ranges.Length; i++)
            {
                var range = ranges[i].Split('-').Select(s =>  Int64.Parse(s));
                for (var ID = range.First(); ID <= range.Last(); ID++)
                {
                    var IdStr = ID.ToString();
                    var nDigits = IdStr.Length;
                    if (nDigits % 2 == 0 && IdStr[..(nDigits / 2)] == IdStr[(nDigits/2)..])
                        answer += ID;                    
                }
            }
            return answer.ToString();
        }

        public string PartTwo()
        {
            long answer = 0;
            for (var i = 0; i < ranges.Length; i++)
            {
                var range = ranges[i].Split('-').Select(s =>  Int64.Parse(s));
                for (var ID = range.First(); ID <= range.Last(); ID++)
                {
                    var IdStr = ID.ToString();
                    var nDigits = IdStr.Length;
                    var invalid = false;

                    for (var subLen = 1; !invalid && subLen <= nDigits/2; subLen++)
                    {
                        if (nDigits % subLen != 0) continue;
                        var allEqual = true;
                        var subStr = IdStr[..subLen];
                        for (var k = 1; allEqual && k < nDigits / subLen; k++)
                        {
                            var currSubStr = IdStr.Substring(k * subLen, subLen);
                            if (subStr != currSubStr)
                                allEqual = false;
                        }
                        if (allEqual)
                            invalid = true;
                    }

                    if (invalid)
                        answer += ID;                    
                }
            }
            return answer.ToString();
        }
    }
}