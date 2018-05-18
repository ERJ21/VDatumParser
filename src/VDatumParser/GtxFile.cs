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
        public decimal GetHeight(decimal latitude, decimal longitude)
        {
            decimal nullReturnValue = 1;

            //convert doubles/floats to decimal for accuracy
            decimal lowerLatitude = (decimal)LowerLeftLatitudeDecimalDegrees;
            decimal lowerLongitude = (decimal)LowerLeftLongitudeDecimalDegrees;
            decimal latitudeDelta = (decimal)DeltaLatitudeDecimalDegrees;
            decimal longitudeDelta = (decimal)DeltaLongitudeDecimalDegrees;

            //Find higher coordinate bounds of gtx file
            decimal higherLatitude = lowerLatitude + latitudeDelta * ((decimal)NumberOfRows - 1);
            decimal higherLongitude = lowerLongitude + longitudeDelta * ((decimal)NumberOfColumns - 1);

            //Check if passed values are in range
            bool latitudeInRange = (latitude >= lowerLatitude && latitude <= higherLatitude);
            bool longitudeInRange = (longitude >= lowerLongitude && longitude <= higherLongitude);
            if (!(latitudeInRange && longitudeInRange))
                return nullReturnValue;

            //Check if passed values are multiples of delta
            bool latitudeMultipleOfDelta = (latitude % latitudeDelta) == 0;
            bool longitudeMultipleOfDelta = (longitude % longitudeDelta) == 0;

            //If values are multiples, find exact data
            if (latitudeMultipleOfDelta && longitudeMultipleOfDelta)
            {
                //Find row and column indeces of represented data
                int rowIndex = (int)((latitude - lowerLatitude) / latitudeDelta);
                int colIndex = (int)((longitude - lowerLongitude) / longitudeDelta);

                //Find index of represented data in single array
                int singleArrayIndex = rowIndex * NumberOfColumns + colIndex;

                //Find and return height in single array
                float height = Heights[singleArrayIndex];
                return (decimal)height;
            }
            //If latitude is multiple, interpolate longitudes
            else if (latitudeMultipleOfDelta)
            {
                //Find nearest represented values in data
                decimal highLongitude = TruncateToDecimal(longitude + longitudeDelta, 3);
                decimal lowLongitude = TruncateToDecimal(longitude, 3);

                //Find weights of eahc value based on proximity
                decimal highLongitudeWeight = ((longitude % longitudeDelta) * 10) / longitudeDelta;
                decimal lowLongitudeWeight = ((longitudeDelta - (longitude % longitudeDelta)) * 10) / longitudeDelta;

                //Find heights of represented values in data
                decimal highHeight = GetHeight(latitude, highLongitude);
                decimal lowHeight = GetHeight(latitude, lowLongitude);

                //Weught heights and find the average
                decimal weightedAverageHeight = (highLongitudeWeight * highHeight + lowLongitudeWeight * lowHeight) / (highLongitudeWeight + lowLongitudeWeight);

                return weightedAverageHeight;
            }
            //If longitude is multiple, interpolate latitude
            else if (longitudeMultipleOfDelta)
            {
                //Find nearest represented values in data
                decimal highLatitude = TruncateToDecimal(latitude + latitudeDelta, 3);
                decimal lowLatitude = TruncateToDecimal(latitude, 3);

                //Find weights of each value based on proximity
                decimal highLatitudeWeight = ((latitude % latitudeDelta) * 10) / latitudeDelta;
                decimal lowLatitudeWeight = ((latitudeDelta - (latitude % latitudeDelta)) * 10) / latitudeDelta;

                //Find heights of represented values in data
                decimal highLatitudeHeight = GetHeight(highLatitude, longitude);
                decimal lowLatitudeHeight = GetHeight(lowLatitude, longitude);

                //Weight heights and find the average
                decimal weightedAverageHeight = (highLatitudeWeight * highLatitudeHeight + lowLatitudeWeight * lowLatitudeHeight) / (highLatitudeWeight + lowLatitudeWeight);

                return weightedAverageHeight;
            }
            //If neither are multiples, interpolate both
            else
            {
                //Find nearest represented values in data
                decimal highLatitude = TruncateToDecimal(latitude + latitudeDelta, 3);
                decimal lowLatitude = TruncateToDecimal(latitude, 3);
                decimal highLongitude = TruncateToDecimal(longitude + longitudeDelta, 3);
                decimal lowLongitude = TruncateToDecimal(longitude, 3);

                //Find weights of each value based on proximity
                decimal highLatitudeWeight = ((latitude % latitudeDelta) * 10) / latitudeDelta;
                decimal lowLatitudeWeight = ((latitudeDelta - (latitude % latitudeDelta)) * 10) / latitudeDelta;

                decimal highLongitudeWeight = ((longitude % longitudeDelta) * 10) / longitudeDelta;
                decimal lowLongitudeWeight = ((longitudeDelta - (longitude % longitudeDelta)) * 10) / longitudeDelta;

                //Find heights of represented values in data
                decimal highLatLowLongHeight = GetHeight(highLatitude, lowLongitude);
                decimal lowLatLowLongHeight = GetHeight(lowLatitude, lowLongitude);

                decimal highLatHighLongHeight = GetHeight(highLatitude, highLongitude);
                decimal lowLatHighLongHeight = GetHeight(lowLatitude, highLongitude);

                //Weight heights and find the average
                decimal weightedAvgLowLongHeight = (highLatLowLongHeight * highLatitudeWeight + lowLatLowLongHeight * lowLatitudeWeight) / (highLatitudeWeight + lowLatitudeWeight);
                decimal weightedAvgHighLongHeight = (highLatHighLongHeight * highLatitudeWeight + lowLatHighLongHeight * highLatitudeWeight) / (highLatitudeWeight + lowLatitudeWeight);

                //Weight heights and find the average
                decimal weightedAverageHeight = (weightedAvgLowLongHeight * lowLongitudeWeight + weightedAvgHighLongHeight * highLongitudeWeight) / (lowLongitudeWeight + highLongitudeWeight);

                return weightedAverageHeight;
            }
        }

        /// <summary>
        /// Truncates decimal to expressed number of decimal places
        /// </summary>
        /// <param name="number"></param>
        /// <param name="places"></param>
        /// <returns></returns>
        private static decimal TruncateToDecimal(decimal number, int places)
        {
            return (decimal)(Math.Truncate((double)number * Math.Pow(10, places)) / Math.Pow(10, places));
        }
    }
}