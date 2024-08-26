using App.Metrics;
using App.Metrics.Counter;
using McDermott.Persistence.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace McDermott.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(ApplicationDbContext _context, IMetrics _metrics) : ODataController
    {
        private static readonly CounterOptions MyCounterOptions = new()
        {
            Name = "My Counter",
            MeasurementUnit = Unit.Calls
        };

        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
        [EnableQuery]
        [HttpGet]  // Tambahkan atribut ini
        public IActionResult Get()
        {
            _metrics.Measure.Counter.Increment(MyCounterOptions);

            return Ok(_context.Users);
        }
    }
}