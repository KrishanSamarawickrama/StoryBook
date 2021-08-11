using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoryBookApi.DTOs
{
    public class EditorDto
    {
        public string EditorId { get; set; }
        public string EditorName { get; set; }
        public string Email { get; set; }
        public bool IsFlowing { get; set; }
    }
}
