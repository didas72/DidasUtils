using System;
using System.Collections.Generic;
using System.Xml.Linq;

using DidasUtils.Binary;
using DidasUtils.Math;

namespace DidasUtils.Compression
{
    public static class ArithmeticCompression
    {
        public static CompressionResult Compress(string text)
        {
            Dictionary<char, int> freqs = new Dictionary<char, int>();

            char[] chars = text.ToCharArray();

            foreach (char c in chars)
            {
                if (!freqs.ContainsKey(c))
                    freqs.Add(c, 1);
                else
                    freqs[c]++;
            }

            Dictionary<char, Tuple<Fraction, Fraction>> frequencies = new Dictionary<char, Tuple<Fraction, Fraction>>();
            Fraction total = new Fraction(0, chars.Length);

            foreach (KeyValuePair<char, int> pair in freqs)
            {
                Fraction fStart = new Fraction(total.num, total.den);
                Fraction fEnd = new Fraction(total.num + pair.Value, total.den);

                frequencies.Add(pair.Key, new Tuple<Fraction, Fraction>(fStart, fEnd));

                total += new Fraction(pair.Value, total.den);
            }

            Tuple<Fraction, Fraction> interval = new Tuple<Fraction, Fraction>(new Fraction(0, 1), new Fraction(1, 1));

            for (int i = 0; i < chars.Length; i++)
            {
                Fraction valueS = frequencies[chars[i]].Item1, valueE = frequencies[chars[i]].Item2;
                Fraction amplitude = interval.Item2 - interval.Item1;

                Fraction iS = interval.Item1 + (valueS * amplitude);
                Fraction iE = interval.Item2 - (amplitude - (valueE * amplitude));

                interval = new Tuple<Fraction, Fraction>(iS, iE);
            }


            Dictionary<char, Fraction> compiledFrequencyTable = CompileFrequencyTable(frequencies);

            long originalBits = chars.Length * sizeof(char) * 8;
            long compressedBitsMessage = interval.Item1.BitSpace();
            long compressedBitsTotal = compiledFrequencyTable.Count * (sizeof(char) + (sizeof(int) * 2 * 8));

            return new CompressionResult()
            {
                frequencyTable = compiledFrequencyTable,
                resultFraction = interval.Item1,
                originalBits = originalBits,
                compressedBitsMessage = compressedBitsMessage,
                compressedBitsTotal = compressedBitsTotal,
                compressionRatio = compressedBitsTotal / (float)originalBits,
            };
        }

        private static Dictionary<char, Fraction> CompileFrequencyTable(Dictionary<char, Tuple<Fraction,Fraction>> table)
        {
            Dictionary<char, Fraction> final = new Dictionary<char, Fraction>();

            foreach (KeyValuePair<char, Tuple<Fraction, Fraction>> pair in table)
            {
                final.Add(pair.Key, pair.Value.Item1);
            }

            return final;
        }

        public class CompressionResult
        {
            public Fraction resultFraction { get; set; }
            public Dictionary<char, Fraction> frequencyTable { get; set; }
            public long originalBits { get; set; }
            public long compressedBitsMessage { get; set; }
            public long compressedBitsTotal  { get; set; }
            public long exportedBits { get; private set; }
            public float compressionRatio { get; set; }
            public float exportedCompressionRatio { get; private set; }

            public CompressionResult()
            {

            }

            public CompressionResult(Fraction resultFraction, Dictionary<char, Fraction> frequencyTable, long originalBits, long compressedBitsMessage, long compressedBitsTotal, long exportedBitCount, float compressionRatio)
            {
                this.resultFraction = resultFraction; this.frequencyTable = frequencyTable;  this.originalBits = originalBits; this.compressedBitsMessage = compressedBitsMessage; this.compressedBitsTotal = compressedBitsTotal; this.exportedBits = exportedBitCount; this.compressionRatio = compressionRatio;
            }

            public string ExportToXml()
            {
                XDeclaration declaration = new XDeclaration("1.0", "ASCII", string.Empty);
                XDocument doc = new XDocument();
                doc.Declaration = declaration;

                XElement root = new XElement("CompressedFile");
                doc.Add(root);



                XElement resultFractionElement = new XElement("ResultFraction");

                XElement num = new XElement("Num", resultFraction.num);
                XElement den = new XElement("Den", resultFraction.den);

                resultFractionElement.Add(num);
                resultFractionElement.Add(den);

                root.Add(resultFractionElement);



                XElement frequencyTableElement = new XElement("FrequencyTable");

                foreach (KeyValuePair<char, Fraction> pair in frequencyTable)
                {
                    XElement pairElement = new XElement("Pair");

                    XElement pairChar = new XElement("Char", pair.Key);
                    XElement pairFraction = new XElement("Fraction");

                    XElement pairFractionNum = new XElement("Num", pair.Value.num);
                    XElement pairFunctionDen = new XElement("Den", pair.Value.den);
                    pairFraction.Add(pairFractionNum);
                    pairFraction.Add(pairFunctionDen);

                    pairElement.Add(pairChar);
                    pairElement.Add(pairFraction);

                    frequencyTableElement.Add(pairElement);
                }

                root.Add(frequencyTableElement);

                exportedBits = doc.ToString().Length * sizeof(char) * 8;
                exportedCompressionRatio = (float)System.Math.Round(originalBits * 100 / (float)exportedBits) / 100;

                return doc.ToString();
            }
        }
    }
}