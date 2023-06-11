using DotNetCore.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace DotNetCore.Helpers
{
    public static class FilesHelper
    {
        public static string GetFileExtension(string fileName)
        {
            return Path.GetExtension(fileName)[1..]
                    .ToLower();
        }

        public static async Task<byte[]> UploadToByteArray(FileUpload upload)
        {
            var path = Path.GetTempFileName();

            try
            {
                await using var fs = new FileStream(path, FileMode.Create);

                await upload.File.OpenReadStream(upload.File.Size, upload.CancellationTokenSource.Token)
                        .CopyToAsync(fs, upload.CancellationTokenSource.Token);                

                var bytes = new byte[upload.File.Size];

                fs.Position = 0;

                await fs.ReadAsync(bytes, upload.CancellationTokenSource.Token);                

                fs.Close();

                File.Delete(path);

                return bytes;
            }
            catch (Exception) when (upload.CancellationTokenSource.IsCancellationRequested)
            {
                File.Delete(path);

                throw;
            }
        }
        
        public static bool IsValidSize(IBrowserFile file, double maxFileSizeMb)
        {            
            return file.Size <= maxFileSizeMb * 1024 * 1024;
        }

        public static async Task<string> UploadToFileSystem(IBrowserFile file, string basePath, string? fileName = null)
        {
            var imageTmp = await file.RequestImageFileAsync("jpg", 200, 200);
            
            using var tmp = new FileUpload(imageTmp);

            var bytes = await UploadToByteArray(tmp);
            
            fileName = fileName == null
                ? file.Name
                : fileName + "." + GetFileExtension(file.Name);            

            return await UploadToFileSystem(bytes, basePath, fileName);
        }

        public static async Task<string> UploadToFileSystem(byte[] bytes, string basePath, string fileName)
        {            
            var filePath = Path.Combine(basePath, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await fileStream.WriteAsync(bytes);
            }

            return fileName;
        }

        public static void RemoveFromFileSystem(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public static string? FoundFileAndExtension(string directoryPath, string fileNameWithoutExtension)
        {
            var file = FoundCompleteFilePath(directoryPath, fileNameWithoutExtension);

            return file == null
                ? null
                : fileNameWithoutExtension + "." + GetFileExtension(file);
        }

        public static string? FoundCompleteFilePath(string directoryPath, string fileNameWithoutExtension)
        {
            var files = Directory.GetFiles(directoryPath, fileNameWithoutExtension + ".*");

            return files.SingleOrDefault();
        }

        public static async Task<string> UploadToBase64(FileUpload upload)
        {
            var bytes = await UploadToByteArray(upload);

            return Convert.ToBase64String(bytes);
        }
    }
}
