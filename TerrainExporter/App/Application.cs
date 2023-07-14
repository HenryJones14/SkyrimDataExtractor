using TerrainExporter.Core;
using TerrainExporter.Data;

namespace TerrainExporter.App
{
	public static class Application
	{
		public static DirectoryInfo? InputPath { get; private set; } = null;
		public static DirectoryInfo? OutputPath { get; private set; } = null;

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
				Console.SetCursorPosition(0, 2);
			}



			// Create data
			Dictionary<Position, ConstructedData> data = new Dictionary<Position, ConstructedData>();
			bool export = false;



			// Parse all files and construct all data
			{
				Console.WriteLine();

				foreach (FileInfo file in InputPath.GetFiles())
				{
					Console.WriteLine();

					{
						ParsedData[] parsed;
						uint id;

						// Parse
						id = Debug.StartProcess("parsing: ", file.Name);
						parsed = Parser.ParseData(file.FullName);
						Debug.EndProcess(id, " Finished!");

						// Construct
						id = Debug.StartProcess("Constructing: ", file.Name);
						Constructor.ConstructData(ref data, parsed);
						Debug.EndProcess(id, " Finished!");
					}

					export = true;
				}
			}



			// Export all data
			if (export)
			{
				Console.WriteLine();
				Console.WriteLine();
				uint id;


				id = Debug.StartProcess("Exporting: ", "LandscapeHeight");
				Exporters.ExportLandscapeHeight(in data, OutputPath.FullName);
				Debug.EndProcess(id, " Exported!");


				id = Debug.StartProcess("Exporting: ", "LandscapeColor");
				Exporters.ExportLandscapeColor(in data, OutputPath.FullName);
				Debug.EndProcess(id, " Exported!");


				id = Debug.StartProcess("Exporting: ", "TextureBlend");
				Exporters.ExportTextureBlend(in data, OutputPath.FullName);
				Debug.EndProcess(id, " Exported!");


				id = Debug.StartProcess("Exporting: ", "WaterHeight");
				Exporters.ExportWaterHeight(in data, OutputPath.FullName);
				Debug.EndProcess(id, " Exported!");
			}



			// Finish
			Console.WriteLine(); Console.WriteLine();
			Console.WriteLine("Done with everything!");
			Console.ReadKey();
		}
	}
}