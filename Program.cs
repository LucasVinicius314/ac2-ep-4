namespace Ac2Ep4
{
  public static class Program
  {
    static string fileInPath = "assets/testeula.ula";
    static string fileOutPath = "assets/testeula.hex";
    static string csvntOutPath = "assets/testeula.csvnt";

    static Dictionary<string, string> instructionDict = new Dictionary<string, string>
    {
      ["An"] = "0",
      ["nAoB"] = "1",
      ["AnB"] = "2",
      ["zeroL"] = "3",
      ["nAeB"] = "4",
      ["Bn"] = "5",
      ["AxB"] = "6",
      ["ABn"] = "7",
      ["AnoB"] = "8",
      ["nAxB"] = "9",
      ["copiaB"] = "A",
      ["AB"] = "B",
      ["umL"] = "C",
      ["AoBn"] = "D",
      ["AoB"] = "E",
      ["copiaA"] = "F",
    };

    static void Main(string[] args)
    {
      var parsedInstructions = ParseInstructions();

      var computedInstructions = ComputeInstructions(parsedInstructions);

      WriteInstructions(computedInstructions);
    }

    static List<string> ParseInstructions()
    {
      using (var stream = new StreamReader(fileInPath))
      {
        var instructions = new List<string>();

        while (true)
        {
          var res = stream.ReadLine();

          if (res == null)
          {
            break;
          }

          instructions.Add(res);

          if (stream.EndOfStream)
          {
            break;
          }
        }

        instructions.RemoveAt(instructions.Count - 1);
        instructions.RemoveAt(0);

        stream.Dispose();

        return instructions;
      }
    }

    static List<string> ComputeInstructions(List<string> instructions)
    {
      var x = 0;
      var y = 0;

      var computedInstructions = new List<string>();

      instructions.ForEach((instruction) =>
      {
        var type = instruction.Substring(0, 1);

        var value = instruction.Substring(2, instruction.Length - 3);

        if (type == "W")
        {
          var inst = instructionDict[value];

          var hexX = x.ToString("X");
          var hexY = y.ToString("X");

          computedInstructions.Add($"{hexX}{hexY}{inst}");

          return;
        }

        var parsedValue = int.Parse(value, System.Globalization.NumberStyles.HexNumber);

        switch (type)
        {
          case "X":
            x = parsedValue;
            break;

          case "Y":
            y = parsedValue;
            break;
        }
      });

      return computedInstructions;
    }

    static void WriteInstructions(List<string> instructions)
    {
      using (var stream = new StreamWriter(fileOutPath, new FileStreamOptions
      {
        Mode = FileMode.Create,
        Access = FileAccess.ReadWrite,
        Share = FileShare.None,
      }))
      {
        instructions.ForEach((inst) => stream.WriteLine(inst));

        stream.Dispose();
      }

      using (var stream = new StreamWriter(csvntOutPath, new FileStreamOptions
      {
        Mode = FileMode.Create,
        Access = FileAccess.ReadWrite,
        Share = FileShare.None,
      }))
      {
        var joined = String.Join(" ", instructions.ToArray());

        stream.WriteLine(joined);

        stream.Dispose();
      }
    }
  }
}