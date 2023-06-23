using TerrainExporter.Data;

namespace TerrainExporter.Core
{
	public static class Exporters
	{
		public const int RESOLUTION = 4096;

		public static void ExportLandscapeHeight(in Dictionary<Position, ConstructedData> Data, string Path)
		{
			/*
			ushort[] raw = new ushort[RESOLUTION * RESOLUTION];

			for (int x = 0; x < RESOLUTION; x++)
			{
				for (int y = 0; y < RESOLUTION; y++)
				{
					raw[x * RESOLUTION + y] = 32767;
				}
			}

			for (int i = 0; i < 128; i++)
			{
				for (int j = 0; j < 128; j++)
				{
					if (Data[i, j] != null)
					{
						int transform1 = i * 32;
						int transform2 = j * 32;

						if (Data[i, j] != null)
						{
							if (Data[i, j].height.HasValue)
							{
								for (int x = 0; x < 32; x++)
								{
									for (int y = 0; y < 32; y++)
									{
										raw[(transform1 + x) * RESOLUTION + (transform2 + y)] = Data[i, j].height.Value.height[x, y];
									}
								}
							}
						}
					}
				}
			}

			using (FileStream stream = File.Open(Path + "Height.raw", FileMode.Create))
			{
				using (BinaryWriter writer = new BinaryWriter(stream, System.Text.Encoding.UTF8, false))
				{
					foreach (ushort pixel in raw)
					{
						writer.Write(pixel);
					}
				}
			}
			*/
		}

		public static void ExportLandscapeColor(in Dictionary<Position, ConstructedData> Data, string Path)
		{
			/*
			using (Bitmap png = new Bitmap(RESOLUTION, RESOLUTION, PixelFormat.Format24bppRgb))
			{
				for (int i = 0; i < 128; i++)
				{
					for (int j = 0; j < 128; j++)
					{
						int transform1 = i * 32;
						int transform2 = j * 32;

						if (Data[i, j] != null)
						{
							if (Data[i, j].color.HasValue)
							{
								for (int x = 0; x < 32; x++)
								{
									for (int y = 0; y < 32; y++)
									{
										png.SetPixel(transform1 + x, RESOLUTION - (transform2 + y) - 1, Data[i, j].color.Value.color[x, y]);
									}
								}
							}
							else
							{
								for (int x = 0; x < 32; x++)
								{
									for (int y = 0; y < 32; y++)
									{
										png.SetPixel(transform1 + x, RESOLUTION - (transform2 + y) - 1, Color.White);
									}
								}
							}
						}
						else
						{
							byte r = (byte)(i * 2);
							byte g = (byte)(j * 2);

							for (int x = 0; x < 32; x++)
							{
								for (int y = 0; y < 32; y++)
								{
									png.SetPixel(transform1 + x, RESOLUTION - (transform2 + y) - 1, Color.FromArgb(r, g, 0));
								}
							}
						}
					}
				}

				png.Save(Path + "Color.png", ImageFormat.Png);
			}
			*/
		}

		public static void ExportTextureBlend(in Dictionary<Position, ConstructedData> Data, string Path)
		{
			/*
            Dictionary<string, Bitmap> textures = new Dictionary<string, Bitmap>();

            for (int x = 0; x < 128; x++)
            {
                for (int y = 0; y < 128; y++)
                {
                    if (Data[x, y] != null)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            for (int j = 0; j < 2; j++)
                            {
                                string id = Data[x, y].texture[i, j];

                                if (id.Contains("null"))
                                {
                                    id = "_Null";
                                }
                                else
                                {
                                    if (id.Contains("snow") || id.Contains("Snow"))
                                    {
                                        id = "_Snow";
                                    }
                                    else if (id.Contains("dirt") || id.Contains("Dirt"))
                                    {
                                        id = "_Dirt";
                                    }
                                    else if (id.Contains("grass") || id.Contains("Grass"))
                                    {
                                        id = "_Grass";
                                    }
                                    else
                                    {
                                        id = "_Dirt";
                                    }
                                }

                                if (!textures.ContainsKey(id))
                                {
                                    textures.Add(id, new Bitmap(256, 256, PixelFormat.Format24bppRgb));
                                }

                                textures[id].SetPixel(x * 2 + i, 255 - (y * 2 + j), Color.White);
                            }
                        }
                    }
                }
            }

            foreach (var item in textures)
            {
                item.Value.Save(Path + item.Key + ".png", ImageFormat.Png);
            }
            */
		}

		public static void ExportWaterHeight(in Dictionary<Position, ConstructedData> Data, string Path)
		{
			/*
			List<string> lines = new List<string>() { "o Water" };
			List<string> other = new List<string>();
			int index = 0;

			for (int x = 0; x < 128; x++)
			{
				for (int y = 0; y < 128; y++)
				{
					if (Data[x, y] != null)
					{
						if (Data[x, y].water.HasValue && Data[x, y].water.Value.enabled)
						{
							lines.Add(string.Format("v {0} {1} {2}", (y - 64) * 4096, (x - 64) * -4096, Data[x, y].water.Value.height));
							lines.Add(string.Format("v {0} {1} {2}", (y - 63) * 4096, (x - 64) * -4096, Data[x, y].water.Value.height));
							lines.Add(string.Format("v {0} {1} {2}", (y - 64) * 4096, (x - 63) * -4096, Data[x, y].water.Value.height));
							lines.Add(string.Format("v {0} {1} {2}", (y - 63) * 4096, (x - 63) * -4096, Data[x, y].water.Value.height));

							other.Add(string.Format("f {0}/1/1 {1}/3/1 {2}/2/1", index + 1, index + 3, index + 2));
							other.Add(string.Format("f {0}/3/1 {1}/4/1 {2}/2/1", index + 3, index + 4, index + 2));

							index = index + 4;
						}
					}
				}
			}

			lines.Add("vt 0.0000 1.0000 0.0000");
			lines.Add("vt 1.0000 1.0000 0.0000");
			lines.Add("vt 0.0000 0.0000 0.0000");
			lines.Add("vt 1.0000 0.0000 0.0000");

			lines.Add("vn 0.0000 1.0000 0.0000");

			lines.Add("s 0");

			lines.AddRange(other);

			File.WriteAllLines(Path + "Water.obj", lines);
			*/
		}
	}
}