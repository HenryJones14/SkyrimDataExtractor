using TerrainExporter.Data;

namespace TerrainExporter.Core
{
	public struct ParsedData
	{
		public Position PositionRecord;

		public byte[] HeightRecord;
		public byte[] ColorRecord;
		public float WaterRecord;

		public Quadruple<string> BaseTexRecord;
		public List<Quadruple<KeyValuePair<string, byte[]>>> AlphaTexRecord;
	}

	public static class Parser
	{
		public static ParsedData[] ParseData(string Path)
		{
			using (StreamReader reader = new StreamReader(Path))
			{
				List<string> lines = new List<string>();
				List<ParsedData> data = new List<ParsedData>();

				string? line = null;
				bool cell = false;
				bool parse = false;

				while (true)
				{
					// Read line
					line = reader.ReadLine();
					if (line == null)
					{
						parse = true;
					}

					// Seperate block
					if(!parse)
					{
						string sub = line;

						try
						{
							sub = line.Substring(8);
						}
						catch (Exception)
						{

						}

						if (sub.StartsWith(": #") && sub.EndsWith("[CELL]"))
						{
							cell = true;
						}
						else if (cell)
						{
							if (sub.StartsWith(": #") && (sub.EndsWith("[LAND]") || sub.EndsWith("[REFR]")))
							{
								parse = true;
							}
						}
					}

					// Parse block
					if (parse)
					{
						data.Add(ParseBlock(lines.ToArray()));

						if (line != null)
						{
							parse = false;
							cell = false;
							lines.Clear();
						}
						else
						{
							break;
						}
					}

					// Add line to cashe
					lines.Add(line);
				}

				reader.Dispose();
				return data.ToArray();
			}
		}

		private static ParsedData ParseBlock(string[] Lines)
		{
			ParsedData data = new ParsedData();

			for (int i = 0; i < Lines.Length; i++)
			{
				if (Lines[i].Contains("ATXT")) // Alpha texture
				{
					string texture = Lines[i+1].Split('#')[1];
					texture = texture.Replace(" [LTEX]", "");

					byte[] bytes = new byte[0];

					if (Lines[i + 5].Contains("VTXT"))
					{
						string[] hex = Lines[i + 5].Split('\"')[1].Split(' ');
						bytes = new byte[hex.Length];

						for (int j = 0; j < hex.Length; j++)
						{
							bytes[j] = Convert.ToByte(hex[j], 16);
						}
					}

					int index = int.Parse(Lines[i + 4].Split(": ")[1]);

					if (data.AlphaTexRecord == null)
					{
						data.AlphaTexRecord = new List<Quadruple<KeyValuePair<string, byte[]>>>();
					}

					Quadruple<KeyValuePair<string, byte[]>> value;

					if (data.AlphaTexRecord.Count <= index)
					{
						value = new Quadruple<KeyValuePair<string, byte[]>>();
						value[Lines[i + 2].Split(": ")[1]] = new KeyValuePair<string, byte[]>(texture, bytes);

						data.AlphaTexRecord.Add(value);
					}
					else
					{
						value = data.AlphaTexRecord[index];
						value[Lines[i + 2].Split(": ")[1]] = new KeyValuePair<string, byte[]>(texture, bytes);

						data.AlphaTexRecord[index] = value;
					}
				}

				if (Lines[i].Contains("BTXT")) // Base texture
				{
					string texture = Lines[i + 1].Split('#')[1];
					texture = texture.Replace(" [LTEX]", "");

					data.BaseTexRecord[Lines[i + 2].Split(": ")[1]] = texture;
				}

				if (Lines[i].Contains("XCLC")) // Position
				{
					if (int.TryParse(Lines[i + 1].Split(':')[1].Trim(), out int x) && int.TryParse(Lines[i + 2].Split(':')[1].Trim(), out int y))
					{
						data.PositionRecord = new Position(x + 64, y + 64);
					}
					continue;
				}

				if (Lines[i].Contains("VHGT")) // Height
				{
					string[] hex = Lines[i].Split('\"')[1].Split(' ');
					byte[] bytes = new byte[hex.Length];

					for (int j = 0; j < hex.Length; j++)
					{
						bytes[j] = Convert.ToByte(hex[j], 16);
					}

					data.HeightRecord = bytes;
					continue;
				}

				if (Lines[i].Contains("VCLR")) // Color
				{
					string[] hex = Lines[i].Split('\"')[1].Split(' ');
					byte[] bytes = new byte[hex.Length];

					for (int j = 0; j < hex.Length; j++)
					{
						bytes[j] = Convert.ToByte(hex[j], 16);
					}

					data.ColorRecord = bytes;
					continue;
				}

				if (Lines[i].Contains("XCLW")) // Water
				{
					string water = Lines[i].Substring(23);

					if (water == "Default" || water == "4294953216.000000")
					{
						data.WaterRecord = -14000f;
					}
					else
					{
						if (float.TryParse(water, out float z))
						{
							data.WaterRecord = z;
						}
						else
						{
							data.WaterRecord = -14000f;
						}
					}
					continue;
				}
			}

			return data;
		}
	}
}