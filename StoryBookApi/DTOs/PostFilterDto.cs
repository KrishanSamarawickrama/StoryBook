using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoryBookApi.DTOs
{
    public class PostFilterDto: PaginationDto
    {
        public string EditorId { get; set; }

    }
}
