using Dapper;
using Microsoft.AspNetCore.Http;
using Shared.OtherModels.Pagination;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;
using Shared.Separation.Filter;
using Shared.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Shared.Expense_Reimbursement.Services
{
    public static class Upload
    {
        public static async Task<string> SaveFileAsync(IFormFile file, string subfolderName, string transactionType, string employeeCode, string referenceNumber)
        {
            var uniqueFileName = (Guid.NewGuid()).ToString();
            var extension = GetFileExtension(file);
            var fileFolder = GetFileFolder(subfolderName, transactionType, employeeCode);
            uniqueFileName = uniqueFileName + extension.ToLower();
            var path = GetDriverPath(subfolderName, fileFolder);
            var driverPath = PhysicalDriver + "/" + path;
            using (var stream = new FileStream(Path.Combine(string.Format(@"{0}", driverPath), uniqueFileName), FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return string.Format(@"{0}/{1}", path, uniqueFileName);
        }


        public static string PhysicalDriver = ConfigurationHelper.config.GetSection("PhysicalDriver").Value.ToString();
        public static string PhysicalDocFolder = "ERPDOCS";

        public static string GetFileFolder(string subfolderName, string transactionType, string employeeCode)
        {
            string fileFolder = "Expense_Reimbursement";
            string path = Path.Combine(PhysicalDriver,PhysicalDocFolder,subfolderName,fileFolder, transactionType, employeeCode);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            } 

            string directoryPath = string.Format(@"{0}/{1}/{2}", fileFolder, transactionType, employeeCode);
            return directoryPath;
        }

        public static string GetFileExtension(IFormFile file)
        {
            var fileExtension = Path.GetExtension(file.FileName);
            return fileExtension;
        }

        public static string GetDriverPath(string subfolder, string fileFloder)
        {
            string path = string.Empty;
            path = string.Format(@"{0}/{1}/{2}", PhysicalDocFolder, subfolder, fileFloder);            
            return path;
        }

        public static void DeleteFile(string FilePath)
        {                    
            if (!string.IsNullOrEmpty(FilePath))
            {
                if (File.Exists(FilePath))
                {
                    File.Delete(FilePath);
                }
            }
        }

 
    }
}





