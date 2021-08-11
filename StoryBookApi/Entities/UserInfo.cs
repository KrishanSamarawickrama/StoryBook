using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace StoryBookApi.Entities
{
    public class UserInfo : IdentityUser
    {
        [Required, StringLength(12)]
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public UserRole UserRole { get; set; }
        public string IsEditor { get; set; } = "FALSE";
        public string IsBanned { get; set; } = "FALSE";
    }

    public enum UserRole
    {
        NormalUser = 'U',
        Moderator = 'M'
    }

}
