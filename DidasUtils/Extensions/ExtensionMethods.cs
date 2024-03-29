﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace DidasUtils.Extensions
{
    /// <summary>
    /// Class that contains several extension methods.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Truncates the string to a given length.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="maxLength">The length to truncate the string to.</param>
        /// <returns>Truncated string.</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value))
                return value;
            return value.Length <= maxLength ? value : value[..maxLength];
        }
        /// <summary>
        /// Returns a string with the given length, either truncated or padded with spaces.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="length">The length to set the string to.</param>
        /// <returns>Rescaled string.</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static string SetLength(this string value, int length)
        {
            if (value.Length >= length)
                return value[..length];

            return value += " ".Loop(length - value.Length);
        }
        /// <summary>
        /// Repeats the string a given amount of times.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="times">The number of times to repeat the string.</param>
        /// <returns>Looped string.</returns>
        public static string Loop(this string value, int times)
        {
            string f = string.Empty;

            for (int i = 0; i < times; i++) f += value;

            return f;
        }
        /// <summary>
        /// Gets the first X values from an array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="amount">The number of values to return.</param>
        /// <returns></returns>
        public static T[] GetFirst<T>(this T[] array, int amount)
        {
            T[] ret = new T[amount];

            Array.Copy(array, ret, amount);

            return ret;
        }
        /// <summary>
        /// Gets the number of remaining bytes in the stream;
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static long RemainingBytes(this Stream stream)
        {
            return stream.Length - stream.Position;
        }
        /// <summary>
        /// Gets an array containing the values present in the given Dictionary.
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="dictionary">Dictionary from which to take the values.</param>
        /// <returns></returns>
        public static V[] GetValues<K, V>(this Dictionary<K, V> dictionary) => dictionary.Values.ToArray();
        /// <summary>
        /// Gets an array containing the keys present in the given Dictionary.
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="dictionary">Dictionary from which to take the keys.</param>
        /// <returns></returns>
        public static K[] GetKeys<K, V>(this Dictionary<K, V> dictionary) => dictionary.Keys.ToArray();
        /// <summary>
        /// Gets the first child with a certain name from a XmlNode.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name">The name to check for.</param>
        /// <param name="child">The child XmlNode</param>
        /// <returns>Boolean indicating the operation's success.</returns>
        public static bool GetFirstChildOfName(this XmlNode parent, string name, out XmlNode child)
        {
            child = null;

            var children = parent.ChildNodes;

            foreach (XmlNode fChild in children)
            {
                if (fChild.Name == name)
                {
                    child = fChild;
                    return true;
                }
            }

            return false;
        }
        /// <summary>
        /// Gets the indexes of all matches of the given substring.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int[] GetIndexesOf(this string str, string value)
        {
            List<int> outp = new();
            int last = 0;

            while (true)
            {
                last = str.IndexOf(value, last);

                if (last == -1) break;
                outp.Add(last);
                last++;
            }

            return outp.ToArray();
        }
        /// <summary>
        /// Adds a KeyValuePair to a dictionary.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="pair"></param>
        public static void Add<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, KeyValuePair<TKey, TValue> pair) => dictionary.Add(pair.Key, pair.Value);
        /// <summary>
        /// Converts the List of bool to a byte array and outputs the number of used bits.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="usedBits"></param>
        /// <returns></returns>
        public static int[] ToIntArray(this List<bool> self, out int usedBits)
        {
            int[] ret = new int[Mathf.DivideRoundUp(self.Count, 32)];

            int byteHeader = 0;
            int bitHeader = 0;

            for (usedBits = self.Count - 1; usedBits >= 0; usedBits--)
            {
                //last bool is high bit
                ret[byteHeader] = ret[byteHeader] << 1;

                if (self[usedBits])
                {
                    ret[byteHeader] = ret[byteHeader] | 0x01;
                }

                bitHeader++;
                if (bitHeader >= 32)
                {
                    byteHeader++;
                    bitHeader = 0;
                }
            }

            usedBits = self.Count;
            return ret;
        }
        /// <summary>
        /// Gets a random but valid value from an enum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rdm"></param>
        /// <returns></returns>
        public static T NextEnum<T>(this Random rdm)
        {
            Array v = Enum.GetValues(typeof(T));
            return (T)v.GetValue(rdm.Next(v.Length));
        }
        /// <summary>
        /// Enqueues a range of items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queue"></param>
        /// <param name="items"></param>
        public static void EnqueueRange<T>(this Queue<T> queue, T[] items)
        {
            foreach (T item in items) queue.Enqueue(item);
        }
    }
}
