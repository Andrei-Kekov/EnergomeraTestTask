using NetTopologySuite.Geometries;
using System.Text.Json.Serialization;

namespace EnergomeraTestTask.Models
{
    public class Locations
    {
        public Coordinate? Center { get; set; }

        public Polygon? Polygon { get; set; }


        // Obsolete members, should ne removed
        [JsonIgnore]
        public OBSOLETE_Point? OBSOLETE_Center { get; set; }

        // Use DTO instead
        [JsonPropertyName("center")]
        public double[]? OBSOLETE_CenterCoordinates
        {
            get => OBSOLETE_Center?.Coordinates;

            set
            {
                if (value is null)
                {
                    OBSOLETE_Center = null;
                    return;
                }
                
                if (value.Length != 2)
                {
                    throw new ArgumentException("Center point must have exactly two coordinates");
                }

                OBSOLETE_Center = new OBSOLETE_Point(value[0], value[1]);
            }
        }

        [JsonIgnore]
        public List<OBSOLETE_Point>? OBSOLETE_Polygon { get; set; }

        // Use DTO instead
        [JsonPropertyName("polygon")]
        public double[][]? OBSOLETE_PolygonCoordinates
        {
            get => OBSOLETE_Polygon?.Select(p => p.Coordinates).ToArray();

            set => OBSOLETE_Polygon = value?.Select(pair => new OBSOLETE_Point(pair[0], pair[1])).ToList();
        }
    }
}
