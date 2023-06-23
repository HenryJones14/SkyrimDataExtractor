using TerrainExporter.Core;
using TerrainExporter.Data;

namespace TerrainExporter
{
	public static class Application
	{
		public static DirectoryInfo InputPath { get; private set; } = null;
		public static DirectoryInfo OutputPath { get; private set; } = null;

		public static void Main(string[] Arguments)
		{
			// Prepare directories
			{
				Console.ForegroundColor = ConsoleColor.White;

				Console.WriteLine("Input: ");
				Console.WriteLine("Output: ");


				for (int i = 0; i < Arguments.Length; i++)
				{
					Arguments[i] = Arguments[i].Trim();

					if (Arguments[i].StartsWith('\'') && Arguments[i].EndsWith('\''))
					{
						Arguments[i] = Arguments[i][1..^1];

						if (Arguments[i].ToLower().Contains("input="))
						{
							try
							{
								InputPath = Directory.CreateDirectory(Arguments[i].Split('=')[1]);
							}
							catch (Exception)
							{

							}

							continue;
						}

						if (Arguments[i].ToLower().Contains("output="))
						{
							try
							{
								OutputPath = Directory.CreateDirectory(Arguments[i].Split('=')[1]);
							}
							catch (Exception)
							{

							}

							continue;
						}
					}
				}


				if (InputPath == null || !InputPath.Exists)
				{
					InputPath = Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "Input\\");
				}

				if (OutputPath == null || !OutputPath.Exists)
				{
					OutputPath = Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "Output\\");
				}


				Console.ForegroundColor = ConsoleColor.Green;

				Console.SetCursorPosition(7, 0);
				Console.Write(InputPath.FullName);

				Console.SetCursorPosition(8, 1);
				Console.Write(OutputPath.FullName);


				Console.ForegroundColor = ConsoleColor.White;
				Console.SetCursorPosition(0, 3);
			}



			// Create data
			Dictionary<Position, ConstructedData> data = new Dictionary<Position, ConstructedData>();
			bool export = false;



			// Parse all files and construct all data
			{
				foreach (FileInfo file in InputPath.GetFiles())
				{
					ParsedData[] parsed;

					// Parse
					{
						Console.ForegroundColor = ConsoleColor.White;
						Console.Write("Parsing: ");

						Console.ForegroundColor = ConsoleColor.Green;
						Console.Write(file.Name);

						int parseleft = Console.CursorLeft;
						int parsetop = Console.CursorTop;

						Console.ForegroundColor = ConsoleColor.White;
						Console.WriteLine();

						parsed = Parser.ParseData(file.FullName);

						int left = Console.CursorLeft;
						int top = Console.CursorTop;

						Console.SetCursorPosition(parseleft, parsetop);
						Console.Write(" Done!");

						Console.SetCursorPosition(left, top);
					}

					// Construct
					{
						Console.ForegroundColor = ConsoleColor.White;
						Console.Write("Constructing: ");

						Console.ForegroundColor = ConsoleColor.Green;
						Console.Write(file.Name);

						int constructleft = Console.CursorLeft;
						int constructtop = Console.CursorTop;

						Console.ForegroundColor = ConsoleColor.White;
						Console.WriteLine();

						Constructor.ConstructData(ref data, parsed);

						int left = Console.CursorLeft;
						int top = Console.CursorTop;

						Console.SetCursorPosition(constructleft, constructtop);
						Console.Write(" Done!");

						Console.SetCursorPosition(left, top);
					}

					export = true;
				}
			}



			// Export all data
			if (export)
			{
				Exporters.ExportLandscapeHeight(in data, OutputPath.FullName);
				Exporters.ExportTextureBlend(in data, OutputPath.FullName);
				Exporters.ExportWaterHeight(in data, OutputPath.FullName);
				Exporters.ExportLandscapeColor(in data, OutputPath.FullName);
			}



			// Finish
			Console.WriteLine("Done with everything!");
			Console.ReadKey();
		}
	}
}