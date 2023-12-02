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
            default:
                Console.WriteLine("day is missing!");
                break;
        }
    }
}