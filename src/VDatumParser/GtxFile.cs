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
            float nullReturnValue = -88.8888f;

            bool latitudeInRange = (latitude >= LowerLeftLatitudeDecimalDegrees && latitude <= (LowerLeftLatitudeDecimalDegrees + DeltaLatitudeDecimalDegrees * (NumberOfRows-2)));
            bool longitudeInRange = (longitude >= LowerLeftLongitudeDecimalDegrees && longitude <= (LowerLeftLongitudeDecimalDegrees + DeltaLongitudeDecimalDegrees * (NumberOfColumns-2)));
            if (!(latitudeInRange && longitudeInRange))
                return nullReturnValue;

            int rowIndex = (int)(((latitude - LowerLeftLatitudeDecimalDegrees) / DeltaLatitudeDecimalDegrees) + 1);
            int colIndex = (int)(((longitude - LowerLeftLongitudeDecimalDegrees) / DeltaLongitudeDecimalDegrees) + 1);

            int singleArrayIndex = rowIndex * NumberOfColumns + colIndex;

            return Heights[singleArrayIndex];
        }
    }
}
