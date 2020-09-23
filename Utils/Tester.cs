using System;
using System.Collections.Generic;

using DidasUtils.Binary;
using DidasUtils.Compression;
using DidasUtils.Math;
using DidasUtils.Networking;

namespace Testing
{
    static class Tester
    {
        static void Main()
        {
            int number = 1;
            List<ArithmeticCompression.CompressionResult> results = new List<ArithmeticCompression.CompressionResult>();

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

        static ArithmeticCompression.CompressionResult TestArithmeticCoding(int charCount)
        {
            string comp = string.Empty;

            for (int i = 0; i < charCount; i++)
            {
                comp += Randomizer.GetRandomChar(i);
            }

            ArithmeticCompression.CompressionResult result = ArithmeticCompression.Compress(comp);

            result.ExportToXml();

            return result;
        }
    }
}
