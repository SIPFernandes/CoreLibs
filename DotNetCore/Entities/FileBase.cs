namespace DotNetCore.Entities
{
    public class FileBase : BaseEntity
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public byte[] Content { get; set; }

        public FileBase(FileBase file) : base (file.CreatorId)
        {
            Id = file.Id;
            Name = file.Name;
            Type = file.Type;
            Content = file.Content;
        }

        public FileBase(string name, string type, byte[] content, string creatorId) : base(creatorId)
        {
            Name = name;
            Type = type;
            Content = content;
        }
    }
}
