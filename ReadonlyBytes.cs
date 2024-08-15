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
        public int Hash { get; private set; }
		public int Length => array.Length;
        private bool isCalculate = false;

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
            Hash = GetHashCode();
		}

        public override bool Equals(object obj)
        {
            if (obj is null || obj.GetType() != typeof(ReadonlyBytes))
                return false;
            ReadonlyBytes other = obj as ReadonlyBytes;
            return other.Hash == Hash;
        }

        public override int GetHashCode()
        {
            ulong offset_basis = 14695981039346656037;
            ulong FNV_prime = 1099511628211;
            ulong hash = offset_basis;
            unchecked
            {
                if (isCalculate)
                    return Hash;
                else 
                {
                    foreach (var item in array)
                    {
                        hash ^= item;
                        hash *= FNV_prime;
                    }
                }
                isCalculate = true;
                Hash = (int)hash;
                return Hash;
            }
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