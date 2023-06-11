namespace BlazorCore.Data.Models
{
    public class AppUserModel
    {
        public string Id { get; private set; }
        public string Email { get; protected set; }
        public string? FirstName { get; protected set; }
        public string? LastName { get; protected set; }
        public DateTime? NotificationsLastVisit { get; set; }        

        public AppUserModel(string id, string email, string? firsName, string? lastName) 
        { 
            Id = id;
            Email = email;
            FirstName = firsName;
            LastName = lastName;
        }
    }
}
