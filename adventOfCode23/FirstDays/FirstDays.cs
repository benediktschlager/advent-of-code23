using System.Text.RegularExpressions;

namespace adventOfCode23.FirstDays;

public class FirstDays
{
    private string[] CleanInput(string s)
    {
        var lines = s.Split(Environment.NewLine);

        return lines.Select(l => l.Trim()).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();
    }

    private string[] CleanInputFile(string file)
    {
        return CleanInput(File.ReadAllText(Path.Join("FirstDays", file)));
    }

    private void Day1()
    {
        var input = CleanInputFile("day1.txt");
        var result = new List<int>();
        foreach (var line in input)
        {
            int? left = null;
            int? right = null;
            var x = new[] { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

            for (var i = 0; i < line.Length; i++)
            {
                if (char.IsDigit(line[i]))
                {
                    left = int.Parse(line[i].ToString());
                    break;
                }

                var match = x
                    .Select((num, xi) => num == line.Substring(i, Math.Min(num.Length, line.Length - i)) ? xi : -1)
                    .Where(ix => ix != -1).ToArray();
                if (match.Length > 0)
                {
                    left = match[0] + 1;
                    break;
                }
            }

            for (var i = line.Length - 1; i >= 0; i--)
            {
                if (char.IsDigit(line[i]))
                {
                    right = int.Parse(line[i].ToString());
                    break;
                }

                var match = x
                    .Select((num, xi) => num == line.Substring(i, Math.Min(num.Length, line.Length - i)) ? xi : -1)
                    .Where(ix => ix != -1).ToArray();
                if (match.Length > 0)
                {
                    right = match[0] + 1;
                    break;
                }
            }

            result.Add(int.Parse(left + right.ToString()));
        }

        Console.WriteLine(result.Sum());
    }

    private void Day2()
    {
        Console.BackgroundColor = ConsoleColor.Blue;
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("Day 2");
        Console.ResetColor();
        var regexGameId = new Regex(@"Game (\d*)");
        var input = CleanInputFile("day2.txt");
        var limits = new Dictionary<string, int>
        {
            { "red", 12 },
            { "green", 13 },
            { "blue", 14 }
        };
        var result = new Dictionary<string, int>();
        var resultSum = 0;
        var resultMulti = 1;
//Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green
        foreach (var line in input)
        {
            var parts = line.Split(": ");
            var gameId = int.Parse(regexGameId.Match(parts[0]).Groups[1].Value);
            foreach (var item in parts[1].Split(", ").SelectMany(s => s.Split("; "))
                         .Where(s => !string.IsNullOrWhiteSpace(s)))
            {
                var s = item.Split(" ");
                var (count, color) = (int.Parse(s[0]), s[1]);
                result[color] = Math.Max(count, result.GetValueOrDefault(color));
            }

            foreach (var x in result.Values) resultMulti *= x;

            Console.WriteLine($"Game {gameId} sum is {resultMulti}");

            var valid = true;
            foreach (var (color, limit) in limits)
                if (result.GetValueOrDefault(color) > limit)
                {
                    valid = false;
                    break;
                }

            resultSum += resultMulti;
            if (valid)
            {
                Console.WriteLine($"Game {gameId} is valid");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"Game {gameId} is invalid");
                Console.ResetColor();
            }

            result.Clear();
            resultMulti = 1;
        }

        Console.WriteLine($"total sum: {resultSum}");
    }

    private void Day3()
    {
        Console.BackgroundColor = ConsoleColor.DarkGray;
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("Day 3");
        Console.ResetColor();
        var input = CleanInputFile("day3.txt");
        // TODO: Parse all number from input (sep !char.IsDigit)
        // TODO: iterate all numbers and check which are part of a real part
        var syms = new List<Part>();
        foreach (var (line, i) in input.Select((l, i) => (l, i)))
            for (var u = 0; u < line.Length; u++)
                if (char.IsDigit(line[u]))
                {
                    var startOfDigits = u;
                    var endOfDigits = u + 1;
                    while (u < line.Length)
                    {
                        if (!char.IsDigit(line[u]))
                        {
                            endOfDigits = u;
                            break;
                        }

                        u += 1;
                        endOfDigits = u;
                    }

                    var partNr = int.Parse(input[i][startOfDigits..endOfDigits]);
                    syms.Add(new Part(i, startOfDigits, endOfDigits - startOfDigits, partNr));
                }

        var inputc = input.Select(l => l.ToCharArray().Select(c => c as char?).ToArray()).ToArray();
        var sum = 0UL;
        var gears = new Dictionary<(int, int), List<Part>>();
        foreach (var nr in syms)
        {
            // Number is too low
            // 413438 is still too low for day 3!
            // => Some parts are not detected..
            // 417964 is still too low for day 3!
            // Wrong too: 518260

            var partNr = int.Parse(input[nr.Line][nr.X..(nr.X + nr.Length)]);
            if (nr.HasSymbol(inputc, gears))
            {
                Console.WriteLine($"{nr.Line}, {nr.X} seems like a part number: {partNr}");
            }
            else
            {
                Console.ForegroundColor = partNr == 332 ? ConsoleColor.Blue : ConsoleColor.Red;
                Console.WriteLine($"{nr.Line}, {nr.X} is not a part number: {partNr}");
                Console.ResetColor();
            }
        }

        foreach (var (_, gearPart) in gears)
        {
            if (gearPart.Count != 2) continue;

            sum += (ulong)gearPart[0].PartNr * (ulong)gearPart[1].PartNr;
        }


        Console.WriteLine($"Sum: {sum}");
    }

    public void Run(int? day = null)
    {
        var today = DateTime.Now.Day;
        switch (day ?? today)
        {
            case 1:
                Day1();
                break;
            case 2:
                Day2();
                break;
            case 3:
                Day3();
                break;
            default:
                Console.WriteLine("day is missing!");
                break;
        }
    }

    private record Part(int Line, int X, int Length, int PartNr)
    {
        private static bool IsSym(char? c)
        {
            return c is not null && c != '.' && !char.IsDigit(c.Value);
        }

        public bool HasSymbol(char?[][] grid, Dictionary<(int, int), List<Part>> gears)
        {
            for (var y = Math.Max(Line - 1, 0); y < Math.Min(grid.Length, Line + 2); y++)
            for (var x = Math.Max(X - 1, 0); x < Math.Min(grid[y].Length, X + Length + 1); x++)
                if (IsSym(grid[y][x]))
                {
                    if (grid[y][x] == '*')
                    {
                        var value = gears.GetValueOrDefault((y, x)) ?? new List<Part>();
                        value.Add(this);
                        gears[(y, x)] = value;
                    }

                    return true;
                }

            return false;
        }
    }
}