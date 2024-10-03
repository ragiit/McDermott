using Microsoft.AspNetCore.Mvc;
using McDermott.Persistence.Context;
using System.Linq;
using McDermott.Domain.Entities;

namespace McDermott.TelemedicianApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TelemedicianController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private bool IsValidToken(string token, string username)
        {
            // Implementasikan logika validasi token dan username di sini
            // Misalnya: cek di database atau sistem lain untuk memastikan token dan username valid
            ////Header
            //var xSignature = Request.Headers["X-signature"].FirstOrDefault();
            //var userKey = Request.Headers["user_key"].FirstOrDefault();

            //// Nilai X-signature dan user_key yang valid (bisa berasal dari database atau konfigurasi)
            //string validXSignature = "QXJnaSBQdXJ3YW50byBXYWh5dSBzYW5ha2kgRHdpIEx1Y2t5";
            //string validUserKey = "s3l3m84R<4!|V<3H!DLip4N";
            return true; // Asumsikan valid untuk sementara
        }

        // Constructor to inject ApplicationDbContext
        public TelemedicianController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{number}/{serviceId}")]
        public IActionResult GetDataPatient(string number, long serviceId)
        {
            

            // Query to fetch user based on number (SipNo or Legacy)
            var result = _context.Users.Where(
                    x => x.Legacy!.Equals(number) ||
                         x.NIP!.Equals(number) ||
                         x.Oracle!.Equals(number) ||
                         x.SAP!.Equals(number));


            // If no user found, return 404 Not Found
            if (user == null)
            {
                return NotFound(new
                {
                    metadata = new
                    {
                        code = 201,
                        message = "Data peserta tidak boleh kosong"
                    }
                });
            }

            // If user is found, return success response
            return Ok(new
            {
                metadata = new
                {
                    code = 200,
                    message = "Sukses"
                },
                data = new
                {
                    User = new
                    {
                        user.Name,
                        user.SipNo,
                        user.Legacy,
                        user.DateOfBirth,
                        user.Gender,
                        // Tambahkan field user lain yang relevan
                    },
                }
            });
        }
    }
}
