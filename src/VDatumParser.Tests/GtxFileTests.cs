using Xunit;

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
            decimal latitude = 29.653M;
            decimal longitude = 270.002M;
            decimal expected = -0.2009M;
            Assert.Equal(expected.ToString(), gtxFileTestObject.GetHeight(latitude, longitude).ToString());
        }

        [Fact]
        public void GetHeightInterpolateValueAtPoint()
        {
            decimal latitude = 29.6535M;
            decimal longitude = 270.0025M;
            decimal expected = -0.200825M;
            Assert.Equal(expected.ToString(), gtxFileTestObject.GetHeight(latitude, longitude).ToString());
        }

        [Fact]
        public void GetHeightInterpolateValueAtPoint2()
        {
            decimal latitude = 29.6525M;
            decimal longitude = 270.0012M;
            decimal expected = -0.20077M;
            Assert.Equal(expected.ToString(), gtxFileTestObject.GetHeight(latitude, longitude).ToString());
        }
    }
}
