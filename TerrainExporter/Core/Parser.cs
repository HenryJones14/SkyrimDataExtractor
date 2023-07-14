using System;
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
				SimpleArray<string> lines = new SimpleArray<string>();
				SimpleArray<ParsedData> data = new SimpleArray<ParsedData>();

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
					else
					{
						if (line.Length > 8)
						{
							string sub = line.Substring(8);

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
					}

					// Parse block
					if (parse)
					{
						data.Add(ParseBlock(in lines));

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

#pragma warning disable CS8604 // This will never be null (null exits before coming here)

					// Add line to cashe
					lines.Add(line);

#pragma warning restore CS8604 // This will never be null (null exits before coming here)
				}

				reader.Dispose();
				return data.ToArray();
			}
		}

		private static ParsedData ParseBlock(in SimpleArray<string> Lines)
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
							bytes[j] = ToByte(hex[j]);
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
						bytes[j] = ToByte(hex[j]);
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

		public static byte ToByte(string Hex)
		{
			byte result = 0;
			result |= (byte)(Hex[0] << 4);
			result |= (byte)(Hex[1] & 0x0F);
			return result;
		}

		private static byte[] ToByteArray(string hex)
		{
			// array to put the result in
			byte[] bytes = new byte[hex.Length / 2];
			// variable to determine shift of high/low nibble
			int shift = 4;
			// offset of the current byte in the array
			int offset = 0;
			// loop the characters in the string
			foreach (char c in hex)
			{
				// get character code in range 0-9, 17-22
				// the % 32 handles lower case characters
				int b = (c - '0') % 32;
				// correction for a-f
				if (b > 9) b -= 7;
				// store nibble (4 bits) in byte array
				bytes[offset] |= (byte)(b << shift);
				// toggle the shift variable between 0 and 4
				shift ^= 4;
				// move to next byte
				if (shift != 0) offset++;
			}
			return bytes;
		}
	}
}