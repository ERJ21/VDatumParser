using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using VDatumParser;


namespace VDatumParser.Tests
{
    public class Class1
    {
        [Fact]
        public void ValueTest1()
        {
            double latitude = 29.675;
            double longitude = 272.461;
            double expected = -0.1981;
            Assert.Equal(expected, ((float)Math.Round(new GtxFileParser().Parse(@"C:\VDatum\ALFLgom02_8301\mllw.gtx").GetHeight(latitude, longitude) * 10000)) / 10000);
        }

        [Fact]
        public void ValueTest2()
        {
            double latitude = 29.677;
            double longitude = 272.508;
            double expected = -0.1980;
            Assert.Equal(expected, ((float)Math.Round(new GtxFileParser().Parse(@"C:\VDatum\ALFLgom02_8301\mllw.gtx").GetHeight(latitude, longitude) * 10000)) / 10000);
        }

        [Fact]
        public void ValueTest3()
        {
            double latitude = 30.172;
            double longitude = 271.981;
            double expected = -0.2033;
            Assert.Equal(expected, ((float)Math.Round(new GtxFileParser().Parse(@"C:\VDatum\ALFLgom02_8301\mllw.gtx").GetHeight(latitude, longitude) * 10000)) / 10000);
        }

        [Fact]
        public void ValueTest4()
        {
            double latitude = 29.693;
            double longitude = 271.945;
            double expected = -0.2008;
            Assert.Equal(expected, ((float)Math.Round(new GtxFileParser().Parse(@"C:\VDatum\ALFLgom02_8301\mllw.gtx").GetHeight(latitude, longitude) * 10000)) / 10000);
        }

        [Fact]
        public void ValueTest5()
        {
            double latitude = 29.775;
            double longitude = 271.955;
            double expected = -0.2015;
            Assert.Equal(expected, ((float)Math.Round(new GtxFileParser().Parse(@"C:\VDatum\ALFLgom02_8301\mllw.gtx").GetHeight(latitude, longitude) * 10000)) / 10000);
        }

        [Fact]
        public void ValueTest6()
        {
            double latitude = 30.206;
            double longitude = 271.970;
            double expected = -0.1997;
            Assert.Equal(expected, ((float)Math.Round(new GtxFileParser().Parse(@"C:\VDatum\ALFLgom02_8301\mllw.gtx").GetHeight(latitude, longitude) * 10000)) / 10000);
        }

        [Fact]
        public void InsideBoundsTest1()
        {
            double latitude = 29.648;
            double longitude = 274.677;
            double expected = -88.8888;
            Assert.Equal(expected, ((float)Math.Round(new GtxFileParser().Parse(@"C:\VDatum\ALFLgom02_8301\mllw.gtx").GetHeight(latitude, longitude) * 10000)) / 10000);
        }

        [Fact]
        public void InsideBoundsTest2()
        {
            double latitude = 30.409;
            double longitude = 274.677;
            double expected = -88.8888;
            Assert.Equal(expected, ((float)Math.Round(new GtxFileParser().Parse(@"C:\VDatum\ALFLgom02_8301\mllw.gtx").GetHeight(latitude, longitude) * 10000)) / 10000);
        }

        [Fact]
        public void InsideBoundsTest3()
        {
            double latitude = 30.409;
            double longitude = 271.943;
            double expected = -88.8888;
            Assert.Equal(expected, ((float)Math.Round(new GtxFileParser().Parse(@"C:\VDatum\ALFLgom02_8301\mllw.gtx").GetHeight(latitude, longitude) * 10000)) / 10000);
        }

        [Fact]
        public void InsideBoundsTest4()
        {
            double latitude = 29.648;
            double longitude = 271.943;
            double expected = -88.8888;
            Assert.Equal(expected, ((float)Math.Round(new GtxFileParser().Parse(@"C:\VDatum\ALFLgom02_8301\mllw.gtx").GetHeight(latitude, longitude) * 10000)) / 10000);
        }


        //latitude small
        [Fact]
        public void OutsideBoundsTest1()
        {
            double latitude = 29.647;
            double longitude = 274.677;
            double expected = 1;
            Assert.Equal(expected, ((float)Math.Round(new GtxFileParser().Parse(@"C:\VDatum\ALFLgom02_8301\mllw.gtx").GetHeight(latitude, longitude) * 10000)) / 10000);
        }
        
        //longitude big
        [Fact]
        public void OutsideBoundsTest2()
        {
            double latitude = 29.648;
            double longitude = 274.678;
            double expected = 1;
            Assert.Equal(expected, ((float)Math.Round(new GtxFileParser().Parse(@"C:\VDatum\ALFLgom02_8301\mllw.gtx").GetHeight(latitude, longitude) * 10000)) / 10000);
        }

        
        //latitude big
        [Fact]
        public void OutsideBoundsTest3()
        {
            double latitude = 30.410;
            double longitude = 274.677;
            double expected = 1;
            Assert.Equal(expected, ((float)Math.Round(new GtxFileParser().Parse(@"C:\VDatum\ALFLgom02_8301\mllw.gtx").GetHeight(latitude, longitude) * 10000)) / 10000);
        }

        
        //longitude small
        [Fact]
        public void OutsideBoundsTest4()
        {
            double latitude = 30.409;
            double longitude = 271.942;
            double expected = 1;
            Assert.Equal(expected, ((float)Math.Round(new GtxFileParser().Parse(@"C:\VDatum\ALFLgom02_8301\mllw.gtx").GetHeight(latitude, longitude) * 10000)) / 10000);
        }

    }
}
