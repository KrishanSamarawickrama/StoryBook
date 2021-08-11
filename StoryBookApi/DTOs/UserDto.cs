using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StoryBookApi.Entities;

namespace StoryBookApi.DTOs
{
    public class UserDto
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public UserRole UserRole { get; set; }
        public string IsEditor { get; set; }
        public string IsBanned { get; set; }
    }
}
