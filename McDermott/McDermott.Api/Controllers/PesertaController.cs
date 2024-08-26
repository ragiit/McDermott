using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace McDermott.Api.Controllers
{
    [Route("peserta")]
    [ApiController]
    public class PesertaController : ControllerBase
    {
        [HttpPost]
        public IActionResult PostPeserta([FromBody] Peserta peserta)
        {
            // Validasi data peserta
            if (peserta == null)
            {
                return BadRequest(new
                {
                    metadata = new
                    {
                        code = 201,
                        message = "Data peserta tidak boleh kosong"
                    }
                });
            }

            // Simpan data peserta ke database atau lakukan proses lain sesuai kebutuhan
            // ...

            return Ok(new
            {
                metadata = new
                {
                    code = 200,
                    message = "Sukses"
                }
            });
        }
    }
}

public class Peserta
{
    public string Nomorkartu { get; set; }
    public string Nik { get; set; }
    public string Nomorkk { get; set; }
    public string Nama { get; set; }
    public string Jeniskelamin { get; set; }
    public DateTime Tanggallahir { get; set; }
    public string Alamat { get; set; }
    public string Kodeprop { get; set; }
    public string Namaprop { get; set; }
    public string Kodedati2 { get; set; }
    public string Namadati2 { get; set; }
    public string Kodekec { get; set; }
    public string Namakec { get; set; }
    public string Kodekel { get; set; }
    public string Namakel { get; set; }
    public string Rw { get; set; }
    public string Rt { get; set; }
}