new DayInfo().Day1();

class DayInfo
{
    string[] CleanInput(string s)
    {
        var lines = s.Split(Environment.NewLine);

        return lines.Select(l => l.Trim()).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();
    }

    string[] CleanInputFile(string file)
    
    {
        return CleanInput(File.ReadAllText(file));
    }
    
public void Day1()
{
     var input =CleanInputFile("day1.txt");
     var result = new List<int>();
     foreach (var line in input)
     {
         int? left = null;
         int? right = null;
         var x = new string[] {"one", "two", "three", "four", "five", "six", "seven", "eight", "nine"};
         
         for (int i = 0; i < line.Length; i++)
         {
             if (Char.IsDigit(line[i]))
             {
                 left = int.Parse(line[i].ToString());
                 break;
             }

             var match = x.Select((num, xi) => num == line.Substring(i, Math.Min(num.Length, line.Length - i )) ? xi : -1).Where(ix => ix != -1).ToArray();
             if (match.Length > 0)
             {
                 left = match[0] + 1;
                 break;
             }
             
         }

         for (int i = line.Length - 1; i >= 0; i--)
         {
             
             if (Char.IsDigit(line[i]))
             {
                 right = int.Parse(line[i].ToString());
                 break;
             }
             var match = x.Select((num, xi) => num == line.Substring(i,  Math.Min(num.Length, line.Length - i)) ? xi : -1).Where(ix => ix != -1).ToArray();
             if (match.Length > 0)
             {
                 right = match[0] + 1;
                 break;
             }
         }
         
         result.Add(int.Parse(left.ToString() + right.ToString()));
     }
    Console.WriteLine(result.Sum());
}
}