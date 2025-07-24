using System.Text.Json.Serialization;

namespace EnergomeraTestTask.Dtos
{
    public class LocationsDto
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public double[]? Center { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public double[][]? Polygon { get; set; }
    }
}
