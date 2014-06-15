﻿// Project Photo Library 0.1
// Copyright © 2013-2014. All Rights Reserved.
// 
// SUBSYSTEM:	PhotoDebug
// FILE:		DefineQuantizationTable.cs
// AUTHOR:		Greg Eakin

using System;
using System.IO;

namespace PhotoLib.Jpeg
{
    /// <summary>
    /// DQT 0xFFDB
    /// </summary>
    public class DefineQuantizationTable : JpegTag
    {
        #region Fields

        private readonly ushort length;

        private readonly byte index;

        private readonly byte[] data;

        #endregion

        // DHT: Define Huffman HuffmanTable

        #region Constructors and Destructors

        public DefineQuantizationTable(BinaryReader binaryReader)
            : base(binaryReader)
        {
            if (Mark != 0xFF || Tag != 0xDB)
            {
                throw new ArgumentException();
            }

            length = (ushort)(binaryReader.ReadByte() << 8 | binaryReader.ReadByte());

            var size = 2;
            if (size < length)
            {
                // until the length is exhausted (loads two quantization tables for baseline JPEG)
                // the precision and the quantization table index -- one byte: precision is specified by the higher four bits and index is specified by the lower four bits
                //   precision in this case is either 0 or 1 and indicates the precision of the quantized values; 8-bit (baseline) for 0 and  up to 16-bit for 1
                // the quantization values -- 64 bytes
                // the quantization tables are stored in zigzag format

                index = binaryReader.ReadByte();
                data = binaryReader.ReadBytes(length - 3);
                size += data.Length;
            }

            if (size != length)
            {
                throw new ArgumentException();
            }
        }

        #endregion

        #region Public Properties

        public ushort Length
        {
            get
            {
                return length;
            }
        }

        public byte Index
        {
            get
            {
                return index;
            }
        }

        public byte[] Data
        {
            get
            {
                return data;
            }
        }

        #endregion
    }
}