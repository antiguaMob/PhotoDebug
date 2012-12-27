﻿using System;
using System.Linq;

namespace FJCore.Decoder
{
    /// <summary>
    ///  The JPEGHuffmanTable class represents a Huffman table read from a
    ///  JPEG image file.  The standard JPEG AC and DC chrominance and
    ///  luminance values are provided as static fields.
    /// </summary>
    public class JpegHuffmanTable
    {
        private readonly short[] lengths;
        private readonly short[] values;

        #region Standard JPEG Huffman Tables

        public static JpegHuffmanTable StdACChrominance =
            new JpegHuffmanTable(new short[] { 0, 2, 1, 2, 4, 4, 3, 4, 7, 5,
                                           4, 4, 0, 1, 2, 0x77 },
                                 new short[] { 0x00, 0x01, 0x02, 0x03, 0x11,
                                            0x04, 0x05, 0x21, 0x31, 0x06,
                                            0x12, 0x41, 0x51, 0x07, 0x61,
                                            0x71, 0x13, 0x22, 0x32, 0x81,
                                            0x08, 0x14, 0x42, 0x91, 0xa1,
                                            0xb1, 0xc1, 0x09, 0x23, 0x33,
                                            0x52, 0xf0, 0x15, 0x62, 0x72,
                                            0xd1, 0x0a, 0x16, 0x24, 0x34,
                                            0xe1, 0x25, 0xf1, 0x17, 0x18,
                                            0x19, 0x1a, 0x26, 0x27, 0x28,
                                            0x29, 0x2a, 0x35, 0x36, 0x37,
                                            0x38, 0x39, 0x3a, 0x43, 0x44,
                                            0x45, 0x46, 0x47, 0x48, 0x49,
                                            0x4a, 0x53, 0x54, 0x55, 0x56,
                                            0x57, 0x58, 0x59, 0x5a, 0x63,
                                            0x64, 0x65, 0x66, 0x67, 0x68,
                                            0x69, 0x6a, 0x73, 0x74, 0x75,
                                            0x76, 0x77, 0x78, 0x79, 0x7a,
                                            0x82, 0x83, 0x84, 0x85, 0x86,
                                            0x87, 0x88, 0x89, 0x8a, 0x92,
                                            0x93, 0x94, 0x95, 0x96, 0x97,
                                            0x98, 0x99, 0x9a, 0xa2, 0xa3,
                                            0xa4, 0xa5, 0xa6, 0xa7, 0xa8,
                                            0xa9, 0xaa, 0xb2, 0xb3, 0xb4,
                                            0xb5, 0xb6, 0xb7, 0xb8, 0xb9,
                                            0xba, 0xc2, 0xc3, 0xc4, 0xc5,
                                            0xc6, 0xc7, 0xc8, 0xc9, 0xca,
                                            0xd2, 0xd3, 0xd4, 0xd5, 0xd6,
                                            0xd7, 0xd8, 0xd9, 0xda, 0xe2,
                                            0xe3, 0xe4, 0xe5, 0xe6, 0xe7,
                                            0xe8, 0xe9, 0xea, 0xf2, 0xf3,
                                            0xf4, 0xf5, 0xf6, 0xf7, 0xf8,
                                            0xf9, 0xfa }, false);

        public static JpegHuffmanTable StdACLuminance =
            new JpegHuffmanTable(new short[] { 0, 2, 1, 3, 3, 2, 4, 3, 5, 5,
                                          4, 4, 0, 0, 1, 0x7d },
                                 new short[] { 0x01, 0x02, 0x03, 0x00, 0x04,
                                          0x11, 0x05, 0x12, 0x21, 0x31,
                                          0x41, 0x06, 0x13, 0x51, 0x61,
                                          0x07, 0x22, 0x71, 0x14, 0x32,
                                          0x81, 0x91, 0xa1, 0x08, 0x23,
                                          0x42, 0xb1, 0xc1, 0x15, 0x52,
                                          0xd1, 0xf0, 0x24, 0x33, 0x62,
                                          0x72, 0x82, 0x09, 0x0a, 0x16,
                                          0x17, 0x18, 0x19, 0x1a, 0x25,
                                          0x26, 0x27, 0x28, 0x29, 0x2a,
                                          0x34, 0x35, 0x36, 0x37, 0x38,
                                          0x39, 0x3a, 0x43, 0x44, 0x45,
                                          0x46, 0x47, 0x48, 0x49, 0x4a,
                                          0x53, 0x54, 0x55, 0x56, 0x57,
                                          0x58, 0x59, 0x5a, 0x63, 0x64,
                                          0x65, 0x66, 0x67, 0x68, 0x69,
                                          0x6a, 0x73, 0x74, 0x75, 0x76,
                                          0x77, 0x78, 0x79, 0x7a, 0x83,
                                          0x84, 0x85, 0x86, 0x87, 0x88,
                                          0x89, 0x8a, 0x92, 0x93, 0x94,
                                          0x95, 0x96, 0x97, 0x98, 0x99,
                                          0x9a, 0xa2, 0xa3, 0xa4, 0xa5,
                                          0xa6, 0xa7, 0xa8, 0xa9, 0xaa,
                                          0xb2, 0xb3, 0xb4, 0xb5, 0xb6,
                                          0xb7, 0xb8, 0xb9, 0xba, 0xc2,
                                          0xc3, 0xc4, 0xc5, 0xc6, 0xc7,
                                          0xc8, 0xc9, 0xca, 0xd2, 0xd3,
                                          0xd4, 0xd5, 0xd6, 0xd7, 0xd8,
                                          0xd9, 0xda, 0xe1, 0xe2, 0xe3,
                                          0xe4, 0xe5, 0xe6, 0xe7, 0xe8,
                                          0xe9, 0xea, 0xf1, 0xf2, 0xf3,
                                          0xf4, 0xf5, 0xf6, 0xf7, 0xf8,
                                          0xf9, 0xfa }, false);

        public static JpegHuffmanTable StdDCChrominance =
            new JpegHuffmanTable(new short[] { 0, 3, 1, 1, 1, 1, 1, 1, 1, 1,
                                          1, 0, 0, 0, 0, 0 },
                                new short[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
                                          10, 11 }, false);

        public static JpegHuffmanTable StdDCLuminance =
            new JpegHuffmanTable(new short[] { 0, 1, 5, 1, 1, 1, 1, 1, 1, 0,
                                         0, 0, 0, 0, 0, 0 },
                                 new short[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
                                          10, 11 }, false);

        #endregion


        /// <summary>
        /// Construct and initialize a Huffman table. Copies are created of
        /// the array arguments. lengths[index] stores the number of Huffman
        /// values with Huffman codes of length index + 1. The values array
        /// stores the Huffman values in order of increasing code length.
        /// 
        /// throws ArgumentException if either parameter is null, if
        /// lengths.Length > 16 or values.Length > 256, if any value in
        /// length or values is negative, or if the parameters do not
        /// describe a valid Huffman table
        /// </summary>
        /// <param name="lengths"> an array of Huffman code lengths</param>
        /// <param name="values">a sorted array of Huffman values</param>
        public JpegHuffmanTable(short[] lengths, short[] values)
            // Create copies of the lengths and values arguments.
            : this(checkLengths(lengths), checkValues(values, lengths), true)
        {
        }

        /// <summary>
        /// Private constructor that avoids unnecessary copying and argument checking.
        /// </summary>
        /// <param name="lengths">lengths an array of Huffman code lengths</param>
        /// <param name="values">a sorted array of Huffman values</param>
        /// <param name="copy">true if copies should be created of the given arrays</param>
        private JpegHuffmanTable(short[] lengths, short[] values, bool copy)
        {
            this.lengths = copy ? (short[])lengths.Clone() : lengths;
            this.values = copy ? (short[])values.Clone() : values;
        }

        private static short[] checkLengths(short[] lengths)
        {
            if (lengths == null || lengths.Length > 16)
                throw new ArgumentException("Length array is null or too long.");

            if(lengths.Any(x => x < 0))
                throw new ArgumentException("Negative values cannot appear in the length array.");

            for (int i = 0; i < lengths.Length; i++)
            {
                if (lengths[i] > ((1 << (i + 1)) - 1))
                    throw new ArgumentException(
                        string.Format("Invalid number of codes for code length {0}", (i + 1).ToString() ));
            }

            return lengths;
        }

        private static short[] checkValues(short[] values, short[] lengths)
        {
            if (values == null || values.Length > 256)
                throw new ArgumentException("Values array is null or too long.");

            if (values.Any(x => x < 0))
                throw new ArgumentException("Negative values cannot appear in the values array.");

            if (values.Length != lengths.Sum(x => (int)x))
                throw new ArgumentException("Number of values does not match code length sum.");

            return values;
        }

        /// <summary>
        ///  Retrieve the array of Huffman code lengths.  If the
        ///  returned array is called lengthcount, there are
        ///  lengthcount[index] codes of length index + 1.
        /// </summary>
        public short[] Lengths { get { return lengths; } }
        public short[] Values { get { return values; } }

    }

}