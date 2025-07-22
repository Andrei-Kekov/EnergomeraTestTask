using EnergomeraTestTask.Models;
using System.Globalization;
using System.Xml;
using System.Xml.XPath;

namespace EnergomeraTestTask.Data
{
    public class KmlReader
    {
        public string FieldsFilePath { get; set; } = "Data\\fields.kml";

        public string CentroidsFilePath { get; set; } = "Data\\centroids.kml";

        public List<Field> GetFields()
        {
            List<Field> fields = new List<Field>();

            if (string.IsNullOrEmpty(FieldsFilePath))
            {
                return fields;
            }

            var doc = new XPathDocument(FieldsFilePath);
            var navigator = doc.CreateNavigator();
            var resolver = new XmlNamespaceManager(navigator.NameTable);
            resolver.AddNamespace("kml", "http://www.opengis.net/kml/2.2");
            var placemarks = navigator.Select("//kml:Folder[kml:name='fields']/kml:Placemark", resolver);

            Field? field;
            long id;
            float size;
            string? polygonString;

            while (placemarks.MoveNext())
            {
                field = new Field();
                field.Name = placemarks.Current!.SelectSingleNode("./kml:name", resolver)?.Value;
                field.Id = long.TryParse(placemarks.Current!.SelectSingleNode(".//kml:SimpleData[@name='fid']", resolver)?.Value, out id) ? id : null;
                field.Size = float.TryParse(placemarks.Current!.SelectSingleNode(".//kml:SimpleData[@name='size']", resolver)?.Value, CultureInfo.InvariantCulture, out size) ? size : null;
                polygonString = placemarks.Current!.SelectSingleNode("./kml:Polygon//kml:coordinates", resolver)?.Value;
                field.Location = new Location();
                field.Location.Polygon = GetPointsFromString(polygonString);
                fields.Add(field);
            }

            LoadCenters(fields);

            return fields;
        }

        private List<Point> GetPointsFromString(string? polygonString)
        {
            var points = new List<Point>();

            if (string.IsNullOrEmpty(polygonString))
            {
                return points;
            }

            string[] coordPairs = polygonString.Split(separator: ' ', options: StringSplitOptions.RemoveEmptyEntries);

            foreach (string coordPair in coordPairs)
            {
                points.Add(new Point(coordPair));
            }

            return points;
        }

        private void LoadCenters(List<Field> fields)
        {
            var doc = new XPathDocument(CentroidsFilePath);
            var navigator = doc.CreateNavigator();
            var resolver = new XmlNamespaceManager(navigator.NameTable);
            resolver.AddNamespace("kml", "http://www.opengis.net/kml/2.2");
            var placemarks = navigator.Select("//kml:Folder[kml:name='centroids']/kml:Placemark", resolver);
            string? centerCoordPair;
            Field? field;
            long id;

            while (placemarks.MoveNext())
            {
                if (!long.TryParse(placemarks.Current!.SelectSingleNode(".//kml:SimpleData[@name='fid']", resolver)!.Value, out id))
                {
                    continue;
                }

                centerCoordPair = placemarks.Current!.SelectSingleNode("./kml:Point/kml:coordinates", resolver)?.Value;

                if (centerCoordPair is null)
                {
                    continue;
                }

                field = fields.Find(f => f.Id == id);

                if (field is null)
                {
                    continue;
                }

                field.Location!.Center = new Point(centerCoordPair);
            }
        }
    }
}
