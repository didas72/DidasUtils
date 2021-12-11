﻿using System;
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
        /// <param name="type"></param>
        /// <param name="data"></param>
        public ErrorProtectedBlock(ErrorProtectionType type, byte[] data)
        {
            if (data.Length > 32768) throw new ArgumentException("Data must no longer than 32768 bytes (32K).");
            if (errorProtection.Length > 256) throw new ArgumentException("Error protection must no longer than 256 bytes.");

            this.errorProtectionType = type;
            this.data = data;
            this.errorProtection = GetErrorProtection(data, type);
        }



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
        public static ErrorProtectedBlock Deserialize(byte[] bytes)
        {
            ErrorProtectedBlock ret = new ErrorProtectedBlock
            {
                errorProtectionType = (ErrorProtectionType)bytes[0],
                data = new byte[BitConverter.ToUInt16(bytes, 1)]
            };
            Array.Copy(bytes, 3, ret.data, 0, ret.data.Length);

            int len = 0xff;
            switch ((ErrorProtectionType)bytes[0])
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

            Array.Copy(bytes, 3 + ret.data.Length, ret.errorProtection, 0, len);

            return ret;
        }



        public static ErrorProtectedBlock[] ProtectData(byte[] data, ErrorProtectionType protectionType, int protectedBlockSize)
        {
            List<ErrorProtectedBlock> blocks = new List<ErrorProtectedBlock>();
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

                case ErrorProtectionType.None: return Array.Empty<byte>();
                default: throw new NotImplementedException();
            }
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



        public enum ErrorProtectionType : byte
        {
            None = 0,
            CheckSum8 = 1,
            CheckSum16 = 2,
            CheckSum32 = 3,
            CheckSum64 = 4,
            Fletcher16 = 5,
            Fletcher32 = 6,
        }
    }
}
