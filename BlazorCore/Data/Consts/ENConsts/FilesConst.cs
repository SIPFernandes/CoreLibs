namespace BlazorCore.Data.Consts.ENConsts
{
    public class FilesConst
    {
        public const string UploadTitle = "Upload document";        
        public const string UploadingDocuments = "Uploading documents...";
        public const string DocumentsUploaded = "{0} Documents uploaded";
        public const string ErrorOccured = "Error occured while uploading";
        public const string UploadCancelConfirmation = "Do you want to remove all documents uploaded?";
        public const string DropzoneText = "Drop files here to upload or";
        public const string ChooseFiles = "choose files";
        public const string FileUploaded = "File uploaded successful";
        public const string ImageUploaded = "The image was uploaded successfully";
        public const string Uploaded = "uploaded";
        public const string FileUploading = "Uploading file...";
        public const string ImageUploading = "Almost there! Your image will be ready any moment now.";
        public const string DeleteConfirm = "Do you want to remove this file?";
        public const string AddImage = "Add Image";
        public const string DragDrop = "Drag & Drop";
        public const string MentionFile = "Mention file";
        
        //errors
        public const string UploadGenericError = "Something went wrong. Please, try again...";
        public static readonly string FileSizeLimit = $"File size exceeds limit";
        public const string FileSizeError = "File is empty.";
        public const string FileTypeRequiredSrc = "File type error";
        
        
        public class UploadsConst
        {
            public const string DropzoneTextImageMaxSize = "Max file size is {0}{1}.";
            public const string DropzoneTextImageSupportedFiles = "Supported file types are .jpg, .png, and .pdf.";
            public const string UploadPhoto = "Upload photo";
            public const string ChangePhoto = "Change photo";
            public const string RemovePhoto = "Remove photo";
            public const string AddFile = "Add file";
        }
    }
}
