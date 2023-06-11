//using Microsoft.AspNetCore.Components.Forms;
//using System;
//using System.IO;
//using System.Text;
//using System.Threading.Tasks;
//using YTrack.Data.Constants;
//using YTrack.Data.Constants.ENGConsts;
//using YTrack.Data.Enums;
//using YTrack.Data.Models;
//using static YTrack.Shared.ComponentsShared.FilesShared.FilePreviewShared.FilePreview;
//using static YTrack.Data.Enums.FileEnums;
//using System.Threading;

using DotNetCore.Enums;

namespace BlazorCore.Areas.Helpers
{
    public static class FilesHelper
    {
        public static string GetFileExtensionColor(string fileType)
        {
            var colorExtension = "color-n500";

            if (Enum.IsDefined(typeof(FileEnums.TextExtensions), fileType))
            {
                colorExtension = "color-r300";
            }
            else if (Enum.IsDefined(typeof(FileEnums.ImageExtensions), fileType))
            {
                colorExtension = "color-b300";
            }
            else if (Enum.IsDefined(typeof(FileEnums.ExcelExtensions), fileType))
            {
                colorExtension = "color-g400";
            }

            return colorExtension;
        }
//        private const double MaxFileSize = 5;

//        public static async Task<string> UploadMedia(IBrowserFile file,
//            CancellationToken? cancellationToken = null)
//        {
//            var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

//            try
//            {                
//                await using var fs = new FileStream(path, FileMode.Create);

//                if (cancellationToken != null)
//                {
//                    await file.OpenReadStream(file.Size, cancellationToken.Value)
//                        .CopyToAsync(fs, cancellationToken.Value);
//                }
//                else
//                {
//                    await file.OpenReadStream(file.Size)
//                        .CopyToAsync(fs);
//                }

//                var bytes = new byte[file.Size];

//                fs.Position = 0;

//                if (cancellationToken != null)
//                {
//                    await fs.ReadAsync(bytes, cancellationToken.Value);
//                }
//                else
//                {
//                    await fs.ReadAsync(bytes);
//                }

//                fs.Close();

//                File.Delete(path);

//                return Convert.ToBase64String(bytes);
//            }
//            catch (Exception) when (cancellationToken != null && 
//                cancellationToken.Value.IsCancellationRequested)
//            {
//                File.Delete(path);

//                throw;
//            }
//        }

//        public static string GetFileExtension(string fileName)
//        {
//            return Path.GetExtension(fileName)[1..]
//                    .ToLower();
//        }

//        public static string GetImageFromByteArray(byte[] image)
//        {
//            var base64 = Encoding.ASCII.GetString(image);
//            return string.Format(FilesConst.ImageConst.Src, base64);
//        }

//        public static async Task<byte[]> StreamToByteArray(Stream input,
//            CancellationToken cancellationToken)
//        {
//            using MemoryStream ms = new();

//            await input.CopyToAsync(ms, cancellationToken: cancellationToken);

//            return ms.ToArray();
//        }

//        public static int GetUploadProgress(int totalSteps, int currentStep)
//        {
//            return (currentStep * 100) / totalSteps;
//        }

//        public static async Task<Tuple<string, string>> GetThumbnailAndImage(InputFileChangeEventArgs e)
//        {
//            var file = e.File;

//            var thumbnailTmp = await file.RequestImageFileAsync("jpg", 100, 100);

//            var imageTmp = await file.RequestImageFileAsync("jpg", 200, 200);

//            var thumbnail = await UploadMedia(thumbnailTmp);

//            var image = await UploadMedia(imageTmp);

//            return new Tuple<string, string>(thumbnail, image);
//        }

//        public static bool IsValidSize(InputFileChangeEventArgs e, double maxFileSizeMb = MaxFileSize)
//        {
//            var file = e.File;

//            return file.Size <= maxFileSizeMb * 1024 * 1024;
//        }

//        public static string GetFileUrl(int attachmentId, string taskId)
//        {
//            var filePreviewTokenModel = new FilePreviewTokenModel(attachmentId);

//            return GetFileUrl(filePreviewTokenModel, taskId);
//        }

//        public static string GetFileUrlWithCommentId(int attachmentId, string taskId, int commentId, int pageId)
//        {
//            var filePreviewTokenModel = new FilePreviewTokenModel(attachmentId, commentId, pageId);

//            return GetFileUrl(filePreviewTokenModel, taskId);
//        }

//        public static string GetFileIcon(this string fileName)
//        {
//            var ext = GetFileExtension(fileName);

//            return GetFileIconFromExt(ext);
//        }

//        public static string GetFileIconFromExt(this string ext)
//        {
//            return Enum.IsDefined(typeof(FileEnums.ImageExtensions), ext)
//                ? IconsConst.Image
//                : IconsConst.File;
//        }

//        public static byte[] GetByteArrayFromBase64(string imageBase64)
//        {
//            return string.IsNullOrEmpty(imageBase64)
//                ? null
//                : Convert.FromBase64String(imageBase64);
//        }

//        public static string GetStorageFileTypeImage(this FileUploadModel file)
//        {
//            return file.Type switch
//            {
//                FileTypes.Image => IconsConst.Attachments.Types.Image,
//                _ => IconsConst.Attachments.Types.File
//            };
//        }

//        private static string GetFileUrl(FilePreviewTokenModel filePreviewTokenModel, string taskId)
//        {
//            var code = TokensHelper.EncodeObjectToken(filePreviewTokenModel);

//            return $"{AppConst.Workspace.Task}/{taskId}/{code}";
//        }
    }
}
