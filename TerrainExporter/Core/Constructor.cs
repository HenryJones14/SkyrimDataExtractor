using TerrainExporter.Data;

namespace TerrainExporter.Core
{
	public struct ConstructedData
	{
		public byte[,];
	}

	public static class Constructor
	{
		public static void ConstructData(ref Dictionary<Position, ConstructedData?> Data, ParsedData[] Blocks)
		{
			for (int i = 0; i < Blocks.Length; i++)
			{
				if (!Blocks[i].PositionRecord.valid)
				{
					Console.WriteLine("Skipping block");
					continue;
				}

				if (Data[Blocks[i].PositionRecord] == null)
				{

				}

				Data[Blocks[i].PositionRecord] = ConstructBlock(in Blocks[i]);
			}
		}

		private static ConstructedData ConstructBlock(in ParsedData Block)
		{
			// LRocks01 | LPineForest02 | LSnow01

			for (int i = 0; i < Cells.Length; i++)
			{
				if (Cells[i].AlphaTexRecord == null)
				{
					continue;

					Cells[i].
				}

				for (int j = 0; j < Cells[i].AlphaTexRecord.Count)

					ushort pos = BitConverter.ToUInt16(Cells[i].AlphaTexRecord[0], 0);
			}
		}
	}
}