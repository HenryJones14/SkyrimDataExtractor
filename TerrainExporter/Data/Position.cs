namespace TerrainExporter.Data
{
	///<summary>
	///2D position saved as bytes (128-255 is null)
	///</summary>
	public struct Position
	{
		private readonly byte _x;
		private readonly byte _y;

		public Position()
		{
			_x = 128;
			_y = 128;
		}

		public Position(int X, int Y)
		{
			if (X < 0 || X > 127 || Y < 0 || Y > 127)
			{
				_x = 128;
				_y = 128;
			}
			else
			{
				_x = (byte)X;
				_y = (byte)Y;
			}
		}

		public bool valid
		{
			get
			{
				if (_x > 127 || _y > 127)
				{
					return false;
				}
				else
				{
					return true;
				}
			}
		}

		public int x
		{
			get
			{
				if (_x > 127)
				{
					throw new InvalidDataException("X has invalid value!");
				}

				return x;
			}
		}

		public int y
		{
			get
			{
				if (_y > 127)
				{
					throw new InvalidDataException("Y has invalid value!");
				}

				return x;
			}
		}

		public override int GetHashCode()
		{
			if (valid)
			{
				return (((_x << 7) | _y) << 2) | 0x01;
			}
			else
			{
				return 0;
			}
		}
	}
}