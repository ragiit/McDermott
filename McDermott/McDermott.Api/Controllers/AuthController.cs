using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace McDermott.Api.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController() : ControllerBase
    {
        [HttpGet]
        public IActionResult GetToken([FromHeader(Name = "x-username")] string username, [FromHeader(Name = "x-password")] string password)
        {
            // Validasi username dan password
            if (IsValidUser(username, password))
            {
                var token = GenerateToken(username);

                var response = new
                {
                    response = new
                    {
                        token = token
                    },
                    metadata = new
                    {
                        message = "Ok",
                        code = 200
                    }
                };

                return Ok(response);
            }
            else
            {
                var errorResponse = new
                {
                    metadata = new
                    {
                        message = "Unauthorized",
                        code = 401
                    }
                };

                return Unauthorized(errorResponse);
            }
        }

        private bool IsValidUser(string username, string password)
        {
            return true;
            // Implementasikan validasi user sesuai dengan kebutuhan
            // Contoh: memeriksa username dan password dari database
            return username == "your_username" && password == "your_password";
        }

        private string GenerateToken(string username)
        {
            // Implementasikan logika untuk membuat token
            // Contoh: menggunakan JWT atau metode lainnya
            return "1231242353534645645"; // Ini hanya contoh, ganti dengan logika token yang sesungguhnya
        }
    }
}