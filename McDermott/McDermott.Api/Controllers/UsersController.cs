using McDermott.Persistence.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace McDermott.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(ApplicationDbContext _context) : ODataController
    {
        [EnableQuery]
        [HttpGet]  // Tambahkan atribut ini
        public IActionResult Get()
        {
            return Ok(_context.Users);
        }
    }
}