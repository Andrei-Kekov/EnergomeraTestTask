using EnergomeraTestTask.Models;
using EnergomeraTestTask.Services;
using Microsoft.AspNetCore.Mvc;
using NetTopologySuite.IO;

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
        public IEnumerable<Field> GetFields() => _fieldService.GetFields();

        [HttpPost]
        [Route("GetSize")]
        public ActionResult<double> GetSize(long fieldId)
        { 
            double? size = _fieldService.GetSize(fieldId);
            return size.HasValue ? size : NotFound();
        }

        [HttpPost]
        [Route("DistanceFromCenter")]
        public ActionResult<double> DistanceFromCenter(long fieldId, Point point)
        {
            double? distance = _fieldService.DistanceFromCenter(fieldId, point);
            return distance.HasValue ? distance : NotFound();
        }

        [HttpPost]
        [Route("GetFieldByPoint")]
        public ActionResult<object> GetFieldByPoint(Point point)
        {
            Field? field = _fieldService.GetFieldByPoint(point);

            if (field is null)
            {
                return Ok(false);
            }

            return Ok(new { field.Id, field.Name });
        }
    }
}
