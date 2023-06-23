/*
        private static void Testing()
        {
            string[] export = new string[]
            {
                "TreePineForest01", "TreePineForest02",
                "MountainCliff01", "MountainCliff02", "MountainCliff03", "MountainCliff03trailer", "MountainCliff04", "MountainCliffCrevasse", "MountainCliffSlope", "MountainCliffSM01"
            };

            string[] lines = File.ReadAllLines("G:/_Downloads/Skyrim Terrain/xEdit.4.0.4/output.yml");
            Dictionary<string, List<Location>> objects = new Dictionary<string, List<Location>>();
            Location location;
            string id;

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("NAME "))
                {
                    try
                    {
                        id = lines[i].Split('#')[1].Split(' ')[0];
                    }
                    catch (Exception)
                    {
                        continue;
                    }

                    location = new Location();
                    location.scale = 1;

                    for (int j = 0; j < 16; j++)
                    {
                        if (lines[i + j].Contains("DATA"))
                        {
                            continue;
                        }

                        if (lines[i + j].Contains("Position:"))
                        {
                            location.posx = float.Parse(lines[i + j + 1].Split(": ")[1]);
                            location.posy = float.Parse(lines[i + j + 2].Split(": ")[1]);
                            location.posz = float.Parse(lines[i + j + 3].Split(": ")[1]);

                            continue;
                        }

                        if (lines[i + j].Contains("Rotation:"))
                        {
                            location.rotx = float.Parse(lines[i + j + 1].Split(": ")[1]);
                            location.roty = float.Parse(lines[i + j + 2].Split(": ")[1]);
                            location.rotz = float.Parse(lines[i + j + 3].Split(": ")[1]);

                            continue;
                        }

                        if (lines[i + j].Contains("Scale:"))
                        {
                            location.scale = float.Parse(lines[i + j].Split(": ")[1]);

                            continue;
                        }
                    }

                    if (!objects.ContainsKey(id))
                    {
                        objects.Add(id, new List<Location>());
                    }

                    objects[id].Add(location);
                }
            }

            //Console.WriteLine(coordinates.Count / 6.0f);

            foreach (var item in objects)
            {
                List<string> text = new List<string>();

                for (int i = 0; i < item.Value.Count; i++)
                {
                    text.Add(item.Value[i].posx.ToString());
                    text.Add(item.Value[i].posy.ToString());
                    text.Add(item.Value[i].posz.ToString());

                    text.Add(item.Value[i].rotx.ToString());
                    text.Add(item.Value[i].roty.ToString());
                    text.Add(item.Value[i].rotz.ToString());

                    text.Add(item.Value[i].scale.ToString());
                }

                File.WriteAllLines("G:/_Downloads/Skyrim Terrain/_Export/Terrain/Terrain/" + item.Key + ".txt", text);
            }

            Console.WriteLine("Done!");
            Console.ReadKey();
        }

        private static void Combine()
        {
            Bitmap flow = new Bitmap(8192, 8192, PixelFormat.Format24bppRgb);

            for (int x = 0; x < 8192; x++)
            {
                for (int y = 0; y < 8192; y++)
                {
                    flow.SetPixel(x, y, Color.FromArgb(0, 135, 0));
                }
            }

            foreach (string path in Directory.GetFiles("G:\\_Downloads\\Flow"))
            {
                string[] split = path.Split('.');

                int offsetx = (int.Parse(split[1]) + 64) * 64;
                int offsety = (int.Parse(split[2]) + 64) * 64;

                Console.WriteLine(offsetx + ":" + offsety);
                Bitmap map = new Bitmap(path);

                for (int x = 0; x < 64; x++)
                {
                    for (int y = 0; y < 64; y++)
                    {
                        Color col = map.GetPixel(x, y);
                        col = Color.FromArgb(col.G, (byte)(255 - col.R), 255);

                        flow.SetPixel(offsetx + x, 8192 - (offsety + y), col);
                    }
                }
            }

            flow.Save("G:\\_Downloads\\Flow.png", ImageFormat.Png);

            Console.WriteLine("Done!");
            Console.ReadKey();
        }
        */

    /*
    public struct Location
    {
        public float posx;
        public float posy;
        public float posz;

        public float rotx;
        public float roty;
        public float rotz;

        public float scale;
    }
    */