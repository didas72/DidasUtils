using System;
using System.Collections.Generic;

using DidasUtils.Binary;
using DidasUtils.Compression;
using DidasUtils.Math;
using DidasUtils.Networking;

namespace Networking
{
    static class Tester
    {
        static void Main()
        {
            int number = 1;
            List<Compression.ArithmeticCoding.CompressionResult> results = new List<Compression.ArithmeticCoding.CompressionResult>();

            for (int i = 0; i < 6; i++)
            {
                results.Add(TestArithmeticCoding(number));
                number *= 10;
            }

            for (int i = 0; i < results.Count; i++)
            {
                Console.WriteLine($"Results for {Math.Pow(10, i)} chars:");
                Console.WriteLine($"\tOriginal size: {results[i].originalBits}b");
                Console.WriteLine($"\tCompressed size (message only): {results[i].compressedBitsMessage} b");
                Console.WriteLine($"\tCompressed size (total): {results[i].compressedBitsTotal} b");
                Console.WriteLine($"\tCompresion ratio: {results[i].compressionRatio}");
                Console.WriteLine($"\tExported size: {results[i].exportedBits} b");
                Console.WriteLine($"\tExported compression ratio [BROKEN]: {results[i].exportedCompressionRatio}");
            }

            Console.ReadLine();
        }

        static Compression.ArithmeticCoding.CompressionResult TestArithmeticCoding(int charCount)
        {
            string comp = string.Empty;

            for (int i = 0; i < charCount; i++)
            {
                comp += Randomizer.GetRandomChar(i);
            }

            Compression.ArithmeticCoding.CompressionResult result = Compression.ArithmeticCoding.Compress(comp);

            result.ExportToXml();

            return result;
        }
    }
}
