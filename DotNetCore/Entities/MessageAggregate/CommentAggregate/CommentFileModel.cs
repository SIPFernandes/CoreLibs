namespace DotNetCore.Entities.MessageAggregate.CommentAggregate
{
    public class CommentFileModel
    {
        public int FileId { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }

        public CommentFileModel(int fileId, string fileName, string fileType)
        {
            FileId = fileId;
            FileName = fileName;
            FileType = fileType;
        }
    }
}
