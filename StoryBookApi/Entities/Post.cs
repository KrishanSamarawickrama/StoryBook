using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StoryBookApi.Entities
{
    public class Post 
    {
        public string PostId { get; set; }

        public string UserId { get; set; }

        public DateTime Date { get; set; }

        [Required, StringLength(500)]
        public string PostContent { get; set; }
    }
}
