using CoordinateSharp;
using EnergomeraTestTask.Dtos;
using EnergomeraTestTask.Models;
using EnergomeraTestTask.Services;
using Microsoft.AspNetCore.Mvc;

namespace EnergomeraTestTask.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FieldController : ControllerBase
    {
        private readonly FieldService _fieldService = new FieldService(new Data.KmlReader());

        private readonly ILogger<FieldController> _logger;

        public FieldController(ILogger<FieldController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("GetFields")]
        public IEnumerable<FieldDto> GetFields() => _fieldService.GetFields().Select(f => new FieldDto(f));

        [HttpPost]
        [Route("GetSize")]
        public ActionResult<double> GetSize(long fieldId)
        { 
            double? size = _fieldService.GetSize(fieldId);
            return size.HasValue ? size : NotFound();
        }

        [HttpPost]
        [Route("DistanceFromCenter")]
        public ActionResult<double> DistanceFromCenter(long fieldId, PointDto point)
        {
            var coord = new Coordinate(point.Latitude, point.Longitude);
            double? distance = _fieldService.MetersFromCenter(fieldId, coord);
            return distance.HasValue ? distance : NotFound();
        }

        [HttpPost]
        [Route("GetFieldByPoint")]
        public ActionResult<object> GetFieldByPoint(PointDto point)
        {
            var coord = new Coordinate(point.Latitude, point.Longitude);
            Field? field = _fieldService.GetFieldByPoint(coord);

            if (field is null)
            {
                return Ok(false);
            }

            return Ok(new { field.Id, field.Name });
        }
    }
}
