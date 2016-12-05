﻿// Project Photo Library 0.1
// Copyright © 2013-2016. All Rights Reserved.
// 
// SUBSYSTEM:	PhotoLib
// FILE:		ImageData.cs
// AUTHOR:		Greg Eakin

using System;
using System.IO;

namespace PhotoLib.Jpeg
{
    public class ImageData
    {
        public void Reset()
        {
            EndOfFile = false;
            _currentByte = 0xFF;
            Index = -1;
            BitsLeft = -1;
            CheckByte();
        }

        private byte _currentByte;

        public ImageData(BinaryReader binaryReader, uint rawSize)
        {
            RawData = binaryReader.ReadBytes((int)rawSize);
            CheckByte();
        }

        public int BitsLeft { get; private set; } = -1;

        public bool EndOfFile { get; private set; }

        public int Index { get; private set; } = -1;

        public byte[] RawData { get; }

        public bool GetNextBit()
        {
            var bit = (_currentByte & (0x01 << BitsLeft)) != 0;
            BitsLeft--;
            CheckByte();
            return bit;
        }

        public ushort GetNextShort(ushort lastShort)
        {
            var bit = GetNextBit() ? 0x01 : 0x00;
            var retval = (lastShort << 1) | bit;
            return (ushort)retval;
        }

        private byte GetNextByte()
        {
            byte retval;

            if (EndOfFile)
            {
                throw new Exception("Reading past EOF is bad!");
            }

            if (Index < RawData.Length - 1)
            {
                retval = RawData[++Index];
                if (retval != 0xFF)
                {
                    return retval;
                }

                var code = RawData[++Index];
                switch (code)
                {
                    case 0x00:
                    case 0xFF:
                        break;

                    case 0xD9:
                        EndOfFile = true;
                        Console.WriteLine("Found 0xD9 EOI marker");
                        break;

                    default:
                        throw new Exception($"Not supposed to happen 0xFF 0x{code:X2}: Position: {RawData.Length - Index}");
                }
            }
            else
            {
                Index++;
                EndOfFile = true;
                retval = 0xFF;

                Console.WriteLine("Read to EOF");
            }

            return retval;
        }

        public ushort GetSetOfBits(ushort total)
        {
            var retval = (ushort)0u;

            var length = (ushort)Math.Min(total, BitsLeft + 1);
            while (length > 0)
            {
                var shift = BitsLeft + 1 - length;
                var mask = (0x0001 << length) - 1;
                var next = _currentByte >> shift;
                retval <<= length;
                retval |= (ushort)(next & mask);

                BitsLeft -= length;
                CheckByte();
                total -= length;
                length = (ushort)Math.Min(total, BitsLeft + 1);
            }

            return retval;
        }

        private void CheckByte()
        {
            if (BitsLeft >= 0)
                return;
            BitsLeft = 7;
            _currentByte = GetNextByte();
        }

        public byte GetValue(HuffmanTable table)
        {
            var hufIndex = (ushort)0;
            var hufBits = (ushort)0;
            HuffmanTable.HCode hCode;
            do
            {
                hufIndex = GetNextShort(hufIndex);
                hufBits++;
            }
            while (!table.Dictionary.TryGetValue(hufIndex, out hCode) || hCode.Length != hufBits);

            return hCode.Code;
        }

        public ushort GetValue(int bits)
        {
            var hufIndex = (ushort)0;
            for (var i = 0; i < bits; i++)
                hufIndex = GetNextShort(hufIndex);

            return hufIndex;
        }

        public int DistFromEnd
        {
            get
            {
                if (Index < 0)
                    return -1;
                return RawData.Length - Index;
            }
        }
    }
}