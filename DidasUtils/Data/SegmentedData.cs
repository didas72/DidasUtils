using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Net.Sockets;

namespace DidasUtils.Data
{
    /// <summary>
    /// Class to enable safer tranfer of data over a NetworkStream.
    /// </summary>
    public static class SegmentedData
    {
        private const int headerSize = 8;



        /// <summary>
        /// Sends a byte array in a safer way.
        /// </summary>
        /// <param name="data">The byte array containing the data to be sent.</param>
        /// <param name="stream">The stream to send the data to.</param>
        /// <param name="blockSize">The size of the blocks to be sent.</param>
        public static void SendToStream(byte[] data, Stream stream, int blockSize)
        {
            if (data == null)
                throw new ArgumentException("Data must not be null.");
            if (data.Length <= 0)
                throw new ArgumentException("Data must not be empty.");
            if (stream == null)
                throw new ArgumentException("Stream must not be null.");
            if (!stream.CanWrite)
                throw new ArgumentException("Stream must be writeable.");
            if (blockSize <= 256)
                throw new ArgumentException("Block size must be at least 256 bytes.");

            int dataBlockSize = blockSize - headerSize;
            int remainingBlocks = Mathf.DivideRoundUp(data.Length, dataBlockSize);
            int head = 0;
            byte[] buffer = new byte[blockSize];

            while (remainingBlocks > 0)
            {
                int dataInBlock = Math.Min(data.Length - head, dataBlockSize);

                //add header containing: block size, remaining blocks and size of data block
                BitConverter.GetBytes(--remainingBlocks).CopyTo(buffer, 0);
                BitConverter.GetBytes(dataInBlock).CopyTo(buffer, 4);
                Array.Copy(data, head, buffer, 8, dataInBlock);
                //no need to clear padding part of the buffer, data is ignored anyway
                stream.Write(buffer, 0, blockSize);

                head += dataInBlock;

                if (stream is not NetworkStream)
                    stream.Flush();
            }
        }

        /// <summary>
        /// Reads a byte array received in blocks.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="blockSize">The expected size of the blocks to receive.</param>
        /// <param name="timeout">The maximum time the operation is allowed to take.</param>
        /// <returns></returns>
        public static byte[] ReadFromStream(Stream stream, int blockSize, TimeSpan timeout = new())
        {
            if (!stream.CanSeek)
                throw new ArgumentException("Stream must be seekable. If using a NetworkStream, use ReadFromSocket.");
            if (!stream.CanRead)
                throw new ArgumentException("Stream must be readable.");
            if (blockSize <= 256)
                throw new ArgumentException("Block size must be at least 256 bytes.");

            DateTime start = DateTime.Now;

            List<byte> bytes = new();

            while (true)
            {
                byte[] block = new byte[blockSize];

                while (stream.Length - stream.Position >= blockSize)
                {
                    if (DateTime.Now - start > timeout) return Array.Empty<byte>();
                    Thread.Sleep(1);
                }

                stream.Read(block, 0, blockSize);

                int remainingBlocks = BitConverter.ToInt32(block, 0);
                int dataInBlock = BitConverter.ToInt32(block, 4);

                byte[] dataBytes = new byte[dataInBlock];
                Array.Copy(block, headerSize, dataBytes, 0, dataInBlock);

                bytes.AddRange(dataBytes);

                if (remainingBlocks == 0)
                    break;
            }

            return bytes.ToArray();
        }

        /// <summary>
        /// Reads a byte array received in blocks.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="blockSize">The expected size of the blocks to receive.</param>
        /// <param name="timeout">The maximum time the operation is allowed to take.</param>
        /// <returns></returns>
        public static byte[] ReadFromSocket(NetworkStream stream, int blockSize, TimeSpan timeout = new())
        {
            if (blockSize <= 256)
                throw new ArgumentException("Block size must be at least 256 bytes.");

            DateTime start = DateTime.Now;

            List<byte> bytes = new();

            while (true)
            {
                byte[] block = new byte[blockSize];

                while (stream.Socket.Available < blockSize)
                {
                    if (!stream.Socket.Connected)
                        return Array.Empty<byte>();
                    if (DateTime.Now - start > timeout)
                        return Array.Empty<byte>();
                    Thread.Sleep(1);
                }

                stream.Read(block, 0, blockSize);

                int remainingBlocks = BitConverter.ToInt32(block, 0);
                int dataInBlock = BitConverter.ToInt32(block, 4);

                byte[] dataBytes = new byte[dataInBlock];
                Array.Copy(block, headerSize, dataBytes, 0, dataInBlock);

                bytes.AddRange(dataBytes);

                if (remainingBlocks == 0)
                    break;
            }

            return bytes.ToArray();
        }
    }
}
