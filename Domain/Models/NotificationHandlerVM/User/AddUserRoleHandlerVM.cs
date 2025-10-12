using MediatR;

namespace Domain.Models.NotificationHandlerVM; 

public class AddUserRoleHandlerVM : INotification {
    public string UserId { get; set; }
}
