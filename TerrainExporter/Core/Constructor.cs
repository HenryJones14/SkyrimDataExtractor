using System.Drawing;
using TerrainExporter.Data;

namespace TerrainExporter.Core
{
	public struct ConstructedData
	{
		public byte[,] test;
		public Color[,] color;
	}

	public static class Constructor
	{
		public static void ConstructData(ref Dictionary<Position, ConstructedData> Data, ParsedData[] Blocks)
		{
			for (int i = 0; i < Blocks.Length; i++)
			{
				if (!Blocks[i].PositionRecord.valid)
				{
					Console.WriteLine("Skipping block (invalid position)");
					continue;
				}

				if (Data.ContainsKey(Blocks[i].PositionRecord))
				{
					Data[Blocks[i].PositionRecord] = ConstructBlock(Data[Blocks[i].PositionRecord], in Blocks[i]);
				}
				else
				{
					Data.Add(Blocks[i].PositionRecord, ConstructBlock(null, in Blocks[i]));
				}
			}
		}

		private static ConstructedData ConstructBlock(ConstructedData? Original, in ParsedData Update)
		{
			// LRocks01 | LPineForest02 | LSnow01

			ConstructedData output;

			if (Original == null)
			{
				output = new ConstructedData();
				output.test = new byte[32, 32];
				output.color = new Color[32, 32];
			}
			else
			{
				output = Original.Value;
			}

			/*
			foreach (Quadruple<KeyValuePair<string, byte[]>> texturelayer in Update.AlphaTexRecord)
			{
				foreach (KeyValuePair<string, byte[]> item in texturelayer)
				{

				}
			}
			*/

			for (int i = 0; i < 289; i++)
			{
				int r = i / 17;
				int c = i % 17;

				if (r == 0 || c == 0)
				{
					continue;
				}

				if (c == 10 || r == 10)
				{
					output.test[r, c] = 255;
				}
			}


			for (int i = 0; i < 1089; i++)
			{
				int r = i / 33;
				int c = i % 33;

				if (r > 0 && c > 0) // discard duplicate data
				{
					if (Update.ColorRecord != null)
					{
						output.color[c - 1, r - 1] = Color.FromArgb(Update.ColorRecord[i * 3], Update.ColorRecord[i * 3 + 1], Update.ColorRecord[i * 3 + 2]);
					}
				}
			}

			return output;
		}
	}
}