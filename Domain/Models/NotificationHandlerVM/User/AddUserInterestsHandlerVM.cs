using MediatR;

namespace Domain.Models.NotificationHandlerVM; 

public class AddUserInterestsHandlerVM : INotification {
    public string UserId { get; set; }
}
