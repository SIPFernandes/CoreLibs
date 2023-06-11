using DotNetCore.Entities;

namespace DotNetCore.Models
{
    public class ReactedObject : BaseEntity
    {
        public Dictionary<string, HashSet<string>> ReactionsDict = new();
        public virtual IReadOnlyCollection<Reaction> Reactions => _reactions.AsReadOnly();
        private readonly List<Reaction> _reactions = new();
        private Type ReactionType; 

        public ReactedObject(IReadOnlyCollection<Reaction> reactions,
            string creatorId) : base(creatorId)
        {
            ReactionType = Reactions.GetType().GetGenericArguments().Single();

            if (reactions is { Count: > 0 })
            {
                ReactionsDict = new Dictionary<string, HashSet<string>>(
                    reactions.GroupBy(Y => Y.ReactionType)
                        .Select(y => new KeyValuePair<string, HashSet<string>>(
                            y.Key, y.Select(z => z.UserId).ToHashSet())));
            }
        }

        public ReactedObject(string creatorId) : base(creatorId)
        {
            ReactionType = Reactions.GetType().GetGenericArguments().Single();
        }

        public Reaction CreateReaction(int objectId, string reaction, string userId)
        {            
            var obj = (Reaction)Activator.CreateInstance(ReactionType, objectId, reaction, userId);

            if (obj is null)
            {
                throw new NullReferenceException(nameof(obj));
            }

            return obj;
        }

        public string? GetUserReaction(string userId)
        {
            string? result = null;

            foreach (var reaction in ReactionsDict)
            {
                if (reaction.Value.Contains(userId))
                {
                    result = reaction.Key;

                    break;
                }
            }

            return result;
        }

        public Reaction? ReactUnreact(Reaction clickedReaction)
        {
            Reaction? oldReaction = null;

            if (ReactionsDict.ContainsKey(clickedReaction.ReactionType) &&
                ReactionsDict[clickedReaction.ReactionType].Remove(clickedReaction.UserId))
            {
                oldReaction = clickedReaction;
            }
            else
            {
                foreach (var reaction in ReactionsDict)
                {
                    if (reaction.Key != clickedReaction.ReactionType &&
                        reaction.Value.Remove(clickedReaction.UserId))
                    {
                        oldReaction = CreateReaction(clickedReaction.ReactedObjId,
                            reaction.Key, clickedReaction.UserId);

                        InsertReaction(clickedReaction.ReactionType, clickedReaction.UserId);

                        break;
                    }
                }
            }

            if (oldReaction == null)
            {
                InsertReaction(clickedReaction.ReactionType, clickedReaction.UserId);
            }

            return oldReaction;
        }

        private void InsertReaction(string reaction, string userId)
        {
            if (!ReactionsDict.ContainsKey(reaction))
            {
                ReactionsDict.Add(reaction, new HashSet<string>());
            }

            ReactionsDict[reaction].Add(userId);
        }
    }
}
