using CoordinateSharp;
using System.Globalization;

namespace EnergomeraTestTask.Models
{
    public class CoordinateParser
    {
        public Coordinate Parse(string s)
        {
            if (s is null)
            {
                throw new ArgumentNullException(nameof(s));
            }

            string[] xy = s.Split(',', StringSplitOptions.TrimEntries);

            if (xy.Length != 2)
            {
                throw new ArgumentException($"Invalid coordinates: \"{s}\". Must contain exactly 2 values seperated by a comma.");
            }

            return new Coordinate(double.Parse(xy[0], CultureInfo.InvariantCulture), double.Parse(xy[1], CultureInfo.InvariantCulture));
        }
    }
}
