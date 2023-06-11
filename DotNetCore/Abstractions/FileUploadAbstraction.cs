using DotNetCore.Entities;
using DotNetCore.Interfaces;
using DotNetCore.Models;
using Microsoft.Extensions.Logging;

namespace DotNetCore.Abstractions
{
    public abstract class FileUploadAbstraction : IFileUploadService
    {
        protected readonly ILogger<FileUploadAbstraction> _logger;
        protected virtual int MaxFileSizeMB { get => 5; } //MB
        private readonly int MaxFileSize;

        public FileUploadAbstraction(ILogger<FileUploadAbstraction> logger)
        {
            _logger = logger;

            MaxFileSize = MaxFileSizeMB * 1024 * 1024; //MB
        }        

        public async Task Upload(FileUpload upload, string uploadedBy, object? data = null)
        {
            CheckFileSize(upload);

            if (upload.Error is null)
            {
                try
                {
                    var file = await TreatUploadedFile(upload, uploadedBy, data);

                    upload.FileId = file.Id;

                    upload.CancellationTokenSource.Dispose();
                }
                catch (Exception ex) when (upload.CancellationTokenSource.IsCancellationRequested)
                {
                    _logger.LogError(ex.Message);
                }
                catch (InvalidCastException ex)
                {
                    _logger.LogError(ex.StackTrace);
                    
                    upload.Error = FileUpload.ErrorType.FileTypeError;                                        
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.StackTrace);

                    upload.Error = FileUpload.ErrorType.GenericError;
                }
            }
            else
            {
                upload.Dispose();
            }
        }
        public void CheckFileSize(FileUpload upload)
        {
            if (upload.File.Size > MaxFileSize)
            {
                upload.Error = FileUpload.ErrorType.FileSizeLimit;
            }
            else if (upload.File.Size == 0)
            {
                upload.Error = FileUpload.ErrorType.FileSizeError;
            }
        }

        public abstract Task<FileBase> GetFile(int fileId);
        public abstract Task<FileBase> SaveFileToDB(FileUpload upload, byte[] bytes, string uploadedBy,
            object? data = null);
        public abstract Task DeleteUploadedFile(FileUpload upload);        
        protected abstract Task<FileBase> TreatUploadedFile(FileUpload upload, string uploadedBy, object? data = null);
    }
}
