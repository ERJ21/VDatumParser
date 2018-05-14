using System.IO;
using VDatumParser;

namespace DemoCmd
{
    class Program
    {
        static void Main(string[] args)
        {
            string gtxFilePath = @"C:\VDatum\ALFL_gom02_8301\mllw.gtx";
            GtxFile gtxFile = new GtxFileParser().Parse(gtxFilePath);
            gtxFile.ToCsv("mllw.csv");
        }
    }

    public static class GtxFileExtensions
    {
        public static void ToCsv(this GtxFile gtxFile, string filePath)
        {
            using (var streamWriter = new StreamWriter(filePath))
            {
                //very first entry is null

                //first row = longitudes
                for (int i = 0; i < gtxFile.NumberOfColumns; i++)
                {
                    streamWriter.Write(",");
                    streamWriter.Write(gtxFile.LowerLeftLongitudeDecimalDegrees + (i * gtxFile.DeltaLongitudeDecimalDegrees));
                }
                //next row
                streamWriter.WriteLine();

                //height counter
                int nh = 0;

                //iterate through all rows/latitude
                for (int i = 0; i < gtxFile.NumberOfRows; i++)
                {
                    //first column = latitude
                    streamWriter.Write(gtxFile.LowerLeftLatitudeDecimalDegrees + (i * gtxFile.DeltaLatitudeDecimalDegrees));

                    //iterate through all columns/latitude
                    for (int j = 0; j < gtxFile.NumberOfColumns; j++)
                    {
                        streamWriter.Write(",");
                        streamWriter.Write(gtxFile.Heights[nh++]);
                    }

                    //next row
                    streamWriter.WriteLine();
                }
            }
        }
    }
}
