using Microsoft.AspNetCore.Http;
using SmartParkingAbstract.Services.File;
using SmartParkingAbstract.Services.General;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingCoreServices.File
{
    public class FileService: IFileService
    {
        private readonly IHelpers helpers;
        private readonly IHttpContextAccessor httpContextAccessor;

        public FileService(IHelpers helpers, IHttpContextAccessor httpContextAccessor)
        {
            this.helpers = helpers;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task<string> SaveFile(IFormFile file)
        {
            var folderName = "Images";
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            fileName = helpers.GenerateFileName(subfix: "_" + fileName);
            var fullPath = Path.Combine(pathToSave, fileName);
            var dbPath = Path.Combine(folderName, fileName);
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            var request = httpContextAccessor?.HttpContext?.Request;
            return $"{request?.Scheme ?? ""}://{request?.Host.Value ?? ""}/{dbPath.Replace("\\", "/")}"; ;
        }
    }
}
