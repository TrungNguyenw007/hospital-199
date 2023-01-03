using System.Threading.Tasks;
using Contract.Parts;
using Contract.Uploads;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using WebClient.RequestHttp;

namespace WebClient.Service.Upload
{
    public class UploadService 
    {
        public UploadService()
        {
            
        }
        
        public async Task<FileDto> UploadFile(IBrowserFile file)
        {
           return await RequestClient.PostAPIWithFileAsync<FileDto>("upload/image", file);
        }

        public async Task<FileDto> UploadImage(IBrowserFile file)
        {
            return  await RequestClient.PostAPIWithFileAsync<FileDto>("upload/image", file);
        }
    }
}