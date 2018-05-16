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

        /// <summary>
        /// Finds height in gtx file for given latitude and longitude
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        public float GetHeight(double latitude, double longitude)
        {
            float nullReturnValue = 1f;

            //Find coordinate bounds of gtx file
            double lowLatitudeBounds = RoundToDecimal(LowerLeftLatitudeDecimalDegrees, 3);
            double highLatitudeBounds = RoundToDecimal(LowerLeftLatitudeDecimalDegrees + DeltaLatitudeDecimalDegrees * (NumberOfRows - 2), 3);
            double lowLongitudeBounds = RoundToDecimal(LowerLeftLongitudeDecimalDegrees, 3);
            double highLongitudeBounds = RoundToDecimal(LowerLeftLongitudeDecimalDegrees + DeltaLongitudeDecimalDegrees * (NumberOfColumns - 2), 3);

            //Check if passed values are in range
            bool latitudeInRange = (latitude >= lowLatitudeBounds && latitude <= highLatitudeBounds);
            bool longitudeInRange = (longitude >= lowLongitudeBounds && longitude <= highLongitudeBounds);
            if (!(latitudeInRange && longitudeInRange))
                return nullReturnValue;

            //Check if passed values are multiples of delta
            bool latitudeMultipleOfDelta = (RoundToDecimal(latitude % DeltaLatitudeDecimalDegrees, 10) % DeltaLatitudeDecimalDegrees)== 0;
            bool longitudeMultipleOfDelta = (RoundToDecimal(longitude % DeltaLongitudeDecimalDegrees, 10) % DeltaLongitudeDecimalDegrees) == 0;

            //If values are multiples, find exact data
            if (latitudeMultipleOfDelta&&longitudeMultipleOfDelta)
            {
                //Find row and column indeces of represented data
                int rowIndex = (int)Math.Round(((latitude - LowerLeftLatitudeDecimalDegrees) / DeltaLatitudeDecimalDegrees) + 1);
                int colIndex = (int)Math.Round(((longitude - LowerLeftLongitudeDecimalDegrees) / DeltaLongitudeDecimalDegrees) + 1);

                //Find index of represented data in single array
                int singleArrayIndex = rowIndex * NumberOfColumns + colIndex;

                //Find height in single array
                float height = Heights[singleArrayIndex - 1];

                return height;
            }
            //If latitude is multiple, interpolate longitudes
            else if(latitudeMultipleOfDelta)
            {
                //Find nearest represented values in data
                double highLongitude = TruncateToDecimal(longitude + DeltaLongitudeDecimalDegrees,3);
                double lowLongitude = TruncateToDecimal(longitude, 3);

                //Find weights of eahc value based on proximity
                float highLongitudeWeight = (float)(((longitude % DeltaLongitudeDecimalDegrees) * 10) / DeltaLongitudeDecimalDegrees);
                float lowLongitudeWeight = (float)(((DeltaLongitudeDecimalDegrees - (longitude % DeltaLongitudeDecimalDegrees)) * 10) / DeltaLongitudeDecimalDegrees);

                //Find heights of represented values in data
                float highHeight = GetHeight(latitude, highLongitude);
                float lowHeight = GetHeight(latitude, lowLongitude);

                //Weught heights and find the average
                float weightedAverageHeight = (highLongitudeWeight * highHeight + lowLongitudeWeight * lowHeight) / (highLongitudeWeight + lowLongitudeWeight);

                return weightedAverageHeight;
            }
            //If longitude is multiple, interpolate latitude
            else if(longitudeMultipleOfDelta)
            {
                //Find nearest represented values in data
                double highLatitude = TruncateToDecimal(latitude + DeltaLatitudeDecimalDegrees, 3);
                double lowLatitude = TruncateToDecimal(latitude, 3);

                //Find weights of each value based on proximity
                float highLatitudeWeight = (float)(((latitude % DeltaLatitudeDecimalDegrees) * 10) / DeltaLatitudeDecimalDegrees);
                float lowLatitudeWeight = (float)(((DeltaLatitudeDecimalDegrees - (latitude % DeltaLatitudeDecimalDegrees)) * 10) / DeltaLatitudeDecimalDegrees);

                //Find heights of represented values in data
                float highLatitudeHeight = GetHeight(highLatitude, longitude);
                float lowLatitudeHeight = GetHeight(lowLatitude, longitude);

                //Weight heights and find the average
                float weightedAverageHeight = (highLatitudeWeight * highLatitudeHeight + lowLatitudeWeight * lowLatitudeHeight) / (highLatitudeWeight + lowLatitudeWeight);

                return weightedAverageHeight;
            }
            //If neither are multiples, interpolate both
            else
            {
                //Find nearest represented values in data
                double highLatitude = TruncateToDecimal(latitude + DeltaLatitudeDecimalDegrees, 3);
                double lowLatitude = TruncateToDecimal(latitude, 3);
                double highLongitude = TruncateToDecimal(longitude + DeltaLongitudeDecimalDegrees, 3);
                double lowLongitude = TruncateToDecimal(longitude, 3);

                //Find weights of each value based on proximity
                float highLatitudeWeight = (float)(((latitude % DeltaLatitudeDecimalDegrees) * 10) / DeltaLatitudeDecimalDegrees);
                float lowLatitudeWeight = (float)(((DeltaLatitudeDecimalDegrees - (latitude % DeltaLatitudeDecimalDegrees)) * 10) / DeltaLatitudeDecimalDegrees);
                float highLongitudeWeight = (float)(((longitude % DeltaLongitudeDecimalDegrees) * 10) / DeltaLongitudeDecimalDegrees);
                float lowLongitudeWeight = (float)(((DeltaLongitudeDecimalDegrees - (longitude % DeltaLongitudeDecimalDegrees)) * 10) / DeltaLongitudeDecimalDegrees);

                //Find heights of represented values in data
                float highLatitudeLowLongitudeHeight = GetHeight(highLatitude, lowLongitude);
                float lowLatitudeLowLongitudeHeight = GetHeight(lowLatitude, lowLongitude);

                float highLatitudeHighLongitudeHeight = GetHeight(highLatitude, highLongitude);
                float lowLatitudeHighLongitudeHeight = GetHeight(lowLatitude, highLongitude);

                //Weight heights and find the average
                float weightedAverageLowLongitudeHeight = (highLatitudeLowLongitudeHeight * highLatitudeWeight + lowLatitudeLowLongitudeHeight * lowLatitudeWeight) / (highLatitudeWeight + lowLatitudeWeight);
                float weightedAverageHighLongitudeHeight = (highLatitudeHighLongitudeHeight * highLatitudeWeight + lowLatitudeHighLongitudeHeight * highLatitudeWeight) / (highLatitudeWeight + lowLatitudeWeight);

                //Weight heights and find the average
                float weightedAverageHeight = (weightedAverageLowLongitudeHeight * lowLongitudeWeight + weightedAverageHighLongitudeHeight * highLongitudeWeight) / (lowLongitudeWeight + highLongitudeWeight);
                
                return weightedAverageHeight;
            }
        }

        /// <summary>
        /// Rounds number to expressed number of decimal places
        /// </summary>
        /// <param name="number"></param>
        /// <param name="places"></param>
        /// <returns></returns>
        private static double RoundToDecimal(double number, int places)
        {
            return Math.Round(number * Math.Pow(10, places)) / Math.Pow(10, places);
        }

        /// <summary>
        /// Truncates number to expressed numnber of decimal places
        /// </summary>
        /// <param name="number"></param>
        /// <param name="places"></param>
        /// <returns></returns>
        private static double TruncateToDecimal(double number, int places)
        {
            return Math.Truncate(number * Math.Pow(10, places)) / Math.Pow(10, places);
        }
    }
}
