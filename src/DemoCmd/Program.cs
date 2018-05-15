using System.IO;
using VDatumParser;
using System;

namespace DemoCmd
{
    class Program
    {
        static void Main(string[] args)
        {
            string gtxFilePath = @"C:\VDatum\ALFLgom02_8301\mllw.gtx";
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

        public static double getHeightFromCsv(this GtxFile gtxFile, string filePath, double latitude, double longitude)
        {
            StringReader stringReader = new StringReader(File.Open(filePath,FileMode.Open,FileAccess.Read,FileShare.ReadWrite).ToString());
            string[] separators = { "," };
            double nullReturnValue = -88.8888;

            //Read first line (column headers/latitude)
            string columnHeader = stringReader.ReadLine();

            //Split column header with "," delimiter; convert to double[]
            double[] columnHeaderValues = Array.ConvertAll(columnHeader.Split(separators,StringSplitOptions.RemoveEmptyEntries),Double.Parse);

            //Find passed latitude in column header values
            bool inRange = (latitude >= columnHeaderValues[0] && latitude <= columnHeaderValues[columnHeaderValues.Length]);
            if (!inRange)
                return nullReturnValue;
            
            int valueColumnIndex = (int)((latitude - columnHeaderValues[0]) / gtxFile.DeltaLatitudeDecimalDegrees);
            double nearestColumnValue = columnHeaderValues[0] + (valueColumnIndex * gtxFile.DeltaLatitudeDecimalDegrees);

            //Get second row
            string rowString = stringReader.ReadLine();
            //Split row with "," delimiter, convert to double[]
            double[] rowValues = Array.ConvertAll(rowString.Split(separators, StringSplitOptions.None), Double.Parse);
            //Get header value (longitude)
            double rowHeaderValue = rowValues[0];

            //Check if longitude value is potentially in the range
            inRange = (longitude >= rowHeaderValue);
            if (!inRange)
                return nullReturnValue;

            //calculate index value for matching row header value
            int valueRowIndex = (int)((longitude - rowValues[0]) / gtxFile.DeltaLongitudeDecimalDegrees);
            double nearestRowValue = rowValues[0] + (valueRowIndex * gtxFile.DeltaLongitudeDecimalDegrees);

            //get to row with value
            for (int i = 0; i < valueRowIndex; i++)
                rowString = stringReader.ReadLine();

            //Split row with "," delimiter, convert to double[]
            rowValues = Array.ConvertAll(rowString.Split(separators, StringSplitOptions.None), Double.Parse);
            //Get height value
            double heightValue = rowValues[valueColumnIndex + 1];

            return heightValue;
        }
    }
}
