using EnergomeraTestTask.Data;
using EnergomeraTestTask.Models;
using NetTopologySuite;
using Ntsg = NetTopologySuite.Geometries;

namespace EnergomeraTestTask.Services
{
    public class FieldService
    {
        private readonly List<Field> _fields;
        private readonly Ntsg.GeometryFactory _geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory();

        public FieldService(KmlReader reader)
        {
            _fields = reader.GetFields();
        }

        public List<Field> GetFields() => new List<Field>(_fields);

        public double? GetSize(long fieldId) => _fields.FirstOrDefault(f => f.Id == fieldId)?.Size;

        public double? DistanceFromCenter(long fieldId, OBSOLETE_Point point)
        {
            if (point is null)
            {
                return null;
            }

            OBSOLETE_Point? center = _fields.FirstOrDefault(f => f.Id == fieldId)?.Locations?.OBSOLETE_Center;

            if (center is null)
            {
                return null;
            }

            double x = center.Longitude - point.Longitude;
            double y = center.Latitude - point.Latitude;

            return Math.Sqrt(x * x + y * y);
        }

        public Field? GetFieldByPoint(OBSOLETE_Point point)
        {
            if (point is null)
            {
                return null;
            }

            return _fields.FirstOrDefault(f => PointIsInField(point, f));
        }

        private bool PointIsInField(OBSOLETE_Point point, Field field)
        {
            if (field?.Locations?.OBSOLETE_Polygon is null || field.Locations.OBSOLETE_Polygon.Count < 3)
            {
                return false;
            }

            var coords = field.Locations.OBSOLETE_Polygon.Select(p => new Ntsg.Coordinate(p.Longitude, p.Latitude)).ToArray();
            var polygon = _geometryFactory.CreatePolygon(coords);
            var ntsgPoint = _geometryFactory.CreatePoint(new Ntsg.Coordinate(point.Longitude, point.Latitude));
            return polygon.Contains(ntsgPoint);
        }
    }
}
