using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class TokenApiViewModel
    {

        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public string? Picture { get; set; }
        public string? CountryCode { get; set; }
        public string? Mobile { get; set; }

        public Gender? Gender { get; set; }

        [DefaultValue(false)]
        public bool HasPassword { get; set; }
        public UserType UserType { get; set; }
        public string? Id { get; set; }
        public string? Language { get; set; }
        [JsonIgnore]
        public bool Active { get; set; }

        public int? RegistrationId { get; set; }

        public bool EmailVerified { get; set; }
        public bool CompleteRegistration { get; set; }


        [DefaultValue(false)]
        public bool NotificationEnabled { get; set; }
        public int? Age { get; set; }
        public double? Weight { get; set; }

    }

}
