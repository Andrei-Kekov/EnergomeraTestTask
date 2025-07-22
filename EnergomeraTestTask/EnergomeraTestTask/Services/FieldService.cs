using EnergomeraTestTask.Data;
using EnergomeraTestTask.Models;
using NetTopologySuite;
using Ntsg = NetTopologySuite.Geometries;

namespace EnergomeraTestTask.Services
{
    public class FieldService
    {
        private readonly List<Field> _fields;
        private readonly KmlReader _reader = new KmlReader();
        private readonly Ntsg.GeometryFactory _geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory();

        public FieldService(KmlReader reader)
        {
            _fields = reader.GetFields();
        }

        public List<Field> GetFields() => new List<Field>(_fields);

        public double? GetSize(long fieldId) => _fields.FirstOrDefault(f => f.Id == fieldId)?.Size;

        public double? DistanceFromCenter(long fieldId, Point point)
        {
            if (point is null)
            {
                return null;
            }

            Point? center = _fields.FirstOrDefault(f => f.Id == fieldId)?.Location?.Center;

            if (center is null)
            {
                return null;
            }

            double x = center.Longitude - point.Longitude;
            double y = center.Latitude - point.Latitude;

            return Math.Sqrt(x * x + y * y);
        }

        public Field? GetFieldByPoint(Point point)
        {
            if (point is null)
            {
                return null;
            }

            return _fields.FirstOrDefault(f => PointIsInField(point, f));
        }

        private bool PointIsInField(Point point, Field field)
        {
            if (field?.Location?.Polygon is null || field.Location.Polygon.Count < 3)
            {
                return false;
            }

            var coords = field.Location.Polygon.Select(p => new Ntsg.Coordinate(p.Longitude, p.Latitude)).ToArray();
            var polygon = _geometryFactory.CreatePolygon(coords);
            var ntsgPoint = _geometryFactory.CreatePoint(new Ntsg.Coordinate(point.Longitude, point.Latitude));
            return polygon.Contains(ntsgPoint);
        }
    }
}
