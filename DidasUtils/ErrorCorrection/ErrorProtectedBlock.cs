using System;
using System.Collections.Generic;

namespace DidasUtils.ErrorCorrection
{
    /// <summary>
    /// Class the represents an error protected block of data.
    /// </summary>
    //TODO: Finish and make public
    public class ErrorProtectedBlock
    {
        /// <summary>
        /// Holds the data contained in the block.
        /// </summary>
        public byte[] data;
        /// <summary>
        /// Holds the data validation codes.
        /// </summary>
        public byte[] errorProtection;
        /// <summary>
        /// The type of error protection used.
        /// </summary>
        public ErrorProtectionType errorProtectionType;



        private ErrorProtectedBlock() { }
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="type">The type of error correction to use.</param>
        /// <param name="data">The data to protect.</param>
        public ErrorProtectedBlock(ErrorProtectionType type, byte[] data)
        {
            if (data.Length > 32768) throw new ArgumentException("Data must no longer than 32768 bytes (32K).");
            if (errorProtection.Length > 256) throw new ArgumentException("Error protection must no longer than 256 bytes.");

            this.errorProtectionType = type;
            this.data = data;
            this.errorProtection = GetErrorProtection(data, type);
        }



        /// <summary>
        /// Serializes a given ErrorProtectedBlock to a byte array.
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
        public static byte[] Serialize(ErrorProtectedBlock block)
        {
            /*
            i - l - name
            0 - 1 - protection type
            1 - 2 - data len
            3 - X - data
            X - X - error protection
             */

            byte[] ret = new byte[3 + block.data.Length + block.errorProtection.Length];

            ret[0] = (byte)block.errorProtectionType;
            Array.Copy(BitConverter.GetBytes((ushort)block.data.Length), 0, ret, 1, 2);
            Array.Copy(block.data, 0, ret, 3, block.data.Length);
            Array.Copy(block.errorProtection, 0, ret, 3 + block.data.Length, block.errorProtection.Length);

            return ret;
        }
        /// <summary>
        /// Deserializes a ErrorProtectedBlock from a byte array.
        /// </summary>
        /// <param name="bytes">The byte array to deserialize from.</param>
        /// <returns></returns>
        public static ErrorProtectedBlock Deserialize(byte[] bytes)
        {
            ErrorProtectedBlock ret = new()
            {
                errorProtectionType = (ErrorProtectionType)bytes[0],
                data = new byte[BitConverter.ToUInt16(bytes, 1)]
            };
            Array.Copy(bytes, 3, ret.data, 0, ret.data.Length);

            byte len = GetErrorProtectionLength(ret.errorProtectionType);

            Array.Copy(bytes, 3 + ret.data.Length, ret.errorProtection, 0, len);

            return ret;
        }
        /// <summary>
        /// Deserializes an array of ErrorProtectedBlock s from a byte array.
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static ErrorProtectedBlock[] DeserializeArray(byte[] bytes)
        {
            List<ErrorProtectedBlock> blocks = new();

            int head = 0;
            
            while (head < bytes.Length)
            {
                int len = 4 + GetErrorProtectionLength((ErrorProtectionType)bytes[head]) + BitConverter.ToUInt16(bytes, head + 1);
                byte[] blockBytes = new byte[len];
                Array.Copy(bytes, head, blockBytes, 0, len);

                blocks.Add(Deserialize(blockBytes));

                head += len;
            }

            return blocks.ToArray();
        }

        //TODO: Add stream deserialization.



        /// <summary>
        /// Protects a byte array.
        /// </summary>
        /// <param name="data">The data to protect.</param>
        /// <param name="protectionType">The type of error protection used.</param>
        /// <param name="protectedBlockSize">The size of the raw data block.</param>
        /// <returns>An array of ErrorProtectedBlocks holding the protected data.</returns>
        public static ErrorProtectedBlock[] ProtectData(byte[] data, ErrorProtectionType protectionType, int protectedBlockSize)
        {
            List<ErrorProtectedBlock> blocks = new();
            int head = 0;

            while (head > data.Length)
            {
                byte[] dataBlock = new byte[Math.Min(protectedBlockSize, data.Length - head)];
                Buffer.BlockCopy(data, head, dataBlock, 0, dataBlock.Length);
                head += dataBlock.Length;

                blocks.Add(new ErrorProtectedBlock(protectionType, dataBlock));
            }

            return blocks.ToArray();
        }
        /// <summary>
        /// Protects a byte array.
        /// </summary>
        /// <param name="data">The data to protect.</param>
        /// <param name="protectionType">The type of error protection used.</param>
        /// <param name="protectedBlockSize">The size of the raw data block.</param>
        /// <returns>A byte array holding the protected data.</returns>
        public static byte[] ProtectDataToArray(byte[] data, ErrorProtectionType protectionType, int protectedBlockSize)
        {
            List<byte> bytes = new();

            foreach (ErrorProtectedBlock b in ProtectData(data, protectionType, protectedBlockSize)) bytes.AddRange(Serialize(b));

            return bytes.ToArray();
        }



        private static byte[] GetErrorProtection(byte[] data, ErrorProtectionType protectionType)
        {
            switch (protectionType)
            {
                case ErrorProtectionType.CheckSum8:
                    return new byte[] { CheckSum.CheckSum8(data) };

                case ErrorProtectionType.Fletcher16:
                    return BitConverter.GetBytes(Fletcher.Fletcher16(data));

                case ErrorProtectionType.Fletcher32:
                    return BitConverter.GetBytes(Fletcher.Fletcher32(data));

                case ErrorProtectionType.None: 
                    return Array.Empty<byte>();

                default: throw new NotImplementedException();
            }
        }
        private static byte GetErrorProtectionLength(ErrorProtectionType type)
        {
            byte len = 0xff;
            switch (type)
            {
                case ErrorProtectionType.None:
                    len = 0;
                    break;

                case ErrorProtectionType.CheckSum8:
                    len = 1;
                    break;

                case ErrorProtectionType.CheckSum16:
                    len = 2;
                    break;

                case ErrorProtectionType.CheckSum32:
                    len = 4;
                    break;

                case ErrorProtectionType.CheckSum64:
                    len = 8;
                    break;

                case ErrorProtectionType.Fletcher16:
                    len = 2;
                    break;

                case ErrorProtectionType.Fletcher32:
                    len = 4;
                    break;
            }

            if (len == 0xff)
                throw new Exception("Invalid error protection type.");

            return len;
        }



        /// <summary>
        /// Validates the data block.
        /// </summary>
        /// <returns></returns>
        public bool Validate()
        {
            byte[] dataProt = GetErrorProtection(data, errorProtectionType);

            if (dataProt.Length != errorProtection.Length) return false;

            for (int i = 0; i < dataProt.Length; i++) if (dataProt[i] != errorProtection[i]) return false;

            return true;
        }



        /// <summary>
        /// Enum that defines the used error protection type.
        /// </summary>
        public enum ErrorProtectionType : byte
        {
            /// <summary>
            /// Default invalid value.
            /// </summary>
            None = 0,
            /// <summary>
            /// 
            /// </summary>
            CheckSum8 = 1,
            /// <summary>
            /// 
            /// </summary>
            CheckSum16 = 2,
            /// <summary>
            /// 
            /// </summary>
            CheckSum32 = 3,
            /// <summary>
            /// 
            /// </summary>
            CheckSum64 = 4,
            /// <summary>
            /// 
            /// </summary>
            Fletcher16 = 5,
            /// <summary>
            /// 
            /// </summary>
            Fletcher32 = 6,
        }
    }
}
