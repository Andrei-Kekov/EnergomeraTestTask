using CoordinateSharp;
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

        private readonly CoordinateParser _coordParser = new CoordinateParser();

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
                field.Locations = new Locations();
                field.Locations.Polygon = ParsePolygon(polygonString);
                fields.Add(field);
            }

            LoadCenters(fields);

            return fields;
        }

        private GeoFence? ParsePolygon(string? polygonString)
        {
            if (string.IsNullOrEmpty (polygonString))
            {
                return null;
            }

            var coordStrings = polygonString.Split(separator: ' ', options: StringSplitOptions.RemoveEmptyEntries);

            if (coordStrings.Length < 3)
            {
                throw new ArgumentException($"Invalid polygon: '{polygonString}'. A polygon must have at least 3 vertices.");
            }

            var vertices = coordStrings.Select(s => _coordParser.Parse(s)).ToList();
            return new GeoFence(vertices);
        }

        private void LoadCenters(List<Field> fields)
        {
            var doc = new XPathDocument(CentroidsFilePath);
            var navigator = doc.CreateNavigator();
            var resolver = new XmlNamespaceManager(navigator.NameTable);
            resolver.AddNamespace("kml", "http://www.opengis.net/kml/2.2");
            var placemarks = navigator.Select("//kml:Folder[kml:name='centroids']/kml:Placemark", resolver);
            string? coordString;
            Field? field;
            long id;

            while (placemarks.MoveNext())
            {
                if (!long.TryParse(placemarks.Current!.SelectSingleNode(".//kml:SimpleData[@name='fid']", resolver)!.Value, out id))
                {
                    continue;
                }

                coordString = placemarks.Current!.SelectSingleNode("./kml:Point/kml:coordinates", resolver)?.Value;

                if (coordString is null)
                {
                    continue;
                }

                field = fields.Find(f => f.Id == id);

                if (field is null)
                {
                    continue;
                }

                field.Locations!.Center = _coordParser.Parse(coordString);
            }
        }
    }
}
