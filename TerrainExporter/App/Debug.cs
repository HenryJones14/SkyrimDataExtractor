using System.Diagnostics;
using System.Reflection.Emit;

namespace TerrainExporter.App
{
	public static class Debug
	{
		private static Dictionary<uint, (int x, int y)> Processes = new Dictionary<uint, (int x, int y)>();
		private static uint PreviousID = 0;

		public static uint StartProcess(string Label)
		{
			Console.ForegroundColor = ConsoleColor.White;
			Console.Write(Label);

			PreviousID++;
			Processes.Add(PreviousID, Console.GetCursorPosition());

			Console.WriteLine();
			return PreviousID;
		}

		public static uint StartProcess(string Label, string Value)
		{
			Console.ForegroundColor = ConsoleColor.White;
			Console.Write(Label);

			Console.ForegroundColor = ConsoleColor.Green;
			Console.Write(Value);

			PreviousID++;
			Processes.Add(PreviousID, Console.GetCursorPosition());

			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine();
			return PreviousID;
		}

		public static void EndProcess(uint ID)
		{
			int x = Console.CursorLeft;
			int y = Console.CursorTop;

			Console.SetCursorPosition(Processes[ID].x, Processes[ID].y);
			Processes.Remove(ID);

			Console.ForegroundColor= ConsoleColor.White;
			Console.Write(" Done!");

			Console.SetCursorPosition(x, y);
		}

		public static void EndProcess(uint ID, string Message)
		{
			int x = Console.CursorLeft;
			int y = Console.CursorTop;

			Console.SetCursorPosition(Processes[ID].x, Processes[ID].y);
			Processes.Remove(ID);

			Console.ForegroundColor = ConsoleColor.White;
			Console.Write(Message);

			Console.SetCursorPosition(x, y);
		}
	}
}
