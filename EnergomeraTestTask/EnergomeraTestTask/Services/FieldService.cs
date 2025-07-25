using CoordinateSharp;
using EnergomeraTestTask.Data;
using EnergomeraTestTask.Models;

namespace EnergomeraTestTask.Services
{
    public class FieldService
    {
        private readonly List<Field> _fields;

        public FieldService(KmlReader reader)
        {
            _fields = reader.GetFields();
        }

        public List<Field> GetFields() => new List<Field>(_fields);

        public double? GetSize(long fieldId) => _fields.FirstOrDefault(f => f.Id == fieldId)?.Size;

        public double? MetersFromCenter(long fieldId, Coordinate coordinate)
        {
            if (coordinate is null)
            {
                return null;
            }

            Coordinate? center = _fields.FirstOrDefault(f => f.Id == fieldId)?.Locations?.Center;

            if (center is null)
            {
                return null;
            }

            return center.Get_Distance_From_Coordinate(coordinate).Meters;
        }

        public Field? GetFieldByPoint(Coordinate coordinate)
        {
            if (coordinate is null)
            {
                return null;
            }

            return _fields.FirstOrDefault(f => f.Locations?.Polygon?.IsPointInPolygon(coordinate) == true);
        }
    }
}
