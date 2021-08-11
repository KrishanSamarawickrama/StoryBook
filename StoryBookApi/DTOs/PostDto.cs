using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StoryBookApi.DTOs
{
    public class PostDto
    {
        public string PostId { get; set; }

        public string UserId { get; set; }

        public string EditorName { get; set; }

        public DateTime Date { get; set; }

        public string PostContent { get; set; }
    }
}
