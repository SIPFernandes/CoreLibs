using DotNetCore.Entities;
using DotNetCore.Models;

namespace DotNetCore.Interfaces
{
    public interface IFileUploadService
    {
        public Task<FileBase> GetFile(int fileId);
        public Task Upload(FileUpload upload, string uploadedBy, object? data = null);
        public Task<FileBase> SaveFileToDB(FileUpload upload, byte[] bytes, string uploadedBy,
            object? data = null);
        public void CheckFileSize(FileUpload upload);
        public Task DeleteUploadedFile(FileUpload upload);
    }
}
