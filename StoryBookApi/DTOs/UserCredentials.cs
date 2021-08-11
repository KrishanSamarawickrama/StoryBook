using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StoryBookApi.DTOs
{
    public class UserCredentials
    {
        [Required]
        public string UserId { get; set; }       

        [Required]
        public string Password { get; set; }
    }
}
