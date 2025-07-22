using System.Text.Json.Serialization;

namespace EnergomeraTestTask.Models
{
    public class Location
    {
        [JsonIgnore]
        public Point? Center { get; set; }

        [JsonPropertyName("center")]
        public double[]? CenterCoordinates
        {
            get => Center?.Coordinates;

            set
            {
                if (value is null)
                {
                    Center = null;
                    return;
                }
                
                if (value.Length != 2)
                {
                    throw new ArgumentException("Center point must have exactly two coordinates");
                }

                Center = new Point(value[0], value[1]);
            }
        }

        [JsonIgnore]
        public List<Point>? Polygon { get; set; }

        [JsonPropertyName("polygon")]
        public double[][]? PolygonCoordinates
        {
            get => Polygon?.Select(p => p.Coordinates).ToArray();

            set => Polygon = value?.Select(pair => new Point(pair[0], pair[1])).ToList();
        }
    }
}
