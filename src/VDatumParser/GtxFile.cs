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


            double lowLatitudeBounds = RoundToDecimal(LowerLeftLatitudeDecimalDegrees, 3);
            double highLatitudeBounds = RoundToDecimal(LowerLeftLatitudeDecimalDegrees + DeltaLatitudeDecimalDegrees * (NumberOfRows - 2), 3);
            double lowLongitudeBounds = RoundToDecimal(LowerLeftLongitudeDecimalDegrees, 3);
            double highLongitudeBounds = RoundToDecimal(LowerLeftLongitudeDecimalDegrees + DeltaLongitudeDecimalDegrees * (NumberOfColumns - 2), 3);

            bool latitudeInRange = (latitude >= lowLatitudeBounds && latitude <= highLatitudeBounds);
            bool longitudeInRange = (longitude >= lowLongitudeBounds && longitude <= highLongitudeBounds);
            if (!(latitudeInRange && longitudeInRange))
                return nullReturnValue;

            double c;
            bool latitudeMultipleOfDelta = (c = RoundToDecimal(latitude % DeltaLatitudeDecimalDegrees, 10) % DeltaLatitudeDecimalDegrees)== 0;
            bool longitudeMultipleOfDelta = (c = RoundToDecimal(longitude % DeltaLongitudeDecimalDegrees, 10) % DeltaLongitudeDecimalDegrees) == 0;

            if (latitudeMultipleOfDelta&&longitudeMultipleOfDelta)
            {
                int rowIndex = (int)Math.Round(((latitude - LowerLeftLatitudeDecimalDegrees) / DeltaLatitudeDecimalDegrees) + 1);
                int colIndex = (int)Math.Round(((longitude - LowerLeftLongitudeDecimalDegrees) / DeltaLongitudeDecimalDegrees) + 1);

                int singleArrayIndex = rowIndex * NumberOfColumns + colIndex;
                float height = Heights[singleArrayIndex - 1];

                return height;
            }
            else if(latitudeMultipleOfDelta)
            {
                double highLongitude = TruncateToDecimal(longitude + DeltaLongitudeDecimalDegrees,3);
                double lowLongitude = TruncateToDecimal(longitude, 3);

                float highLongitudeWeight = (float)(((longitude % DeltaLongitudeDecimalDegrees) * 10) / DeltaLongitudeDecimalDegrees);
                float lowLongitudeWeight = (float)(((DeltaLongitudeDecimalDegrees - (longitude % DeltaLongitudeDecimalDegrees)) * 10) / DeltaLongitudeDecimalDegrees);

                float highHeight = GetHeight(latitude, highLongitude);
                float lowHeight = GetHeight(latitude, lowLongitude);

                float weightedAverageHeight = (highLongitudeWeight * highHeight + lowLongitudeWeight * lowHeight) / (highLongitudeWeight + lowLongitudeWeight);

                return weightedAverageHeight;
            }
            else if(longitudeMultipleOfDelta)
            {
                double highLatitude = TruncateToDecimal(latitude + DeltaLatitudeDecimalDegrees, 3);
                double lowLatitude = TruncateToDecimal(latitude, 3);

                float highLatitudeWeight = (float)(((latitude % DeltaLatitudeDecimalDegrees) * 10) / DeltaLatitudeDecimalDegrees);
                float lowLatitudeWeight = (float)(((DeltaLatitudeDecimalDegrees - (latitude % DeltaLatitudeDecimalDegrees)) * 10) / DeltaLatitudeDecimalDegrees);

                float highHeight = GetHeight(highLatitude, longitude);
                float lowHeight = GetHeight(lowLatitude, longitude);

                float weightedAverageHeight = (highLatitudeWeight * highHeight + lowLatitudeWeight * lowHeight) / (highLatitudeWeight + lowLatitudeWeight);

                return weightedAverageHeight;
            }
            else
            {
                double highLatitude = TruncateToDecimal(latitude + DeltaLatitudeDecimalDegrees, 3);
                double lowLatitude = TruncateToDecimal(latitude, 3);
                double highLongitude = TruncateToDecimal(longitude + DeltaLongitudeDecimalDegrees, 3);
                double lowLongitude = TruncateToDecimal(longitude, 3);

                float highLatitudeWeight = (float)(((latitude % DeltaLatitudeDecimalDegrees) * 10) / DeltaLatitudeDecimalDegrees);
                float lowLatitudeWeight = (float)(((DeltaLatitudeDecimalDegrees - (latitude % DeltaLatitudeDecimalDegrees)) * 10) / DeltaLatitudeDecimalDegrees);
                float highLongitudeWeight = (float)(((longitude % DeltaLongitudeDecimalDegrees) * 10) / DeltaLongitudeDecimalDegrees);
                float lowLongitudeWeight = (float)(((DeltaLongitudeDecimalDegrees - (longitude % DeltaLongitudeDecimalDegrees)) * 10) / DeltaLongitudeDecimalDegrees);

                float highLatitudeLowLongitudeHeight = GetHeight(highLatitude, lowLongitude);
                float lowLatitudeLowLongitudeHeight = GetHeight(lowLatitude, lowLongitude);

                float highLatitudeHighLongitudeHeight = GetHeight(highLatitude, highLongitude);
                float lowLatitudeHighLongitudeHeight = GetHeight(lowLatitude, highLongitude);

                float weightedAverageLowLongitudeHeight = (highLatitudeLowLongitudeHeight * highLatitudeWeight + lowLatitudeLowLongitudeHeight * lowLatitudeWeight) / (highLatitudeWeight + lowLatitudeWeight);
                float weightedAverageHighLongitudeHeight = (highLatitudeHighLongitudeHeight * highLatitudeWeight + lowLatitudeHighLongitudeHeight * highLatitudeWeight) / (highLatitudeWeight + lowLatitudeWeight);

                float weightedAverageHeight = (weightedAverageLowLongitudeHeight * lowLongitudeWeight + weightedAverageHighLongitudeHeight * highLongitudeWeight) / (lowLongitudeWeight + highLongitudeWeight);
                
                return weightedAverageHeight;
            }
        }

        private static double RoundToDecimal(double number, int places)
        {
            return Math.Round(number * Math.Pow(10, places)) / Math.Pow(10, places);
        }

        private static double TruncateToDecimal(double number, int places)
        {
            return Math.Truncate(number * Math.Pow(10, places)) / Math.Pow(10, places);
        }
    }
}
