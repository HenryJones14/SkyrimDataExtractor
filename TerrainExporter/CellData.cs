
namespace TerrainConstructor
{
    public class CellArguments
    {
        public int x;
        public int y;

        public HeightData? height;
        public ColorData? color;
        public TextureData? texture;
        public WaterData? water;

        public ObjectData? objects;

        public CellArguments()
        {
            x = -1;
            y = -1;

            height = null;
            color = null;
            texture = null;
            water = null;

            objects = null;
        }

        public void Reset()
        {
            x = -1;
            y = -1;

            height = null;
            color = null;
            texture = null;
            water = null;

            objects = null;
        }
    }

    public class CellData
    {
        public int x;
        public int y;

        public readonly HeightData? height;
        public readonly ColorData? color;
        public readonly TextureData? texture;
        public readonly WaterData? water;

        public readonly ObjectData? objects;

        private CellData()
        {
            x = -1;
            y = -1;

            height = null;
            color = null;
            texture = null;
            water = null;

            objects = null;
        }

        public CellData(CellArguments Update)
        {
            x = Update.x;
            y = Update.y;

            if (x < 0 || y < 0)
            {
                throw new ArgumentOutOfRangeException("Position can't be negative");
            }

            height = Update.height;
            color = Update.color;
            texture = Update.texture;
            water = Update.water;

            objects = Update.objects;
        }

        public CellData(CellArguments Original, CellArguments Update)
        {
            x = Original.x;
            y = Original.y;

            if (x < 0 || y < 0)
            {
                throw new ArgumentOutOfRangeException("Position can't be negative");
            }

            if (Update.x != x || Update.y != y)
            {
                throw new ArgumentOutOfRangeException("Position can't be different in update");
            }

            height = UpdateData(Original.height, Update.height);
            color = UpdateData(Original.color, Update.color);
            texture = UpdateData(Original.texture, Update.texture);
            water = UpdateData(Original.water, Update.water);

            objects = UpdateData(Original.objects, Update.objects);
        }

        private T UpdateData<T>(T Original, T Update) where T : new()
        {
            if (Update == null)
            {
                if (Original == null)
                {
                    return new T();
                }
                else
                {
                    return Original;
                }
            }
            else
            {
                return Update;
            }
        }

        /*private ObjectData UpdateData(ObjectData? Original, ObjectData? Update)
        {
            // Merge objects here
            // Need to be able to both add and remove
            // Remove flags: "Initially Disabled", "Deleted"
            return new ObjectData();
        }*/

        public CellArguments GetArguments()
        {
            CellArguments arguments = new CellArguments
            {
                x = x,
                y = y,

                height = height,
                color = color,
                texture = texture,
                water = water
            };

            return arguments;
        }

        /* Old
        public CellData(CellData? Original, CellArguments Update)
        {
            // Position
            {
                if (Original == null)
                {
                    x = Update.PositionX;
                    y = Update.PositionY;
                }
                else
                {
                    if (Original.x != Update.PositionX || Original.y != Update.PositionY)
                    {
                        Original = null;
                    }

                    x = Update.PositionX;
                    y = Update.PositionY;
                }
            }

            if (string.IsNullOrEmpty(Update.HeightData))
            {
                if (Original != null)
                {
                    height = Original.height;
                }
                else
                {
                    height = new ushort[33, 33];

                    for (int x = 0; x < 33; x++)
                    {
                        for (int y = 0; y < 33; y++)
                        {
                            height[x, y] = 32767;
                        }
                    }
                }
            }
            else
            {
                string[] hex = Update.HeightData.Split(' ');
                byte[] data = new byte[hex.Length];

                for (int i = 0; i < hex.Length; i++)
                {
                    data[i] = Convert.ToByte(hex[i], 16);
                }


                height = new ushort[33, 33];

                float offset = BitConverter.ToSingle(data, 0) * 8;
                float row_offset = 0;

                for (int i = 0; i < 1089; i++)
                {
                    float value = (sbyte)data[i + 4] * 8; // signed byte, each unit equals 8 game units

                    int r = i / 33;
                    int c = i % 33;

                    if (c == 0) // first column value controls height for all remaining points in cell 
                    {
                        row_offset = 0;
                        offset += value;
                    }

                    else // other col values control height of all points in the same row
                    {
                        row_offset += value;
                    }

                    value = (offset + row_offset) / 4.0f + 32767;

                    height[c, r] = (ushort)value;
                }
            }

            if (string.IsNullOrEmpty(Update.ColorData))
            {
                if (Original != null)
                {
                    color = Original.color;
                }
                else
                {
                    color = new Color[33, 33];

                    for (int x = 0; x < 33; x++)
                    {
                        for (int y = 0; y < 33; y++)
                        {
                            color[x, y] = Color.FromArgb(255, 255, 255);
                        }
                    }
                }
            }
            else
            {
                string[] hex = Update.ColorData.Split(' ');
                byte[] data = new byte[hex.Length];

                for (int i = 0; i < hex.Length; i++)
                {
                    data[i] = Convert.ToByte(hex[i], 16);
                }


                color = new Color[33, 33];

                for (int i = 0; i < 1089; i++)
                {
                    int r = i / 33;
                    int c = i % 33;

                    color[c, r] = Color.FromArgb(data[i * 3], data[i * 3 + 1], data[i * 3 + 2]);
                }
            }

            if (Update.BaseTextureData == null || Update.BaseTextureData.Length != 4)
            {
                if (Original != null)
                {
                    texture = Original.texture;
                }
                else
                {
                    texture = new string[,] { { "null", "null" }, { "null", "null" } };
                }
            }
            else
            {
                texture = Update.BaseTextureData;
            }

            // Ocean height
            if (string.IsNullOrEmpty(Update.WaterData))
            {
                if (Original != null)
                {
                    water = Original.water;
                }
                else
                {
                    water = -32000f;
                }
            }
            else
            {
                if (Update.WaterData == "Default" || Update.WaterData == "4294953216.000000")
                {
                    water = -14000f;
                }
                else
                {
                    water = float.Parse(Update.WaterData);
                }
            }
        }
        */
    }
}
