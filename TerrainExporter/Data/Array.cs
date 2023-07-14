namespace TerrainExporter.Data
{
	///<summary>
	///Unsafe array for memory optimization (index is not validated)
	///</summary>
	public struct SimpleArray<T>
	{
		private T[] data;
		private int count;

		public int Length { get { return count; } }

		public SimpleArray()
		{
			data = new T[10];
			count = 0;
		}

		public T this[int index]
		{
			get
			{
				if (index >= count)
				{
					throw new IndexOutOfRangeException();
				}

				return data[index];
			}

			set
			{
				if (index >= count)
				{
					throw new IndexOutOfRangeException();
				}

				data[index] = value;
			}
		}

		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public void Add(T Item)
		{
			if (count == data.Length)
			{
				//Console.WriteLine(data.Length + " -> " + (data.Length * 2));

				T[] newdata = new T[data.Length * 2];
				Array.Copy(data, newdata, count);
				data = newdata;
			}

			data[count] = Item;
			count++;
		}

		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public void Clear()
		{
			count = 0;
		}

		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public T[] ToArray()
		{
			T[] array = new T[count];
			Array.Copy(data, array, count);
			return array;
		}
	}
}