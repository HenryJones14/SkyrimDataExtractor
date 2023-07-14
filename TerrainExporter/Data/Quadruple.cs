namespace TerrainExporter.Data
{
	///<summary>
	///
	///</summary>
	public struct Quadruple<T> : IEnumerable<T>
	{
		private T BottomLeft;
		private T BottomRight;
		private T TopLeft;
		private T TopRight;

		public T this[int x, int y]
		{
			get
			{
				if (y == 0)
				{
					if (x == 0)
					{
						return BottomLeft;
					}
					else if (x == 1)
					{
						return BottomRight;
					}
				}
				else if (y == 1)
				{
					if (x == 0)
					{
						return TopLeft;
					}
					else if (x == 1)
					{
						return TopRight;
					}
				}

				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(x + " : " + y);
				throw new IndexOutOfRangeException("Index ranges are only 0-1!");
			}

			set
			{
				if (y == 0)
				{
					if (x == 0)
					{
						BottomLeft = value;
					}
					else if (x == 1)
					{
						BottomRight = value;
					}
				}
				else if (y == 1)
				{
					if (x == 0)
					{
						TopLeft = value;
					}
					else if (x == 1)
					{
						TopRight = value;
					}
				}

				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(x + " : " + y);
				throw new IndexOutOfRangeException("Index ranges are only 0-1!");
			}
		}

		public T this[string id]
		{
			get
			{
				switch (id)
				{
					case ("Bottom Left"):
						return BottomLeft;

					case ("Bottom Right"):
						return BottomRight;

					case ("Top Left"):
						return TopLeft;

					case ("Top Right"):
						return TopRight;


					default:
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine(id);
						throw new IndexOutOfRangeException("Index must be { Bottom Left, Bottom Right, Top Left, Top Right }!");
				}
			}

			set
			{
				switch (id)
				{
					case ("Bottom Left"):
						BottomLeft = value;
						return;

					case ("Bottom Right"):
						BottomRight = value;
						return;

					case ("Top Left"):
						TopLeft = value;
						return;

					case ("Top Right"):
						TopRight = value;
						return;


					default:
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine(id);
						throw new IndexOutOfRangeException("Index must be { Bottom Left, Bottom Right, Top Left, Top Right }!");
				}
			}
		}

		public IEnumerator<T> GetEnumerator()
		{
			yield return BottomLeft;
			yield return BottomRight;
			yield return TopLeft;
			yield return TopRight;
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}