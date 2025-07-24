using System.Globalization;
using System.Text.Json.Serialization;

namespace EnergomeraTestTask.Models
{
    public class OBSOLETE_Point
    {
        public double Latitude { get; set; }

        public double Longitude { get; set; }

        [JsonConstructor]
        public OBSOLETE_Point(double latitude = 0.0, double longitude = 0.0)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        [JsonIgnore]
        public double[] Coordinates => [ Latitude, Longitude ];
    }
}
