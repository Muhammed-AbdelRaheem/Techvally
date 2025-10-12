using System.ComponentModel;
using MediatR;

namespace Domain.Models.NotificationHandlerVM; 

public class UpdateUserHandlerVM : INotification {
    public ApplicationUser User { get; set; }
    
    [DefaultValue(false)]
    public bool AddRole { get; set; }
}
