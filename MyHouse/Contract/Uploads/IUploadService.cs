using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Contract.Uploads
{
    public interface IUploadService
    {
        public Task<FileDto> UploadFile(IFormFile file);
        public Task<FileDto>   UploadImage(IFormFile file);
        
    }
}