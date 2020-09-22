using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace DidasUtils.Binary
{
    public static class BinaryUtils
    {
        public static long ByteCount(object o)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, o);

            long size = stream.Length;

            stream.Dispose();

            return size; 
        }
    }
}