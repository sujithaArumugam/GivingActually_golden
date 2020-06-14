using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace GivingActually_May.Models
{
    public class LoginModel
    {
        [Display(Name = "Username")]
        [Required]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [Required]
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public DateTime LastLoginTime { get; set; }
        public string Location { get; set; }
        public string latitide { get; set; }
        public string longitude { get; set; }
    }
    public class FacebookLoginModel
    {
        public string uid { get; set; }
        public string accessToken { get; set; }
    }
    public class UserModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
        public bool IsAcLocked { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public bool IsSpamUser { get; set; }
        public string DisplayName { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public DateTime? CurrentLoginDate { get; set; }

    }
    public class IpInfo
    {

        [JsonProperty("ip")]
        public string Ip { get; set; }

        [JsonProperty("hostname")]
        public string Hostname { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("loc")]
        public string Loc { get; set; }

        [JsonProperty("org")]
        public string Org { get; set; }

        [JsonProperty("postal")]
        public string Postal { get; set; }
    }
    public class RegisterModel
    {
        [Display(Name = "UserName")]
        [Required]
        public string UserName { get; set; }


        [Display(Name = "DisplayName")]
        [Required]
        public string DPName { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [Required]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Required]
        public string ConfirmPassword { get; set; }

        [Display(Name = "One Time Password")]
        [Required]
        public string SecurityToken { get; set; }
        public bool IsSecurityTokenGenerated { get; set; }
        public bool UserAlreadyExists { get; set; }
        public bool IsResitered { get; set; }
        public string BasedOut { get; set; }
    }

    public class UserDetails
    {
        public UserDetails()
        {
            Users = new List<UserModel>();
        }
        public List<UserModel> Users { get; set; }
    }
}
