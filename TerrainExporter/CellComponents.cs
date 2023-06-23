using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerrainConstructor
{
    public readonly struct HeightData
    {
        public readonly ushort[,] height;

        public HeightData()
        {
            height = new ushort[32, 32];

            for (int x = 0; x < 32; x++)
            {
                for (int y = 0; y < 32; y++)
                {
                    height[x, y] = 32767; // 15bit max value (half of ushort)
                }
            }
        }

        public HeightData(string Data) // Most of the code from https://en.uesp.net/wiki/Skyrim_Mod:Mod_File_Format/LAND
        {
            string[] hex = Data.Split(' ');
            byte[] data = new byte[hex.Length];

            for (int i = 0; i < hex.Length; i++)
            {
                data[i] = Convert.ToByte(hex[i], 16);
            }


            height = new ushort[32, 32];

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



                if (r > 0 && c > 0) // discard duplicate data
                {
                    height[c - 1, r - 1] = (ushort)((offset + row_offset) / 4.0f + 32767f); // divided by 4 to compress from 32bit to 16bit and avoid overflow (original data can be divided by 8 without losing data)
                }
            }
        }
    }

    public readonly struct ColorData
    {
        public readonly Color[,] color;

        public ColorData()
        {
            color = new Color[32, 32];

            for (int x = 0; x < 32; x++)
            {
                for (int y = 0; y < 32; y++)
                {
                    color[x, y] = Color.FromArgb(255, 255, 255);
                }
            }
        }

        public ColorData(string Data)
        {
            string[] hex = Data.Split(' ');
            byte[] data = new byte[hex.Length];

            for (int i = 0; i < hex.Length; i++)
            {
                data[i] = Convert.ToByte(hex[i], 16);
            }


            color = new Color[32, 32];

            for (int i = 0; i < 1089; i++)
            {
                int r = i / 33;
                int c = i % 33;

                if (r > 0 && c > 0) // discard duplicate data
                {
                    color[c - 1, r - 1] = Color.FromArgb(data[i * 3], data[i * 3 + 1], data[i * 3 + 2]);
                }
            }
        }
    }

    public readonly struct TextureData
    {

    }

    public readonly struct WaterData
    {
        public readonly bool enabled;
        public readonly float height;

        public WaterData()
        {
            enabled = false;
            height = -14000;
        }

        public WaterData(bool Data1, string Data2) 
        {
			if (Data1)
			{
				if (Data2 == "Default" || Data2 == "4294953216.000000")
				{
					enabled = true;
					height = -14000;
				}
				else
				{
					if (float.TryParse(Data2, out float z))
					{
						enabled = true;
						height = z;
					}
					else
					{
						enabled = false;
						height = -14000;
					}
				}
			}
			else
			{
				enabled = false;
				height = 0;
			}
        }
    }

    public readonly struct ObjectData
    {

    }
}
