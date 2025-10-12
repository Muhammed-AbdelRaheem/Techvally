using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Domain.Models.NotificationHandlerVM;

public class DeleteUserByProviderVM : INotification {

  
    public string UserId { get; set; }
    
    public Provider Providers { get; set; }
    
    public String ControllerName { get; set; }
    
    public String ActionName { get; set; }
    
    public bool Callback { get; set; }
    
    public bool Revoke_From_Server { get; set; }

}


