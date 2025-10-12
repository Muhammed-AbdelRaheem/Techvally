using MediatR;

namespace Domain.Models.NotificationHandlerVM; 

public class ForgotPasswordHandlerVM: INotification {
    public ApplicationUser User { get; set; }
    public string Email { get; set; }
    public string language { get; set; }

}
