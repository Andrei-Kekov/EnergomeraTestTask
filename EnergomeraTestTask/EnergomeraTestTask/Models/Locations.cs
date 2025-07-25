using CoordinateSharp;

namespace EnergomeraTestTask.Models
{
    public class Locations
    {
        public CoordinateSharp.Coordinate? Center { get; set; }

        public GeoFence? Polygon { get; set; }
    }
}
