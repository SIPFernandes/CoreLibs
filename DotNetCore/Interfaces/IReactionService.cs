using DotNetCore.Models;

namespace DotNetCore.Interfaces
{
    public interface IReactionService
    {
        public Task<bool> ReactUnreact(ReactedObject reactedObj, string reaction, string userId);
        public Task SendNotification(ReactedObject reactedObj, string creatorId,
            object? notifData = null);
    }
}
