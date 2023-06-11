using Microsoft.AspNetCore.Components.Forms;

namespace DotNetCore.Models
{
    public class FileUpload : IDisposable
    {                
        public int FileId { get; set; }
        public IBrowserFile File { get; private set; }                        
        public ErrorType? Error { get; set; }
        public int UploadCompletion { get; set; }
        public CancellationTokenSource CancellationTokenSource { get; private set; }        

        public FileUpload(IBrowserFile file)
        {
            CancellationTokenSource = new CancellationTokenSource();                        
            File = file;            
        }

        public void Dispose()
        {
            try
            {
                CancellationTokenSource.Cancel();
                CancellationTokenSource.Dispose();
            }            
            catch (Exception ex) when (ex is ObjectDisposedException) { }                        

            GC.SuppressFinalize(this);
        }

        public enum ErrorType
        {
            FileSizeError,
            FileSizeLimit,
            FileTypeError,
            GenericError
        }
    }
}
