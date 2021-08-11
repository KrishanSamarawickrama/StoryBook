using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoryBookApi.Entities
{
    public class UserFollow
    {
        public string UserId { get; set; }
        public string FollowUserId { get; set; }
        public UserInfo User { get; set; }
        public UserInfo FollowUser { get; set; }

    }
}
