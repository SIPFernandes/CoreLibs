using DotNetCore.Entities;

namespace DotNetCore.Interfaces
{
    public interface IReactionRepository
    {
        public Task<bool> ReactUnreact(Reaction clickedReaction, Reaction? oldReaction);
    }
}
