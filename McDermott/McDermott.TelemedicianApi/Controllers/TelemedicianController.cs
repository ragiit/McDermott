using Microsoft.AspNetCore.Mvc;
using McDermott.Persistence.Context;
using McDermott.Domain.Entities; // Import the ViewModel namespace
using System.Linq;
using System.Collections.Generic;
using MediatR;
using static McDermott.Application.Features.Commands.Config.UserCommand;
using McDermott.TelemedicianApi.ViewModel;
using McDermott.Application.Dtos.Config;

namespace McDermott.TelemedicianApi.Controllers
{
    [NonController]
    [Route("api/[controller]")]
    public class TelemedicianController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private readonly IMediator _mediator;

        public TelemedicianController(ApplicationDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        [HttpGet("{number}/{serviceId}")]
        public async Task<IActionResult> GetDataPatient(string number, long serviceId)
        {
            // Query untuk mendapatkan user berdasarkan identifier (Legacy, NIP, Oracle, SAP)
            var user = await _mediator.Send(new GetDataUserForKioskQuery(number));
            var doctorIds = _context.Users
                .Where(u => u.DoctorServiceIds.Contains(serviceId)) // Filter dokter berdasarkan serviceId
                .Select(u => u.Id)
                .ToList();

            // Dapatkan dokter yang sesuai dengan daftar ID dokter yang ditemukan
            var filteredDoctors = _context.Users
                .Where(u => doctorIds.Contains(u.Id))
                .ToList();
            // Jika user tidak ditemukan, kembalikan status 404 dengan pesan khusus
            if (user == null && filteredDoctors == null)
            {
                return NotFound(new
                {
                    metadata = new
                    {
                        code = 404,
                        message = "Data peserta tidak ditemukan"
                    }
                });
            }

            // Ambil daftar dokter, misalnya berdasarkan serviceId jika diperlukan

            // Kembalikan data user dan dokter dalam response API
            return Ok(new
            {
                User = user,
                Docter = filteredDoctors,
            });
        }
    }
}