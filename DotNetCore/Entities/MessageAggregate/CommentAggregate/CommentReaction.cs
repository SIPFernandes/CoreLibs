namespace DotNetCore.Entities.MessageAggregate.CommentAggregate
{
    public class CommentReaction : Reaction
    {        
        public CommentReaction(int reactedObjId, string reactionType, string userId) : base(reactedObjId, reactionType, userId)
        {            
        }      

        public override bool Equals(object? obj)
        {
            return Equals(obj as CommentReaction);
        }

        public bool Equals(CommentReaction? other)
        {
            return other is not null &&
                   ReactedObjId == other.ReactedObjId &&
                   UserId == other.UserId &&
                   ReactionType == other.ReactionType;
        }

        public override int GetHashCode()
        {
            HashCode hash = new();
            hash.Add(ReactedObjId);
            hash.Add(UserId);
            hash.Add(ReactionType);
            return hash.ToHashCode();
        }        

        public static bool operator ==(CommentReaction? left, CommentReaction? right)
        {
            return EqualityComparer<CommentReaction>.Default.Equals(left, right);
        }

        public static bool operator !=(CommentReaction? left, CommentReaction? right)
        {
            return !(left == right);
        }  
    }
}
