using System.Text.Json.Serialization;

namespace DotNetCore.Entities.MessageAggregate.CommentAggregate
{
    public class CommentReplied
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public string CreatorId { get; set; }
        public DateTime CreatedAt { get; set; }

        [JsonConstructor]
        public CommentReplied(int id, string comment, string creatorId, DateTime createdAt)
        {
            Id = id;
            Comment = comment;
            CreatorId = creatorId;
            CreatedAt = createdAt;
        }
        
        public CommentReplied(Comment comment) 
        {
            if (comment.ReplyToId != null)
            {
                Id = comment.ReplyToId.Value;

                ReplyToReply = true;
            }
            else
            {
                Id = comment.Id;
            }
            
            Comment = comment.Text;
            CreatorId = comment.CreatorId;
            CreatedAt = comment.CreatedAt;
        }

        [JsonIgnore]
        public bool ReplyToReply;
    }
}
