using System;

namespace DidasUtils.Data
{
    /// <summary>
    /// Holds a variable size list of bits (bools).
    /// </summary>
    /// <remarks>Booleans are stored in order, but all other values are stored in reverse bit order. Do not mix bool methods with other methods</remarks>
    public class BitList
    {
        //1010 => [0] = false and [1] = true
        private const int blockSize = sizeof(int) * 8;



        /// <summary>
        /// Gets or sets the value at the given index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public bool this[int index]
        {
            get
            {
                if (index >= Count) throw new ArgumentOutOfRangeException(nameof(index));

                int internalIndex = index / blockSize;
                int bit = index % blockSize;

                return (data[internalIndex] & (0x01 << bit)) != 0;
            }

            set
            {
                if (index >= Count) throw new ArgumentOutOfRangeException(nameof(index));

                int internalIndex = index / blockSize;
                int bit = index % blockSize;

                if (value)
                    data[internalIndex] |= 1 << bit;
                else
                    data[internalIndex] ^= 1 << bit;
            }
        }



        /// <summary>
        /// The count of values stored in the list.
        /// </summary>
        public int Count { get; private set; }
        /// <summary>
        /// The current capacity of the list.
        /// </summary>
        public int Capacity { get => data.Length * blockSize; }



        private int[] data; 



        /// <summary>
        /// Initializes an empty BitList.
        /// </summary>
        public BitList()
        {
            data = Array.Empty<int>();
            Count = 0;
        }
        /// <summary>
        /// Initializes a BitList with a minimum given starting capacity.
        /// </summary>
        /// <param name="capacity"></param>
        public BitList(int capacity)
        {
            data = Array.Empty<int>();
            EnsureCapacity(capacity);
            Count = 0;
        }
        /// <summary>
        /// Initializes a BitList holding each bit in the given array.
        /// </summary>
        /// <param name="source"></param>
        public BitList(bool[] source)
        {
            data = Array.Empty<int>();
            int head = 0;
            EnsureCapacity(source.Length);
            while (head < source.Length)
                FastSet(head++, source[head]);

            Count = source.Length;
        }
        /// <summary>
        /// Initializes a BitList holding each bit in the given array.
        /// </summary>
        /// <param name="source"></param>
        public BitList(byte[] source)
        {
            int len = Mathf.DivideRoundUp(source.Length, sizeof(int));
            data = new int[len];
            Buffer.BlockCopy(source, 0, data, 0, len);
            Count = source.Length * 8;
        }
        /// <summary>
        /// Initializes a BitList holding each bit in the given array.
        /// </summary>
        /// <param name="source"></param>
        public BitList(int[] source)
        {
            data = new int[source.Length];
            Array.Copy(source, data, source.Length);
            Count = source.Length * blockSize;
        }



        /// <summary>
        /// Appends a bool to the end of the list.
        /// </summary>
        /// <param name="value"></param>
        public void Add(bool value)
        {
            EnsureCapacity(++Count);
            FastSet(Count - 1, value);
        }
        /// <summary>
        /// Appends a range of packed bools to the end of the list.
        /// </summary>
        /// <param name="values"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void AddRange(bool[] values)
        {
            if (values == null) throw new ArgumentNullException(nameof(values));

            int iHead = Count, eHead = 0;

            Count += values.Length;
            EnsureCapacity(Count);

            while (eHead < values.Length)
                FastSet(iHead++, values[eHead++]);
        }
        /// <summary>
        /// Appends a range of packed bools to the end of the list.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="count"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public void AddRange(byte[] values, int count)
        {
            if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count));
            if (values == null) throw new ArgumentNullException(nameof(values));
            if (count > values.Length * 8) throw new ArgumentOutOfRangeException(nameof(count));

            int eHead = 0, iHead = count;
            Count += count;
            EnsureCapacity(Count);

            while (eHead < count)
            {
                int B = eHead / 8, b = eHead % 8;
                FastSet(iHead++, (values[B] & 1 << b) != 0);
                eHead++;
            }
        }
        /// <summary>
        /// Appends a range of packed bools to the end of the list.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="count"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public void AddRange(int[] values, int count)
        {
            if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count));
            if (values == null) throw new ArgumentNullException(nameof(values));
            if (count > values.Length * 8 * sizeof(int)) throw new ArgumentOutOfRangeException(nameof(count));

            int eHead = 0, iHead = count;
            Count += count;
            EnsureCapacity(Count);

            while (eHead < count)
            {
                int B = eHead / (8 * sizeof(int)), b = eHead % (8 * sizeof(int));
                FastSet(iHead++, (values[B] & 1 << b) != 0);
                eHead++;
            }
        }
        /// <summary>
        /// Pops the last element of the list and returns it.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public bool Pop()
        {
            if (Count == 0) throw new InvalidOperationException();

            return FastGet(Count--);
        }
        /// <summary>
        /// Removes the last <paramref name="count"/> elements from the list.
        /// </summary>
        /// <param name="count"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void Pop(int count)
        {
            if (Count < count) throw new InvalidOperationException();
            Count -= count;
        }
        /// <summary>
        /// Removes the last <paramref name="count"/> elements and trims the list.
        /// </summary>
        /// <param name="count"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void PopAndTrim(int count)
        {
            if (Count < count) throw new InvalidOperationException();
            Count -= count;
            Clean();
        }
        /// <summary>
        /// Empties the list.
        /// </summary>
        public void Clear()
        {
            Count = 0;
        }
        /// <summary>
        /// Empties and trims the list.
        /// </summary>
        public void ClearAndTrim()
        {
            Count = 0;
            Clean();
        }
        /// <summary>
        /// Ensures the list has a minimum capacity.
        /// </summary>
        /// <param name="capacity"></param>
        public void SetMinCapacity(int capacity)
        {
            EnsureCapacity(capacity);
        }
        /// <summary>
        /// Trims off unsused capacity from the list to reduce memory usage
        /// </summary>
        public void Trim()
        {
            Clean();
        }
        /// <summary>
        /// Truncates the list to hole only a given amount of values.
        /// </summary>
        /// <param name="finalCount"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void Truncate(int finalCount)
        {
            if (finalCount < 0) throw new ArgumentOutOfRangeException(nameof(finalCount));
            Count = finalCount;
        }
        /// <summary>
        /// Truncates the list to hole only a given amount of values and then trims it.
        /// </summary>
        /// <param name="finalCount"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void TruncateAndTrim(int finalCount)
        {
            if (finalCount < 0) throw new ArgumentOutOfRangeException(nameof(finalCount));
            Count = finalCount;
            Clean();
        }



        /// <summary>
        /// Gets a byte from the BitList at the given index, padding with zeros the empty space.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public byte GetByte(int index)
        {
            if (index < 0) throw new ArgumentOutOfRangeException(nameof(index));
            if (index >= Count) throw new ArgumentOutOfRangeException(nameof(index));

            byte result = 0;

            int bit = 0;

            while (bit < 8 && bit + index < Count)
            {
                if (FastGet(index + bit))
                    result |= (byte)(1 << bit);

                bit++;
            }

            return result;
        }
        /// <summary>
        /// Gets a sbyte from the BitList at the given index, padding with zeros the empty space.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public sbyte GetSbyte(int index)
        {
            if (index < 0) throw new ArgumentOutOfRangeException(nameof(index));
            if (index >= Count) throw new ArgumentOutOfRangeException(nameof(index));

            sbyte result = 0;

            int bit = 0;

            while (bit < 8 && bit + index < Count)
            {
                if (FastGet(index + bit))
                    result |= (sbyte)(1 << bit);

                bit++;
            }

            return result;
        }
        /// <summary>
        /// Gets an ushort from the BitList at the given index, padding with zeros the empty space.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public ushort GetUshort(int index)
        {
            if (index < 0) throw new ArgumentOutOfRangeException(nameof(index));
            if (index >= Count) throw new ArgumentOutOfRangeException(nameof(index));

            ushort result = 0;

            int bit = 0;

            while (bit < 16 && bit + index < Count)
            {
                if (FastGet(index + bit))
                    result |= (ushort)(1 << bit);

                bit++;
            }

            return result;
        }
        /// <summary>
        /// Gets a short from the BitList at the given index, padding with zeros the empty space.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public short GetShort(int index)
        {
            if (index < 0) throw new ArgumentOutOfRangeException(nameof(index));
            if (index >= Count) throw new ArgumentOutOfRangeException(nameof(index));

            short result = 0;

            int bit = 0;

            while (bit < 16 && bit + index < Count)
            {
                if (FastGet(index + bit))
                    result |= (short)(1 << bit);

                bit++;
            }

            return result;
        }
        /// <summary>
        /// Gets an uint from the BitList at the given index, padding with zeros the empty space.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public uint GetUint(int index)
        {
            if (index < 0) throw new ArgumentOutOfRangeException(nameof(index));
            if (index >= Count) throw new ArgumentOutOfRangeException(nameof(index));

            uint result = 0;

            int bit = 0;

            while (bit < 32 && bit + index < Count)
            {
                if (FastGet(index + bit))
                    result |= (uint)(1 << bit);

                bit++;
            }

            return result;
        }
        /// <summary>
        /// Gets an int from the BitList at the given index, padding with zeros the empty space.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public int GetInt(int index)
        {
            if (index < 0) throw new ArgumentOutOfRangeException(nameof(index));
            if (index >= Count) throw new ArgumentOutOfRangeException(nameof(index));

            int result = 0;

            int bit = 0;

            while (bit < 32 && bit + index < Count)
            {
                if (FastGet(index + bit))
                    result |= (int)(1 << bit);

                bit++;
            }

            return result;
        }
        /// <summary>
        /// Gets an ulong from the BitList at the given index, padding with zeros the empty space.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public ulong GetUlong(int index)
        {
            if (index < 0) throw new ArgumentOutOfRangeException(nameof(index));
            if (index >= Count) throw new ArgumentOutOfRangeException(nameof(index));

            ulong result = 0;

            int bit = 0;

            while (bit < 64 && bit + index < Count)
            {
                if (FastGet(index + bit))
                {
                    ulong v = (ulong)(1 << bit);
                    result |= v;
                }

                bit++;
            }

            return result;
        }
        /// <summary>
        /// Gets a long from the BitList at the given index, padding with zeros the empty space.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public long GetLong(int index)
        {
            if (index < 0) throw new ArgumentOutOfRangeException(nameof(index));
            if (index >= Count) throw new ArgumentOutOfRangeException(nameof(index));

            long result = 0;

            int bit = 0;

            while (bit < 64 && bit + index < Count)
            {
                if (FastGet(index + bit))
                {
                    long v = (long)(1 << bit);
                    result |= v;
                }

                bit++;
            }

            return result;
        }



        /// <summary>
        /// Appends a byte to the BitList.
        /// </summary>
        /// <param name="value"></param>
        public void AddByte(byte value)
        {
            int iHead = Count;
            Count += 8;
            EnsureCapacity(Count);

            for (int b = 7; b >= 0; b--)
                FastSet(iHead++, (value & (1 << b)) != 0);
        }
        /// <summary>
        /// Appends a sbyte to the BitList.
        /// </summary>
        /// <param name="value"></param>
        public void AddSbyte(sbyte value)
        {
            int iHead = Count;
            Count += 8;
            EnsureCapacity(Count);

            for (int b = 7; b >= 0; b--)
                FastSet(iHead++, (value & (1 << b)) != 0);
        }
        /// <summary>
        /// Appends an ushort to the BitList.
        /// </summary>
        /// <param name="value"></param>
        public void AddUshort(ushort value)
        {
            int iHead = Count;
            Count += 16;
            EnsureCapacity(Count);

            for (int b = 15; b >= 0; b--)
                FastSet(iHead++, (value & (1 << b)) != 0);
        }
        /// <summary>
        /// Appends a short to the BitList.
        /// </summary>
        /// <param name="value"></param>
        public void AddShort(short value)
        {
            int iHead = Count;
            Count += 16;
            EnsureCapacity(Count);

            for (int b = 15; b >= 0; b--)
                FastSet(iHead++, (value & (1 << b)) != 0);
        }
        /// <summary>
        /// Appends an uint to the BitList.
        /// </summary>
        /// <param name="value"></param>
        public void AddUint(uint value)
        {
            int iHead = Count;
            Count += 32;
            EnsureCapacity(Count);

            for (int b = 31; b >= 0; b--)
                FastSet(iHead++, (value & (1 << b)) != 0);
        }
        /// <summary>
        /// Appends an int to the BitList.
        /// </summary>
        /// <param name="value"></param>
        public void AddInt(int value)
        {
            int iHead = Count;
            Count += 32;
            EnsureCapacity(Count);

            for (int b = 31; b >= 0; b--)
                FastSet(iHead++, (value & (1 << b)) != 0);
        }
        /// <summary>
        /// Appends an ulong to the BitList.
        /// </summary>
        /// <param name="value"></param>
        public void AddUlong(ulong value)
        {
            int iHead = Count;
            Count += 64;
            EnsureCapacity(Count);

            for (int b = 63; b >= 0; b--)
                FastSet(iHead++, (value & (ulong)(1 << b)) != 0);
        }
        /// <summary>
        /// Appends a long to the BitList.
        /// </summary>
        /// <param name="value"></param>
        public void AddLong(long value)
        {
            int iHead = Count;
            Count += 64;
            EnsureCapacity(Count);

            for (int b = 63; b >= 0; b--)
                FastSet(iHead++, (value & (long)(1 << b)) != 0);
        }



        /// <summary>
        /// Sets the value at a given index without checking if the index is within bounds.
        /// </summary>
        /// <param name="index">The index of the value to set.</param>
        /// <param name="value">The value to be set to.</param>
        public void FastSet(int index, bool value)
        {
            int internalIndex = index / blockSize;
            int bit = index % blockSize;

            if (value)
                data[internalIndex] |= 1 << bit;
            else
                data[internalIndex] &= ~(1 << bit);
        }
        /// <summary>
        /// Gets the value at a given index without checking if the index is within bounds.
        /// </summary>
        /// <param name="index">The index of the value to set.</param>
        /// <returns></returns>
        public bool FastGet(int index)
        {
            int internalIndex = index / blockSize;
            int bit = index % blockSize;

            return (data[internalIndex] & (0x01 << bit)) != 0;
        }



        /// <summary>
        /// Serializes a BitList into a byte array.
        /// </summary>
        /// <param name="list">The list to serialize.</param>
        /// <returns></returns>
        public static byte[] Serialize(BitList list)
        {
            byte[] ret = new byte[4 + (list.data.Length * sizeof(int))];
            Array.Copy(BitConverter.GetBytes(list.Count), ret, sizeof(int));
            Buffer.BlockCopy(list.data, 0, ret, sizeof(int), list.data.Length * sizeof(int));

            return ret;
        }
        //TODO: change so no need to allocate new memory
        /*public static void Serialize(Stream s, BitList list)
        {
            if (!s.CanWrite) throw new ArgumentException($"Stream {nameof(s)} must be writeable.");

            byte[] vs = Serialize(list);
            s.Write(vs, 0, vs.Length);
        }*/

        /// <summary>
        /// Deserializes a BitList from a byte array.
        /// </summary>
        /// <param name="source">The byte array to deserialize from.</param>
        /// <returns></returns>
        public static BitList Deserialize(byte[] source)
        {
            int count = BitConverter.ToInt32(source, 0);
            BitList list = new BitList(count);
            Buffer.BlockCopy(source, sizeof(int), list.data, 0, source.Length - sizeof(int));

            return list;
        }



        private void EnsureCapacity(int capacity)
        {
            if (Capacity < capacity)
            {
                int len = Mathf.DivideRoundUp(capacity, blockSize);
                int[] tmp = new int[len];
                Array.Copy(data, 0, tmp, 0, data.Length);
                data = tmp;
            }
        }
        private void Clean()
        {
            if (Count + blockSize > Capacity)
            {
                int len = Mathf.DivideRoundUp(Count, blockSize);
                int[] tmp = new int[len];
                Array.Copy(data, 0, tmp, 0, len);
                data = tmp;
            }
        }
    }
}
