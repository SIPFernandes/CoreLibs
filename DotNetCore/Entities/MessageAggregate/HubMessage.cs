using System.Text.Json.Serialization;
using DotNetCore.Helpers;

namespace DotNetCore.Entities.MessageAggregate
{
    public class HubMessage : BaseEntity
    {        
        public object Obj { get; set; }        
        public string Type { get; set; }
        public HubMessage(object obj, string type, string creatorId) : base(creatorId)
        {
            Obj = obj;
            Type = type;
        }
        
        public T? GetObj<T>() where T : class
        {
            var obj = Obj.ToString();

            return string.IsNullOrEmpty(obj)
                ? null
                : obj.Deserialize<T>();
        }
    }
}
