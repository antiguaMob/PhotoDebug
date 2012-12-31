﻿namespace PhotoTests
{
    using System.IO;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using PhotoLib.Jpeg;
    using PhotoLib.Tiff;

    [TestClass]
    public class JpegFileTests
    {
        // http://www.impulseadventure.com/photo/jpeg-huffman-coding.html
        public static readonly byte[] SimpleData = new byte[]
                {
                     0xFF, 0xD8, 0xFF, 0xE0, 0x00, 0x10, 0x4A, 0x46, 0x49, 0x46, 0x00, 0x01, 0x02, 0x00, 0x00, 0x64,
                     0x00, 0x64, 0x00, 0x00, 0xFF, 0xEC, 0x00, 0x11, 0x44, 0x75, 0x63, 0x6B, 0x79, 0x00, 0x01, 0x00,
                     0x04, 0x00, 0x00, 0x00, 0x50, 0x00, 0x00, 0xFF, 0xEE, 0x00, 0x0E, 0x41, 0x64, 0x6F, 0x62, 0x65,
                     0x00, 0x64, 0xC0, 0x00, 0x00, 0x00, 0x01, 0xFF, 0xDB, 0x00, 0x84, 0x00, 0x02, 0x02, 0x02, 0x02,
                     0x02, 0x02, 0x02, 0x02, 0x02, 0x02, 0x03, 0x02, 0x02, 0x02, 0x03, 0x04, 0x03, 0x02, 0x02, 0x03,
                     0x04, 0x05, 0x04, 0x04, 0x04, 0x04, 0x04, 0x05, 0x06, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x06,
                     0x06, 0x07, 0x07, 0x08, 0x07, 0x07, 0x06, 0x09, 0x09, 0x0A, 0x0A, 0x09, 0x09, 0x0C, 0x0C, 0x0C,
                     0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x01, 0x03, 0x03, 0x03,
                     0x05, 0x04, 0x05, 0x09, 0x06, 0x06, 0x09, 0x0D, 0x0B, 0x09, 0x0B, 0x0D, 0x0F, 0x0E, 0x0E, 0x0E,
                     0x0E, 0x0F, 0x0F, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0F, 0x0F, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C,
                     0x0F, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C,
                     0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0xFF, 0xC0, 0x00,
                     0x11, 0x08, 0x00, 0x08, 0x00, 0x10, 0x03, 0x01, 0x11, 0x00, 0x02, 0x11, 0x01, 0x03, 0x11, 0x01,
                     0xFF, 0xC4, 0x01, 0xA2, 0x00, 0x00, 0x00, 0x07, 0x01, 0x01, 0x01, 0x01, 0x01, 0x00, 0x00, 0x00,
                     0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x05, 0x03, 0x02, 0x06, 0x01, 0x00, 0x07, 0x08, 0x09, 0x0A,
                     0x0B, 0x01, 0x00, 0x02, 0x02, 0x03, 0x01, 0x01, 0x01, 0x01, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00,
                     0x00, 0x00, 0x01, 0x00, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x10, 0x00,
                     0x02, 0x01, 0x03, 0x03, 0x02, 0x04, 0x02, 0x06, 0x07, 0x03, 0x04, 0x02, 0x06, 0x02, 0x73, 0x01,
                     0x02, 0x03, 0x11, 0x04, 0x00, 0x05, 0x21, 0x12, 0x31, 0x41, 0x51, 0x06, 0x13, 0x61, 0x22, 0x71,
                     0x81, 0x14, 0x32, 0x91, 0xA1, 0x07, 0x15, 0xB1, 0x42, 0x23, 0xC1, 0x52, 0xD1, 0xE1, 0x33, 0x16,
                     0x62, 0xF0, 0x24, 0x72, 0x82, 0xF1, 0x25, 0x43, 0x34, 0x53, 0x92, 0xA2, 0xB2, 0x63, 0x73, 0xC2,
                     0x35, 0x44, 0x27, 0x93, 0xA3, 0xB3, 0x36, 0x17, 0x54, 0x64, 0x74, 0xC3, 0xD2, 0xE2, 0x08, 0x26,
                     0x83, 0x09, 0x0A, 0x18, 0x19, 0x84, 0x94, 0x45, 0x46, 0xA4, 0xB4, 0x56, 0xD3, 0x55, 0x28, 0x1A,
                     0xF2, 0xE3, 0xF3, 0xC4, 0xD4, 0xE4, 0xF4, 0x65, 0x75, 0x85, 0x95, 0xA5, 0xB5, 0xC5, 0xD5, 0xE5,
                     0xF5, 0x66, 0x76, 0x86, 0x96, 0xA6, 0xB6, 0xC6, 0xD6, 0xE6, 0xF6, 0x37, 0x47, 0x57, 0x67, 0x77,
                     0x87, 0x97, 0xA7, 0xB7, 0xC7, 0xD7, 0xE7, 0xF7, 0x38, 0x48, 0x58, 0x68, 0x78, 0x88, 0x98, 0xA8,
                     0xB8, 0xC8, 0xD8, 0xE8, 0xF8, 0x29, 0x39, 0x49, 0x59, 0x69, 0x79, 0x89, 0x99, 0xA9, 0xB9, 0xC9,
                     0xD9, 0xE9, 0xF9, 0x2A, 0x3A, 0x4A, 0x5A, 0x6A, 0x7A, 0x8A, 0x9A, 0xAA, 0xBA, 0xCA, 0xDA, 0xEA,
                     0xFA, 0x11, 0x00, 0x02, 0x02, 0x01, 0x02, 0x03, 0x05, 0x05, 0x04, 0x05, 0x06, 0x04, 0x08, 0x03,
                     0x03, 0x6D, 0x01, 0x00, 0x02, 0x11, 0x03, 0x04, 0x21, 0x12, 0x31, 0x41, 0x05, 0x51, 0x13, 0x61,
                     0x22, 0x06, 0x71, 0x81, 0x91, 0x32, 0xA1, 0xB1, 0xF0, 0x14, 0xC1, 0xD1, 0xE1, 0x23, 0x42, 0x15,
                     0x52, 0x62, 0x72, 0xF1, 0x33, 0x24, 0x34, 0x43, 0x82, 0x16, 0x92, 0x53, 0x25, 0xA2, 0x63, 0xB2,
                     0xC2, 0x07, 0x73, 0xD2, 0x35, 0xE2, 0x44, 0x83, 0x17, 0x54, 0x93, 0x08, 0x09, 0x0A, 0x18, 0x19,
                     0x26, 0x36, 0x45, 0x1A, 0x27, 0x64, 0x74, 0x55, 0x37, 0xF2, 0xA3, 0xB3, 0xC3, 0x28, 0x29, 0xD3,
                     0xE3, 0xF3, 0x84, 0x94, 0xA4, 0xB4, 0xC4, 0xD4, 0xE4, 0xF4, 0x65, 0x75, 0x85, 0x95, 0xA5, 0xB5,
                     0xC5, 0xD5, 0xE5, 0xF5, 0x46, 0x56, 0x66, 0x76, 0x86, 0x96, 0xA6, 0xB6, 0xC6, 0xD6, 0xE6, 0xF6,
                     0x47, 0x57, 0x67, 0x77, 0x87, 0x97, 0xA7, 0xB7, 0xC7, 0xD7, 0xE7, 0xF7, 0x38, 0x48, 0x58, 0x68,
                     0x78, 0x88, 0x98, 0xA8, 0xB8, 0xC8, 0xD8, 0xE8, 0xF8, 0x39, 0x49, 0x59, 0x69, 0x79, 0x89, 0x99,
                     0xA9, 0xB9, 0xC9, 0xD9, 0xE9, 0xF9, 0x2A, 0x3A, 0x4A, 0x5A, 0x6A, 0x7A, 0x8A, 0x9A, 0xAA, 0xBA,
                     0xCA, 0xDA, 0xEA, 0xFA, 0xFF, 0xDA, 0x00, 0x0C, 0x03, 0x01, 0x00, 0x02, 0x11, 0x03, 0x11, 0x00,
                     0x3F, 0x00, 0xFC, 0xFF, 0x00, 0xE2, 0xAF, 0xEF, 0xF3, 0x15, 0x7F, 0xFF, 0xD9                
                };

        [TestMethod]
        public void SimpleImage()
        {
            using (var memory = new MemoryStream(SimpleData))
            {
                var binaryReader = new BinaryReader(memory);
                binaryReader.BaseStream.Seek(0x0000u, SeekOrigin.Begin);
                var length = (uint)SimpleData.Length;

                // NextMark E0 APP0
                
                // NextMark EC APP12
                
                // NextMark EE APP14
                
                // NextMark DB SOI
                var startOfImage = new StartOfImage(binaryReader, 0x0000u, length);
                Assert.AreEqual(0xFF, startOfImage.Mark);
                Assert.AreEqual(0xD8, startOfImage.Tag);

                // NextMark C0 SOF0

                // NextMark C4 DHT
                var huffmanTable = startOfImage.HuffmanTable;
                Assert.AreEqual(0xFF, huffmanTable.Mark);
                Assert.AreEqual(0xC4, huffmanTable.Tag);
                huffmanTable.ToString();

                // NextMark DA SOS
                var startOfScan = startOfImage.StartOfScan;
                Assert.AreEqual(0xFF, startOfScan.Mark);
                Assert.AreEqual(0xDA, startOfScan.Tag);

                var imageData = startOfImage.ImageData;
            }
        }
    }
}