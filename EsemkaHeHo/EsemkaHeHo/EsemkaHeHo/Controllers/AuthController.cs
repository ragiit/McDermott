using EsemkaHeHo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace EsemkaHeHo.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public AuthController() { }

        public class LoginModel
        {
            public string Email { get; set; } = null!;

            public string Password { get; set; } = null!;
        }

        [HttpPost("login")]
        public IActionResult Auth(LoginModel req)
        {
            return Ok(new
            {
                Token = "ey12345",
                ExpiredDate = DateTime.Now,  
            });
        }

        public class RegisterModel
        { 
            public Guid? DepartmentId { get; set; }

            public string FullName { get; set; } = null!;

            public string Email { get; set; } = null!;

            public string Password { get; set; } = null!;

            [JsonConverter(typeof(JsonStringEnumConverter))]
            public EnumRole Role { get; set; }

            public string? PhoneNumber { get; set; }

            public string? Address { get; set; }

            public DateTime? DateOfBirth { get; set; } 

            public decimal? Salary { get; set; }  
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterModel req)
        {
            var user = new Models.User();
            return Created("Ok", user);
        }
    }
}
