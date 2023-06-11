using DotNetCore.Entities;
using DotNetCore.Models;

namespace BlazorCore.Areas.Interfaces
{
    public interface IReactedObjService
    {
        public Task<bool> ReactUnreact(ReactedObject reactedObj, string reaction, string userId);
    }
}
