using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StoryBookApi.DTOs
{
    public class PostCreationDto
    {
        [Required, StringLength(500)]
        public string PostContent { get; set; }
    }
}
