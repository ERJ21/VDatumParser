using System.IO;
using System;

namespace VDatumParser
{
    public class GtxFileParser
    {
        public GtxFile Parse(string filePath)
        {
            using (var binaryReader = new BigEndianBinaryReader(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
            {
                double lowerLeftLatitudeDecimalDegrees = binaryReader.ReadDouble();
                double lowerLeftLongitudeDecimalDegrees = binaryReader.ReadDouble();
                double deltaLatitudeDecimalDegrees = binaryReader.ReadDouble();
                double deltaLongitudeDecimalDegrees = binaryReader.ReadDouble();
                int numberOfRows = binaryReader.ReadInt32();
                int numberOfColumns = binaryReader.ReadInt32();

                float[] heights = new float[numberOfRows * numberOfColumns];
                int n = 0;

                //iterate through all rows/latitude
                for (int i = 0; i < numberOfRows; i++)
                {
                    //iterate through all columns/latitude
                    for (int j = 0; j < numberOfColumns; j++)
                    {
                        float x = binaryReader.ReadSingle();
                        heights[n++] = x;
                    }
                }

                return new GtxFile(
                    filePath: filePath,
                    lowerLeftLatitudeDecimalDegrees: lowerLeftLatitudeDecimalDegrees,
                    lowerLeftLongitudeDecimalDegrees: lowerLeftLongitudeDecimalDegrees,
                    deltaLatitudeDecimalDegrees: deltaLatitudeDecimalDegrees,
                    deltaLongitudeDecimalDegrees: deltaLongitudeDecimalDegrees,
                    numberOfRows: numberOfRows,
                    numberOfColumns: numberOfColumns,
                    heights: Array.AsReadOnly(heights));
            }
        }
    }
}
