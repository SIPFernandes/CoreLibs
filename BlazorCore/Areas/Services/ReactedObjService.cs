using BlazorCore.Areas.Interfaces;
using DotNetCore.Interfaces;
using DotNetCore.Models;

namespace BlazorCore.Areas.Services
{
    public class ReactedObjService : IReactedObjService
    {
        private readonly IReactionService _reactionService;        
        public ReactedObjService(IReactionService reactionService)
        {
            _reactionService = reactionService;            
        }

        public async Task<bool> ReactUnreact(ReactedObject reactedObj, string reaction, string userId)
        {
            return await _reactionService.ReactUnreact(reactedObj, reaction, userId);                        
        }        
    }
}
