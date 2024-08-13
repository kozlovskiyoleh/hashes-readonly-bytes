using NUnit.Framework.Constraints;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace hashes
{
	public class ReadonlyBytes : IEnumerable<byte>
	{
		private readonly byte[] array;

		public int Length => array.Length;

		public byte this[int index]
		{
			get
			{
				if (index > array.Length || index < 0)
					throw new IndexOutOfRangeException();
				return array[index];
			}
			set
			{
                if (index > array.Length || index < 0)
                    throw new IndexOutOfRangeException();
				array[index] = value;
            }
		}

		public ReadonlyBytes(params byte[] data)
		{
			if (data == null)
				throw new ArgumentNullException(nameof(data));
			array = data;
		}

        public override int GetHashCode()
        {
            return HashCode.Combine(array);
        }

        public override string ToString()
        {
			StringBuilder str = new StringBuilder("[");
			for(int i = 0; i < array.Length; i++)
			{
				if (i == array.Length - 1)
					str.Append(array[i]);
				else
                    str.Append($"{array[i]}, ");
            }
			return str.Append("]").ToString();
        }

        public IEnumerator<byte> GetEnumerator()
        {
            int i = 0;
            while (i < array.Length)
            {
                yield return array[i];
                i++;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}