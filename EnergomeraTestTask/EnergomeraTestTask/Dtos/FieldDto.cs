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

            if (field.Locations is not null)
            {
                Locations = new LocationsDto(field.Locations);
            }
        }

        public long? Id { get; set; }

        public string? Name { get; set; }

        public float? Size { get; set; }

        public LocationsDto Locations { get; set; } = new LocationsDto();
    }
}