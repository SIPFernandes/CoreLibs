using DotNetCore.Interfaces;
using DotNetCore.Models;
using System.ComponentModel.DataAnnotations;

namespace DotNetCore.Entities.MessageAggregate.CommentAggregate
{
    public class Comment : ReactedObject, IAggregateRoot
    {
        public int CommentObjId { get; private set; }
        [Required]
        public string Text { get; set; }
        public virtual IReadOnlyCollection<Comment> Replies => _replies.AsReadOnly();
        private readonly List<Comment> _replies = new();
        public int? ReplyToId { get; set; }
        public CommentReplied? CommentReplied { get; set; }

        public Comment(string text, string creatorId, int objId = 0) : base(creatorId) 
        {
            CommentObjId = objId;
            Text = text;
        }

        public Comment(Comment comment, bool nestedReplies = false) : base(comment.Reactions, comment.CreatorId)
        {
            Id = comment.Id;

            CommentObjId = comment.CommentObjId;

            Text = comment.Text;

            CreatedAt = comment.CreatedAt;
            
            ModifiedAt = comment.ModifiedAt;

            ReplyToId = comment.ReplyToId;

            IsDeleted = comment.IsDeleted;

            if (nestedReplies)
            {
                RepliesCount = comment.Replies.Count;
            }
            else
            {
                CommentReplied = comment.CommentReplied;
            }
        }

        public int RepliesCount;
        public Dictionary<int, Comment>? RecentReplies;
    }
}
