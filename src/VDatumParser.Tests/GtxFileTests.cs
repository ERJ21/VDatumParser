﻿using Xunit;

namespace VDatumParser.Tests
{
    public class GtxFileTests
    {
        private static GtxFile gtxFileTestObject = new GtxFile("fakePath", 29.650, 270.000, 0.001, 0.001, 6, 7, new float[]{
                       //270.000f,   270.001f,   270.002f,   270.003f,   270.004f,   270.005f,   270.006f,
            /*29.650f,*/    -.2006f,    -.2006f,    -.2007f,    -.2006f,    -.2006f,    -.2006f,    -.2007f,
            /*29.651f,*/    -.2006f,    -.2007f,    -.2007f,    -.2007f,    -.2006f,    -.2006f,    -.2007f,
            /*29.652f,*/    -.2007f,    -.2007f,    -.2008f,    -.2007f,    -.2007f,    -.2006f,    -.2006f,
            /*29.653f,*/    -.2008f,    -.2008f,    -.2009f,    -.2008f,    -.2007f,    -.2006f,    -.2006f,
            /*29.654f,*/    -.2008f,    -.2008f,    -.2008f,    -.2008f,    -.2007f,    -.2006f,    -.2006f,
            /*29.655f,*/    -.2007f,    -.2007f,    -.2007f,    -.2007f,    -.2006f,    -.2006f,    -.2006f
            });

        private static string gtxFilePath = @"C:\VDatum\ALFLgom02_8301\mllw.gtx";
        private static GtxFile gtxFileTestObject2 = new GtxFileParser().Parse(gtxFilePath);

        [Theory]
        [InlineData(29.653, 270.002, -0.2009)] //Exact point data
        [InlineData(29.6535, 270.0025, -0.200825)] //Interpolate halfway between two latitude points, exact longitude point
        [InlineData(29.6525, 270.0012, -0.20077)] //Interpolate partway between two longitude points, halfway between two longitude points
        public void GetHeight_Theory_ReturnExpected(decimal latitude, decimal longitude, decimal expected)
        {
            Assert.Equal(expected.ToString(), gtxFileTestObject.GetHeight(latitude, longitude).ToString());
        }

        [Theory]
        [InlineData(30.182,272.094,-0.2)]//Exact point data
        [InlineData(30.1735,272.1315,-0.198775)]//Interpolate halfway between two longitude and latitude points
        [InlineData(30.231,272.049,-0.2013)]//Exact point data
        public void GetHeight_Theory_ReturnExpectedFromGtxFile(decimal latitude, decimal longitude, decimal expected)
        {
            Assert.Equal(expected.ToString(), gtxFileTestObject2.GetHeight(latitude, longitude).ToString());
        }
    }
}
