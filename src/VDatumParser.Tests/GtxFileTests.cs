using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using VDatumParser;


namespace VDatumParser.Tests
{
    public class GtxFileTests
    {
        private static GtxFile gtxFileTestObject = new GtxFile("fakePath", 29.650, 270.000, 0.001, 0.001, 7, 8, new float[]{
                        270.000f,   270.001f,   270.002f,   270.003f,   270.004f,   270.005f,   270.006f,
            29.650f,    -.2006f,    -.2006f,    -.2007f,    -.2006f,    -.2006f,    -.2006f,    -.2007f,
            29.651f,    -.2006f,    -.2007f,    -.2007f,    -.2007f,    -.2006f,    -.2006f,    -.2007f,
            29.652f,    -.2007f,    -.2007f,    -.2008f,    -.2007f,    -.2007f,    -.2006f,    -.2006f,
            29.653f,    -.2008f,    -.2008f,    -.2009f,    -.2008f,    -.2007f,    -.2006f,    -.2006f,
            29.654f,    -.2008f,    -.2008f,    -.2008f,    -.2008f,    -.2007f,    -.2006f,    -.2006f,
            29.655f,    -.2007f,    -.2007f,    -.2007f,    -.2007f,    -.2006f,    -.2006f,    -.2006f
            });

        [Fact]
        public void GetHeightReturnValueAtPoint()
        {
            double latitude = 29.653;
            double longitude = 270.002;
            float expected = -0.2009f;
            Assert.Equal(expected, gtxFileTestObject.GetHeight(latitude, longitude));
        }

        [Fact]
        public void GetHeightInterpolateReturnValueAtPoint()
        {
            double latitude = 29.6535;
            double longitude = 270.0025;
            float expected = -0.200825f;
            Assert.Equal(expected, gtxFileTestObject.GetHeight(latitude, longitude));
        }
    }
}
