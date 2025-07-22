using System.Globalization;
using System.Text.Json.Serialization;

namespace EnergomeraTestTask.Models
{
    public class Point
    {
        public double Latitude { get; set; }

        public double Longitude { get; set; }

        [JsonConstructor]
        public Point(double latitude = 0.0, double longitude = 0.0)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public Point(string coordinatePair)
        {
            if (coordinatePair is null)
            {
                throw new ArgumentNullException(nameof(coordinatePair));
            }

            string[] coordValues = coordinatePair.Split(separator: ',', options: StringSplitOptions.TrimEntries);

            if (coordValues.Length != 2)
            {
                throw new FormatException("The point must have exactly two coordinates");
            }

            Latitude = double.Parse(coordValues[0], CultureInfo.InvariantCulture);
            Longitude = double.Parse(coordValues[1], CultureInfo.InvariantCulture);
        }

        [JsonIgnore]
        public double[] Coordinates => [ Latitude, Longitude ];

        public override string ToString()
        {
            return $"{Latitude.ToString(CultureInfo.InvariantCulture)},{Longitude.ToString(CultureInfo.InvariantCulture)}";
        }
    }
}
