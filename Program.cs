namespace Ac2Ep4
{
  public static class Program
  {
    // Caminho dos arquivos de entrada e saída
    static string fileInPath = "assets/testeula.ula";
    static string fileOutPath = "assets/testeula.hex";
    static string csvntOutPath = "assets/testeula.csvnt";

    // Tabela de conversão de instruções
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

    // Método de fluxo principal do programa
    static void Main(string[] args)
    {
      // Transformar o arquivo em linhas
      var parsedInstructions = ParseInstructions();

      // Processar as linhas e transformá-las em instruções à medida que for necessário
      var computedInstructions = ComputeInstructions(parsedInstructions);

      // Escrever as instruções processadas
      WriteInstructions(computedInstructions);
    }

    // Método de parse de intruções que extrai as linhas do arquivo e retorna uma lista de linhas como string
    static List<string> ParseInstructions()
    {
      using (var stream = new StreamReader(fileInPath))
      {
        var instructions = new List<string>();

        while (true)
        {
          // Ler a linha
          var res = stream.ReadLine();

          // Checar se a linha foi lida
          if (res == null)
          {
            break;
          }

          // Adicionar a linha na lista de linhas
          instructions.Add(res);

          // Verificar se o final do arquivo foi encontrado
          if (stream.EndOfStream)
          {
            break;
          }
        }

        // Remover a primeira linha encontrada (inicio:)
        instructions.RemoveAt(instructions.Count - 1);

        // Remover a última linha encontrada (fim.)
        instructions.RemoveAt(0);

        stream.Dispose();

        return instructions;
      }
    }

    // Método que computa e faz a montagem das instruções. Retorna uma lista com as instruções de 3 caracteres como string
    static List<string> ComputeInstructions(List<string> instructions)
    {
      var x = 0;
      var y = 0;

      // Instanciar uma lista vazia
      var computedInstructions = new List<string>();

      // Percorrer a lista de instruções
      instructions.ForEach((instruction) =>
      {
        // Definir o tipo da instrução: se é uma alteração no X ou Y ou se é uma instrução a ser adicionada para a escrita
        var type = instruction.Substring(0, 1);

        // Definir o valor da instrução, ou qual instrução será computada
        var value = instruction.Substring(2, instruction.Length - 3);

        // Caso seja uma instrução que deverá ser escrita no arquivo
        if (type == "W")
        {
          // Traduzir a instrução
          var inst = instructionDict[value];

          // Transformar em hexadecimal
          var hexX = x.ToString("X");
          var hexY = y.ToString("X");

          // Adicionar a instrução à lista
          computedInstructions.Add($"{hexX}{hexY}{inst}");

          return;
        }

        // Caso seja uma instrução de alteração de valor

        // Transformar a string hexadecimal em número
        var parsedValue = int.Parse(value, System.Globalization.NumberStyles.HexNumber);

        // Executar a alteração de valor de X ou de Y
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

    // Método que escreve o arquivo com as intruções já processadas, pegando-as como parâmetro
    static void WriteInstructions(List<string> instructions)
    {
      using (var stream = new StreamWriter(fileOutPath, new FileStreamOptions
      {
        Mode = FileMode.Create,
        Access = FileAccess.ReadWrite,
        Share = FileShare.None,
      }))
      {
        // Percorrer e escrever as intruções que vieram como parâmetro, linha após linha
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
        // Juntar as instruções que vieram como parâmetro em uma só linha
        var joined = String.Join(" ", instructions.ToArray());

        // Escrever a linha que foi gerada
        stream.WriteLine(joined);

        stream.Dispose();
      }
    }
  }
}