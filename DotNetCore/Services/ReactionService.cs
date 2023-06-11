using DotNetCore.Entities.MessageAggregate.NotificationsAggregate;
using DotNetCore.Enums;
using DotNetCore.Helpers;
using DotNetCore.Interfaces;
using DotNetCore.Models;

namespace DotNetCore.Services
{
    public class ReactionService : IReactionService
    {
        private readonly IReactionRepository _reactionRepository;
        private readonly INotificationService _notifService;
        public ReactionService(IReactionRepository reactionRepository,
            INotificationService notifService) 
        {
            _reactionRepository = reactionRepository;
            _notifService = notifService;
        }

        public async Task<bool> ReactUnreact(ReactedObject reactedObj, string reaction, string userId)
        {            
            var clickedReaction = reactedObj.CreateReaction(reactedObj.Id, reaction, userId);

            var oldReaction = reactedObj.ReactUnreact(clickedReaction);

            var reacted = await _reactionRepository.ReactUnreact(clickedReaction, oldReaction);

            if (reacted && userId != reactedObj.CreatorId) 
            {
                await SendNotification(reactedObj, userId);
            }
            
            return reacted;
        }       

        public virtual async Task SendNotification(ReactedObject reactedObj, string creatorId,
            object? notifData = null)
        {
            var text = notifData != null ? notifData.Serialize() : string.Empty;

            var type = reactedObj.GetType().Name + BaseNotificationEnum.Type.Reaction.ToString();

            var notif = new Notification(text, type, creatorId, reactedObj.CreatorId);

            await _notifService.Insert(notif);            
        }
    }
}
