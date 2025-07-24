using EnergomeraTestTask.Models;

namespace EnergomeraTestTask.Dtos
{
    public class FieldDto
    {
        public FieldDto() { }

        public FieldDto(Field field)
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            Id = field.Id;
            Name = field.Name;
            Size= field.Size;

            if (field.Locations is null)
            {
                return;
            }

            Locations = new LocationsDto();

            if (field.Locations.Center is not null)
            {
                Locations.Center = [field.Locations.Center.X, field.Locations.Center.Y];
            }

            if (field.Locations.Polygon is not null)
            {
                Locations.Polygon = field.Locations.Polygon.Coordinates.Select(c => new double[] { c.X, c.Y }).ToArray();
            }
        }

        public long? Id { get; set; }

        public string? Name { get; set; }

        public float? Size { get; set; }

        public LocationsDto Locations { get; set; } = new LocationsDto();
    }
}