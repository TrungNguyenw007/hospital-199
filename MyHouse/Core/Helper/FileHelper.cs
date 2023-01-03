using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Contract.Uploads;
using Core.Const;
using Core.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Core.Helper
{
    public static class FileHelper
    {
        private static List<string> validImgExtensions = new List<string>()
        {
            ".jpg",
            ".png"
        };
        public static async Task<FileDto> UploadImage(IFormFile file,string basePath,string baseUri ="")
        {
            string directoryPath = "";
            var filePath = "";
            var fileDto = new FileDto();
                if (file.Length > 0)
                {
                    var ext = Path.GetExtension(file.FileName);
    
                    if (!validImgExtensions.Any(x => x.Contains(ext)))
                    {
                        throw new GlobalException(HttpMessage.InvalidExtension, HttpStatusCode.BadRequest);
                    }

                    var directory = $"{DateTime.Now:dd-MM-yyyy}";
                    directoryPath = Path.GetFullPath(Path.Combine(basePath,directory));
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    var fileName = $"{DateTime.Now:yyyyMMddHHmmss}_" + file.FileName;
                    filePath = Path.Combine(directoryPath,fileName);
                    using (var fileStream = new FileStream(filePath , FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    fileDto.Path = filePath;
                    fileDto.FileName =Path.Combine(directory , fileName);
                    fileDto.Url = baseUri + fileDto.FileName;
                    return fileDto;
                }
                else
                {
                    return fileDto;
                }
           
        }

        public static void WriteLog(Exception e,string basePath)
        {
            
            string filePath = Path.GetFullPath(Path.Combine(basePath,$"{DateTime.Now:dd-MM-yyyy}.txt"));
            StreamWriter sw;

            if (!File.Exists(filePath))
            {
               sw = File.CreateText(filePath);
            }
            else
            {
                sw = File.AppendText(filePath);
            }


            sw.WriteLine("----------------------------------------");
            sw.WriteLine($"Time:{DateTime.Now:dd-MM-yyyy-->HH/mm}");
            sw.WriteLine($"Error:{e.Message}");
            sw.WriteLine(e.StackTrace);
            sw.WriteLine("----------------------------------------");

            sw.Flush();
            sw.Close();
        }
        
  
        
        
    }
}