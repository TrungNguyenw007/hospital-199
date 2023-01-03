using System;
using System.Threading.Tasks;
using Contract;
using Contract.Uploads;
using Core.Helper;
using Microsoft.AspNetCore.Http;
using Volo.Abp.DependencyInjection;

namespace Application.Uploads
{
    public class UploadService : IUploadService,ITransientDependency
    {
        
        
        public UploadService()
        {
            
        }


        public async Task<FileDto> UploadFile(IFormFile file)
        {
            throw new Exception();
        }

        public Task<FileDto> UploadImage(IFormFile file)
        {
            return FileHelper.UploadImage(file, GlobalSetting.BASE_IMAGE_PATH,GlobalSetting.BASE_IMAGE_URL);
        }
    }
}