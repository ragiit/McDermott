namespace McDermott.Web.Extentions
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IWebHostEnvironment _environment;

        public FileUploadService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string?> DownloadFile(string fileName)
        {
            try
            {
                // Ganti "nama_file.extension" dengan nama file yang ingin kamu download

                // Lokasi file di wwwroot
                var filePath = @$"{_environment.WebRootPath}\Uploads\{fileName}";
                return filePath;
                //// Cek apakah file ada
                //if (!System.IO.File.Exists(filePath))
                //{
                //    return null!;
                //}

                //// Ambil konten file dari server
                ////var content = await _httpClient.GetStreamAsync(filePath);

                //// Simpan file secara lokal

                //var memory = new MemoryStream();
                //using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                //await fileStream.CopyToAsync(memory);

                //memory.Position = 0;

                //return fileStream;

                //// Ambil konten file
                ////var content = await System.IO.File.ReadAllBytesAsync(filePath);

                ////// Mengirim file ke client
                ////var response = _httpContextAccessor.HttpContext.Response;
                //////response.Clear();
                ////response.Headers.Add("Content-Disposition", $"attachment; filename=\"{fileName}\"");
                ////response.ContentType = "application/octet-stream";

                ////await response.Body.WriteAsync(content, 0, content.Length);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<(int, string)> UploadFileAsync(IBrowserFile file, int? maxFileSize = 0, string[] allowedExtensions = null)
        {
            if (file == null)
                return (0, $"Null File");

            var uploadDirectory = Path.Combine(_environment.WebRootPath, "files");
            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }

            if (maxFileSize > 0)
            {
                if (file.Size > maxFileSize)
                {
                    return (0, $"File: {file.Name} exceeds the maximum allowed file size.");
                }
            }

            var fileExtension = Path.GetExtension(file.Name);
            if (allowedExtensions is not null && allowedExtensions.Count() > 0 && !allowedExtensions.Contains(fileExtension))
            {
                return (0, $"File: {file.Name}, File type not allowed");
            }

            //var fileName = $"{Guid.NewGuid()}{fileExtension}";
            var fileName = $"{file.Name}";
            var path = Path.Combine(uploadDirectory, fileName);
            await using var fs = new FileStream(path, FileMode.Create);
            long size = maxFileSize == null || maxFileSize == 0 ? file.Size : maxFileSize.ToInt32();
            await file.OpenReadStream(size).CopyToAsync(fs);
            return (1, fileName);
        }
    }
}