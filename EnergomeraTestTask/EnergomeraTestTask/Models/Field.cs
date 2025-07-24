namespace EnergomeraTestTask.Models
{
    public class Field
    {
        public long? Id { get; set; }

        public string? Name { get; set; }

        public float? Size { get; set; }

        // remove?
        public Locations? Locations { get; set; }
    }
}
