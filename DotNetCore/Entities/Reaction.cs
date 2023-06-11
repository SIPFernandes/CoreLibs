namespace DotNetCore.Entities
{
    public class Reaction
    {
        public int ReactedObjId { get; set; }
        public string UserId { get; set; }
        public string ReactionType { get; set; }
        
        public Reaction(int reactedObjId, string reactionType, string userId) 
        { 
            ReactedObjId = reactedObjId;
            UserId = userId;
            ReactionType = reactionType;
        }
    }
}
