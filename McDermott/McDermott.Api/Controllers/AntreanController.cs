using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace McDermott.Api.Controllers
{
    [Route("antrean")]
    [ApiController]
    public class AntreanController : ControllerBase
    {
        private bool IsValidToken(string token, string username)
        {
            // Implementasikan logika validasi token dan username di sini
            // Misalnya: cek di database atau sistem lain untuk memastikan token dan username valid
            return true; // Asumsikan valid untuk sementara
        }

        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
        [HttpGet("status/{kode_poli}/{tanggalperiksa}")]
        public IActionResult GetStatusAntrean([FromRoute] string kode_poli, [FromRoute] DateTime tanggalperiksa,
                                              [FromHeader(Name = "x-token")] string token, [FromHeader(Name = "x-username")] string username)
        {
            // Validasi token dan username (implementasikan logika validasi sesuai kebutuhan)
            if (!IsValidToken(token, username))
            {
                var unauthorizedResponse = new
                {
                    metadata = new
                    {
                        message = "Unauthorized",
                        code = 401
                    }
                };
                return Unauthorized(unauthorizedResponse);
            }

            // Contoh hardcoded response
            var antreanList = new List<object>
            {
                new
                {
                    namapoli = "Poli Umum",
                    totalantrean = "25",
                    sisaantrean = 4,
                    antreanpanggil = "A1-21",
                    keterangan = "",
                    kodedokter = 123456,
                    namadokter = "Dr. Ali",
                    jampraktek = "08:00-13:00"
                },
                new
                {
                    namapoli = "Poli Umum",
                    totalantrean = "11",
                    sisaantrean = 1,
                    antreanpanggil = "A2-10",
                    keterangan = "",
                    kodedokter = 123466,
                    namadokter = "Dr. Adi",
                    jampraktek = "08:00-12:00"
                }
            };

            var response = new
            {
                response = antreanList,
                metadata = new
                {
                    message = "Ok",
                    code = 200
                }
            };

            return Ok(response);
        }

        [HttpPost]
        public IActionResult AmbilAntrean([FromBody] AntreanRequest request,
                                          [FromHeader(Name = "x-token")] string token,
                                          [FromHeader(Name = "x-username")] string username)
        {
            // Validasi token dan username (implementasikan logika validasi sesuai kebutuhan)
            if (!IsValidToken(token, username))
            {
                var unauthorizedResponse = new
                {
                    metadata = new
                    {
                        message = "Unauthorized",
                        code = 401
                    }
                };
                return Unauthorized(unauthorizedResponse);
            }

            // Implementasikan logika pengambilan antrean di sini
            // Contoh: Memproses permintaan dan mengembalikan respons hardcoded
            var antreanResponse = new
            {
                response = new
                {
                    nomorkartu = request.NomorKartu,
                    nik = request.NIK,
                    kodepoli = request.KodePoli,
                    tanggalperiksa = request.TanggalPeriksa.ToString("yyyy-MM-dd"),
                    keluhan = request.Keluhan,
                    kodedokter = request.KodeDokter,
                    jampraktek = request.JamPraktek,
                    norm = request.NoRM,
                    nohp = request.NoHP,
                    nomorantrean = "A1-21", // Nomor antrean contoh
                    kodebooking = Guid.NewGuid().ToString(), // Kode booking contoh
                    keterangan = "Silakan menunggu antrean Anda"
                },
                metadata = new
                {
                    message = "Ok",
                    code = 200
                }
            };

            return Ok(antreanResponse);
        }

        [HttpPut("batal")]
        public IActionResult BatalAntrean([FromBody] BatalAntreanRequest request,
                                          [FromHeader(Name = "x-token")] string token,
                                          [FromHeader(Name = "x-username")] string username)
        {
            // Validasi token dan username (implementasikan logika validasi sesuai kebutuhan)
            if (!IsValidToken(token, username))
            {
                var unauthorizedResponse = new
                {
                    metadata = new
                    {
                        message = "Unauthorized",
                        code = 401
                    }
                };
                return Unauthorized(unauthorizedResponse);
            }

            // Implementasikan logika pembatalan antrean di sini
            //bool success = BatalAntrean(request);
            bool success = true;

            if (success)
            {
                var response = new
                {
                    metadata = new
                    {
                        message = "Antrean berhasil dibatalkan",
                        code = 200
                    }
                };
                return Ok(response);
            }
            else
            {
                var response = new
                {
                    metadata = new
                    {
                        message = "Antrean gagal dibatalkan. Antrean tidak ditemukan atau sudah diproses.",
                        code = 201
                    }
                };
                return StatusCode(201, response);
            }
        }

        [HttpGet("sisapeserta/{nomorkartu_jkn}/{kode_poli}/{tanggalperiksa}")]
        public IActionResult SisaAntreanPeserta(
           string nomorkartu_jkn,
           string kode_poli,
           DateTime tanggalperiksa,
           [FromHeader(Name = "x-token")] string token,
           [FromHeader(Name = "x-username")] string username)
        {
            // Validasi token dan username (implementasikan logika validasi sesuai kebutuhan)
            if (!IsValidToken(token, username))
            {
                var unauthorizedResponse = new
                {
                    metadata = new
                    {
                        message = "Unauthorized",
                        code = 401
                    }
                };
                return Unauthorized(unauthorizedResponse);
            }

            // Implementasikan logika untuk mendapatkan data sisa antrean
            var antrean = GetSisaAntrean(nomorkartu_jkn, kode_poli, tanggalperiksa);

            if (antrean != null)
            {
                var response = new
                {
                    response = new
                    {
                        nomorantrean = antrean.NomorAntrean,
                        namapoli = antrean.NamaPoli,
                        sisaantrean = antrean.SisaAntrean,
                        antreanpanggil = antrean.AntreanPanggil,
                        keterangan = antrean.Keterangan
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
                var response = new
                {
                    metadata = new
                    {
                        message = "Antrean tidak ditemukan atau sudah tidak berlaku.",
                        code = 201
                    }
                };
                return StatusCode(201, response);
            }
        }

        private AntreanResponse GetSisaAntrean(string nomorkartu_jkn, string kode_poli, DateTime tanggalperiksa)
        {
            // Implementasikan logika untuk mendapatkan data sisa antrean berdasarkan nomorkartu_jkn, kode_poli, dan tanggalperiksa
            // Contoh: Simulasi data sementara

            if (nomorkartu_jkn == "00012345678" && kode_poli == "001" && tanggalperiksa == new DateTime(2020, 1, 28))
            {
                return new AntreanResponse
                {
                    NomorAntrean = "A20",
                    NamaPoli = "Poli Umum",
                    SisaAntrean = 4,
                    AntreanPanggil = "A8",
                    Keterangan = ""
                };
            }

            return null; // Return null jika tidak ditemukan
        }
    }
}

// Model untuk request body
public class AntreanRequest
{
    public string NomorKartu { get; set; }
    public string NIK { get; set; }
    public string KodePoli { get; set; }
    public DateTime TanggalPeriksa { get; set; }
    public string Keluhan { get; set; }
    public int KodeDokter { get; set; }
    public string JamPraktek { get; set; }
    public string NoRM { get; set; }
    public string NoHP { get; set; }
}

// Model untuk request body
public class BatalAntreanRequest
{
    public string NomorKartu { get; set; }
    public string KodePoli { get; set; }
    public DateTime TanggalPeriksa { get; set; }
    public string Keterangan { get; set; }
}

// Model untuk response body
public class AntreanResponse
{
    public string NomorAntrean { get; set; }
    public string NamaPoli { get; set; }
    public int SisaAntrean { get; set; }
    public string AntreanPanggil { get; set; }
    public string Keterangan { get; set; }
}