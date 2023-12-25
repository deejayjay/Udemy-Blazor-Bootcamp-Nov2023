using Microsoft.AspNetCore.Components.Forms;
using TangyWeb_Server.Service.IService;

namespace TangyWeb_Server.Service
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FileUploadService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public bool DeleteFile(string filePath)
        {
            var fileAbsolutePath = $"{_webHostEnvironment.WebRootPath}{filePath}";

            if (File.Exists(fileAbsolutePath))
            {
                File.Delete(fileAbsolutePath);
                return true;
            }

            return false;
        }

        public async Task<string> UploadFileAsync(IBrowserFile file)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.Name)}";
            var folderDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "images", "product");

            Directory.CreateDirectory(folderDirectory);

            var filePath = Path.Combine(folderDirectory, fileName);
            
            await using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.OpenReadStream().CopyToAsync(fileStream);

            var uploadPath = $"/images/product/{fileName}";
            
            return uploadPath;
        }
    }
}
