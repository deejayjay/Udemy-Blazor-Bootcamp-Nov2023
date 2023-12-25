using Microsoft.AspNetCore.Components.Forms;

namespace TangyWeb_Server.Service.IService
{
    public interface IFileUploadService
    {
        Task<string> UploadFileAsync(IBrowserFile file);
        bool DeleteFile(string filePath);
    }
}
