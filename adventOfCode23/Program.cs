using System.Reflection;
using System.Text.RegularExpressions;
using adventOfCode23.FirstDays;

var isRunningInCi = args.Length > 1 && args[1..].Contains("--ci");

async Task UpdateReadme()
{
    // This method only updates README file so skip it when running in CI
    if (isRunningInCi) return;

    if (Environment.CurrentDirectory.Split("/")[^4] != "adventOfCode23")
    {
        Console.WriteLine("SKIP");
        return;
    }

    await using var stream = File.Open("../../../../README.md", FileMode.Open);
    var reader = new StreamReader(stream);
    var writer = new StreamWriter(stream);

    while (!reader.EndOfStream)
    {
        var line = await reader.ReadLineAsync();
        if (line is null) continue;
        var isComment = line.Contains("<!--") &&
                        line.Split("<!--")[1].Split("-->")[0].Trim() == "solution days";
        if (!isComment) continue;

        stream.Position -= line.Length + 1;
        var day = typeof(FirstDays).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
            .Select(m => new Regex(@"Day(\d?\d$)").Match(m.Name).Groups[1].Value)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(int.Parse)
            .Max();
        await writer.WriteLineAsync(
            $"The solutions of day 1 to {day} can be found in [FirstDays/FirstDays.cs](adventOfCode23/FirstDays/FirstDays.cs) <!-- solution days -->");
        break;
    }

    await writer.FlushAsync();
}

var updateReadmeTask = UpdateReadme();

new FirstDays().Run();

await updateReadmeTask;