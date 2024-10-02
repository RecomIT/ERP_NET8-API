using Microsoft.AspNetCore.Http;
using Shared.Helpers;
using Shared.OtherModels.File;
using Shared.OtherModels.User;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Services
{
    public static class FileProcessor
    {
        public static string[] FileFormats = new string[] { "PDF", "CSV", "XLS" };
        public static string GetFileMimetype(string fileExtension)
        {
            string mimetype = null;
            if (fileExtension.ToLower() == "pdf") {
                mimetype = Mimetype.PDF;
            }
            else if (fileExtension.ToLower() == "xls") {
                mimetype = Mimetype.EXCEL;
            }
            else if (fileExtension.ToLower() == "xlsx") {
                mimetype = Mimetype.EXCELX;
            }
            else if (fileExtension.ToLower() == "png") {
                mimetype = Mimetype.PNG;
            }
            else if (fileExtension.ToLower() == "jpg" || fileExtension.ToLower() == "jpeg") {
                mimetype = Mimetype.JPEG;
            }
            return mimetype ?? Mimetype.PDF;
        }
        static string PhysicalDriver = ConfigurationHelper.config.GetSection("PhysicalDriver").Value.ToString();
        static string PhysicalDocFolder = "ERPDOCS";
        static string PhysicalImgFolder = "ERPImages";

        public static void CreateOrgDirectory(string orgCode)
        {
            string docPath = $"{PhysicalDriver}//{PhysicalDocFolder}//{orgCode}";
            Directory.CreateDirectory(docPath);

            Directory.CreateDirectory($"{docPath}//Pdf");
            Directory.CreateDirectory($"{docPath}//Excel");
            Directory.CreateDirectory($"{docPath}//Images");

            string imgPath = string.Format(@"{0}/{1}/{2}", PhysicalDriver, PhysicalImgFolder, orgCode);
            Directory.CreateDirectory(imgPath);

            Directory.CreateDirectory(string.Format(@"{0}/{1}", imgPath, "Images"));
        }
        public static FileDetail Process(long id, IFormFile file, string filePath, AppUser user)
        {
            FileDetail fileDetail = new FileDetail();
            if (id > 0 && !Utility.IsNullEmptyOrWhiteSpace(filePath)) {
                DeleteFile(string.Format(@"{0}/{1}", PhysicalDriver, filePath));
            }
            fileDetail.File = Task.Run(() => SaveFileAsync(file, string.Format(@"{0}", user.OrgCode))).Result;
            fileDetail.FilePath = fileDetail.File.Substring(0, fileDetail.File.LastIndexOf("/"));
            fileDetail.FileName = fileDetail.File.Substring(fileDetail.File.LastIndexOf("/") + 1);
            fileDetail.Extenstion = fileDetail.FileName.Substring(fileDetail.FileName.LastIndexOf(".") + 1);
            fileDetail.FileSize = Math.Round(Convert.ToDecimal((file.Length / 1024)), 0, MidpointRounding.AwayFromZero).ToString() + "KB";
            fileDetail.ActualFileName = file.FileName;
            return fileDetail;
        }
        public static void DeleteFile(string FilePath)
        {
            if (!string.IsNullOrEmpty(FilePath)) {
                if (File.Exists(FilePath)) {
                    File.Delete(FilePath);
                }
            }
        }
        public static byte[] GetFileBytes(string filepath)
        {
            byte[] fileByte = null;
            if (File.Exists(filepath)) {
                FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                Byte[] bytes = br.ReadBytes((Int32)fs.Length);
                br.Close();
                fileByte = bytes;
            }
            return fileByte;
        }
        public static string GetFileExtension(IFormFile file)
        {
            var fileExtension = Path.GetExtension(file.FileName);
            return fileExtension;
        }
        public static string GetFileFolder(string fileExtension)
        {
            string fileFolder = string.Empty;
            if (fileExtension == ".jpeg" || fileExtension == ".jpg" || fileExtension == ".png") {
                fileFolder = "Images";
            }
            else if (fileExtension == ".pdf") {
                fileFolder = "Pdf";
            }
            else if (fileExtension == ".xlsx" || fileExtension == ".xls") {
                fileFolder = "Excel";
            }
            return fileFolder;
        }
        public static string GetDriverPath(string subfolder, string fileFloder)
        {
            string path = string.Empty;
            if (fileFloder == "Images") {
                path = $"{PhysicalImgFolder}//{subfolder}//{fileFloder}";
            }
            else if (fileFloder != "Images") {
                path = $"{PhysicalDocFolder}//{subfolder}//{fileFloder}";
            }
            return path;
        }
        public static string PhysicalDriverImagePath {
            get {
                return @"C:/Projects/Images/";
            }
        }
        public static async Task<string> SaveFileAsync(IFormFile file, string subfolderName)
        {
            var uniqueFileName = (Guid.NewGuid()).ToString();
            var extension = GetFileExtension(file);
            var fileFolder = GetFileFolder(extension.ToLower());
            uniqueFileName = uniqueFileName + extension.ToLower();
            var path = GetDriverPath(subfolderName, fileFolder);
            var driverPath = PhysicalDriver + "//" + path;
            using (var stream = new FileStream(Path.Combine(driverPath, uniqueFileName), FileMode.Create)) {
                await file.CopyToAsync(stream);
            }
            return string.Format(@"{0}/{1}", path, uniqueFileName);
        }
        public static byte[][] ConvertStreamsToBytes(Stream[] streams)
        {
            // Create a jagged array to store byte arrays
            byte[][] byteArray = new byte[streams.Length][];

            for (int i = 0; i < streams.Length; i++) {
                // Convert each Stream to a byte array
                byteArray[i] = ReadFully(streams[i]);
            }

            return byteArray;
        }
        public static byte[] ReadFully(Stream stream)
        {
            // Helper method to convert a Stream to a byte array
            using (MemoryStream ms = new MemoryStream()) {
                stream.CopyTo(ms);
                return ms.ToArray();
            }
        }
        public static byte[] ConvertJaggedArrayToFlatArray(byte[][] jaggedArray)
        {
            // Use LINQ SelectMany to flatten the jagged array
            return jaggedArray.SelectMany(bytes => bytes).ToArray();
        }
        public static byte[][] GetYourJaggedArray()
        {
            // Example: Create a jagged array for demonstration
            byte[][] byteArray = new byte[][]
            {
            new byte[] { 0x01, 0x02, 0x03 },
            new byte[] { 0x04, 0x05, 0x06 },
            new byte[] { 0x07, 0x08, 0x09 }
            };

            return byteArray;
        }
    }
}
