using Xunit;

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

        private static GtxFile gtxFileTestObjectALFL = new GtxFile("fakePath", 30.171, 272.095, 0.001, 0.001, 5, 4, new float[]{
                          //272.095      272.096     272.097     272.098
            /*30.171*/      -0.2f,      -0.2f,      -0.1999f,   -0.1999f,
            /*30.172*/      -0.2f,      -0.2f,      -0.1999f,   -0.1999f,
            /*30.173*/      -0.2f,      -0.1999f,   -0.1999f,   -0.1999f,
            /*30.174*/      -0.2f,      -0.1999f,   -0.1999f,   -0.1999f,
            /*30.115*/      -0.2f,      -0.1999f,   -0.1999f,   -0.1999f
            });

        private static GtxFile gtxFileTestObjectLAmobile = new GtxFile("fakePath", 30.219, 271.939, 0.001, 0.001, 7, 4, new float[]{
                           //271.939        271.940        271.941     271.942
            /*30.219*/      -0.1859f,      -0.1859f,      -0.1860f,   -0.1860f,
            /*30.220*/      -0.1855f,      -0.1856f,      -0.1856f,   -0.1856f,
            /*30.221*/      -0.1952f,      -0.1852f,      -0.1853f,   -0.1853f,
            /*30.222*/      -0.1848f,      -0.1849f,      -0.1849f,   -0.1849f,
            /*30.223*/      -0.1845f,      -0.1845f,      -0.1845f,   -0.1845f,
            /*30.224*/      -0.1840f,      -0.1841f,      -0.1841f,   -0.1841f,
            /*30.225*/      -0.1828f,      -0.1834f,      -0.1834f,   -0.1835f

            });
 

        [Theory]
        [InlineData(29.653, 270.002, -0.2009)] //Exact point data
        [InlineData(29.6535, 270.0025, -0.200825)] //Interpolate halfway between two latitude points, exact longitude point
        [InlineData(29.6525, 270.0012, -0.20077)] //Interpolate partway between two longitude points, halfway between two longitude points
        public void GetHeight_Theory_ReturnExpected(double latitude, double longitude, double expected)
        {
            Assert.Equal(expected, gtxFileTestObject.GetHeight(latitude, longitude));
        }

        [Theory]
        [InlineData(30.171,272.095,-0.2)]//Exact point data
        [InlineData(30.173,272.0955,-0.19995)]//Interpolate halfway between two longitude points, exact latitude
        
        public void GetHeight_Theory_ReturnExpectedFromGtxFileALFLgom(double latitude, double longitude, double expected)
        {
            Assert.Equal(expected, gtxFileTestObjectALFL.GetHeight(latitude, longitude));
        }

        [Theory]
        [InlineData(30.2245, 271.940, -0.18375)]//Interpolate halfway between two latiude points, exact longitude
        [InlineData(30.220,271.940,-0.1856)]//Exact point data
        [InlineData(30.225,271.9395,-0.1831)]//Interpolate halfway between two longitude points, exact latitude
        public void GetHeight_Theory_ReturnExpectedFromGtxFileLAmobile(double latitude, double longitude, double expected)
        {
            Assert.Equal(expected.ToString(), gtxFileTestObjectLAmobile.GetHeight(latitude, longitude).ToString());
        }
    }
}
