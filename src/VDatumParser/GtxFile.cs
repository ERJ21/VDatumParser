using System;

namespace VDatumParser
{
    public class GtxFile
    {
        public GtxFile(string filePath, double lowerLeftLatitudeDecimalDegrees, double lowerLeftLongitudeDecimalDegrees, double deltaLatitudeDecimalDegrees, double deltaLongitudeDecimalDegrees, int numberOfRows, int numberOfColumns, float[] heights)
        {
            FilePath = filePath;
            LowerLeftLatitudeDecimalDegrees = lowerLeftLatitudeDecimalDegrees;
            LowerLeftLongitudeDecimalDegrees = lowerLeftLongitudeDecimalDegrees;
            DeltaLatitudeDecimalDegrees = deltaLatitudeDecimalDegrees;
            DeltaLongitudeDecimalDegrees = deltaLongitudeDecimalDegrees;
            NumberOfRows = numberOfRows;
            NumberOfColumns = numberOfColumns;
            Heights = heights;
        }

        public string FilePath { get; }

        public double LowerLeftLatitudeDecimalDegrees { get; }

        public double LowerLeftLongitudeDecimalDegrees { get; }

        public double DeltaLatitudeDecimalDegrees { get; }

        public double DeltaLongitudeDecimalDegrees { get; }

        public int NumberOfRows { get; }

        public int NumberOfColumns { get; }

        public float[] Heights { get; }

        public float GetHeight(double latitude, double longitude)
        {
            float nullReturnValue = 1f;

            double lowLatitudeBounds = Round(LowerLeftLatitudeDecimalDegrees,3);
            double highLatitudeBounds = Round(LowerLeftLatitudeDecimalDegrees + DeltaLatitudeDecimalDegrees * (NumberOfRows - 1),3);
            double lowLongitudeBounds = Round(LowerLeftLongitudeDecimalDegrees,3);
            double highLongitudeBounds = Round(LowerLeftLongitudeDecimalDegrees + DeltaLongitudeDecimalDegrees * (NumberOfColumns - 1),3);

            bool latitudeInRange = (latitude >= lowLatitudeBounds && latitude <= highLatitudeBounds);
            bool longitudeInRange = (longitude >= lowLongitudeBounds && longitude <= highLongitudeBounds);
            if (!(latitudeInRange && longitudeInRange))
                return nullReturnValue;

            int rowIndex = (int)Math.Round(((latitude - LowerLeftLatitudeDecimalDegrees) / DeltaLatitudeDecimalDegrees));
            int colIndex = (int)Math.Round(((longitude - LowerLeftLongitudeDecimalDegrees) / DeltaLongitudeDecimalDegrees) + 1);

            int singleArrayIndex = rowIndex * NumberOfColumns + colIndex;
            float height = Heights[singleArrayIndex - 1];

            return height;
        }

        private static double Round(double number, int places)
        {
            return Math.Round(number * Math.Pow(10, places)) / Math.Pow(10, places);
        }
    }
}
