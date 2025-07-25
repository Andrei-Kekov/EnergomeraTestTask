using EnergomeraTestTask.Models;

namespace EnergomeraTestTask.Dtos
{
    public class LocationsDto
    {
        public double[]? Center { get; set; }

        public double[][]? Polygon { get; set; }

        public LocationsDto() { }

        public LocationsDto(Locations locations)
        {
            if (locations is null)
            {
                throw new ArgumentNullException(nameof(locations));
            }

            if (locations.Center is not null)
            {
                Center = [locations.Center.Latitude.ToDouble(), locations.Center.Longitude.ToDouble()];
            }

            if (locations.Polygon is not null)
            {
                Polygon = locations.Polygon.Points.Select(p => new double[] { p.Latitude, p.Longitude }).ToArray();
            }
        }
    }
}
