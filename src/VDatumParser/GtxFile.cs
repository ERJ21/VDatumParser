using System;
using System.Collections.ObjectModel;

namespace VDatumParser
{
    public class GtxFile
    {
        public GtxFile(string filePath, double lowerLeftLatitudeDecimalDegrees, double lowerLeftLongitudeDecimalDegrees, double deltaLatitudeDecimalDegrees, double deltaLongitudeDecimalDegrees, int numberOfRows, int numberOfColumns, ReadOnlyCollection<float> heights)
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

        public ReadOnlyCollection<float> Heights { get; }

        /// <summary>
        /// Finds height in gtx file for given latitude and longitude
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        public double GetHeight(double lat, double lon)
        {
            float nullReturnValue = 1f;

            //Change significant figure of variables for accuracy
            double lowerLatitude = Math.Round(LowerLeftLatitudeDecimalDegrees / DeltaLatitudeDecimalDegrees);
            double lowerLongitude = Math.Round(LowerLeftLongitudeDecimalDegrees / DeltaLongitudeDecimalDegrees);
            double latitude = lat * 1000;
            double longitude = lon * 1000;

            //Find higher coordinate bounds of gtx file
            double higherLatitude = lowerLatitude + (NumberOfRows - 1);
            double higherLongitude = lowerLongitude + (NumberOfColumns - 1);

            //Check if passed values are in range
            bool latitudeInRange = (latitude >= lowerLatitude && latitude <= higherLatitude);
            bool longitudeInRange = (longitude >= lowerLongitude && longitude <= higherLongitude);
            if (!(latitudeInRange && longitudeInRange))
                return nullReturnValue;

            //Check if passed values are multiples of delta
            bool latitudeMultipleOfDelta = (latitude % 1) == 0;
            bool longitudeMultipleOfDelta = (longitude % 1) == 0;

            //If values are multiples, find exact data
            if (latitudeMultipleOfDelta && longitudeMultipleOfDelta)
            {
                //Find row and column indeces of represented data
                int rowIndex = (int)(latitude - lowerLatitude);
                int colIndex = (int)(longitude - lowerLongitude);

                //Find index of represented data in single array
                int singleArrayIndex = rowIndex * NumberOfColumns + colIndex;

                //Find and return height in single array
                decimal temp = (decimal)Heights[singleArrayIndex];
                return Convert.ToDouble(temp);
            }
            //If latitude is multiple, interpolate longitudes
            else if (latitudeMultipleOfDelta)
            {
                //Find nearest represented values in data
                double highLongitude = Math.Truncate(longitude + 1);
                double lowLongitude = Math.Truncate(longitude);

                //Find weights of eahc value based on proximity
                double highLongitudeWeight = Math.Round((longitude % 1) * 10);
                double lowLongitudeWeight = 10 - highLongitudeWeight;

                //Find heights of represented values in data
                double highHeight = 10000 * GetHeight(latitude / 1000, highLongitude / 1000);
                double lowHeight = 10000 * GetHeight(latitude / 1000, lowLongitude / 1000);

                //Weught heights and find the average
                double weightedAverageHeight = (highLongitudeWeight * highHeight + lowLongitudeWeight * lowHeight) / (highLongitudeWeight + lowLongitudeWeight);

                return weightedAverageHeight / 10000;
            }
            //If longitude is multiple, interpolate latitude
            else if (longitudeMultipleOfDelta)
            {
                //Find nearest represented values in data
                double highLatitude = Math.Truncate(latitude + 1);
                double lowLatitude = Math.Truncate(latitude);

                //Find weights of each value based on proximity
                double highLatitudeWeight = Math.Round((latitude % 1) * 10);
                double lowLatitudeWeight = 10 - highLatitudeWeight;

                //Find heights of represented values in data
                double highLatitudeHeight = 10000 * GetHeight(highLatitude / 1000, longitude / 1000);
                double lowLatitudeHeight = 10000 * GetHeight(lowLatitude / 1000, longitude / 1000);

                //Weight heights and find the average
                double weightedAverageHeight = (highLatitudeWeight * highLatitudeHeight + lowLatitudeWeight * lowLatitudeHeight) / (highLatitudeWeight + lowLatitudeWeight);

                return weightedAverageHeight / 10000;
            }
            //If neither are multiples, interpolate both
            else
            {
                //Find nearest represented values in data
                double highLatitude = Math.Truncate(latitude + 1);
                double lowLatitude = Math.Truncate(latitude);
                double highLongitude = Math.Truncate(longitude + 1);
                double lowLongitude = Math.Truncate(longitude);

                //Find weights of each value based on proximity
                double highLatitudeWeight = Math.Round((latitude % 1) * 10);
                double lowLatitudeWeight = 10 - highLatitudeWeight;

                double highLongitudeWeight = Math.Round((longitude % 1) * 10);
                double lowLongitudeWeight = 10 - highLongitudeWeight;

                //Find heights of represented values in data
                double highLatLowLongHeight = 10000 * GetHeight(highLatitude / 1000, lowLongitude / 1000);
                double lowLatLowLongHeight = 10000 * GetHeight(lowLatitude / 1000, lowLongitude / 1000);

                double highLatHighLongHeight = 10000 * GetHeight(highLatitude / 1000, highLongitude / 1000);
                double lowLatHighLongHeight = 10000 * GetHeight(lowLatitude / 1000, highLongitude / 1000);

                //Weight heights and find the average
                double weightedAvgLowLongHeight = (highLatLowLongHeight * highLatitudeWeight + lowLatLowLongHeight * lowLatitudeWeight) / 10;
                double weightedAvgHighLongHeight = (highLatHighLongHeight * highLatitudeWeight + lowLatHighLongHeight * highLatitudeWeight) / 10;

                //Weight heights and find the average
                double weightedAverageHeight = (weightedAvgLowLongHeight * lowLongitudeWeight + weightedAvgHighLongHeight * highLongitudeWeight) / (lowLongitudeWeight + highLongitudeWeight);

                return weightedAverageHeight / 10000;
            }
        }
    }
}