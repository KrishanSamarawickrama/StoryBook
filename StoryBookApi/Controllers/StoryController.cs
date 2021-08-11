using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StoryBookApi.DTOs;
using StoryBookApi.Entities;
using StoryBookApi.Helpers;

namespace StoryBookApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class StoryController : ControllerBase
    {
        private readonly ILogger<StoryController> logger;
        private readonly ApplicationDbContext db;

        public StoryController(ILogger<StoryController> logger,
            ApplicationDbContext Db)
        {
            this.logger = logger;
            db = Db;
        }

        [HttpGet("list")]
        public async Task<ActionResult<List<PostDto>>> Get([FromQuery] PostFilterDto request)
        {
            var currUserId = HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var currUser = await db.Users.SingleAsync(x => x.UserId == currUserId);

            string editorId = "";
            if (!string.IsNullOrEmpty(request.EditorId))
            {
                var editor = await db.Users.SingleAsync(x => x.UserId == request.EditorId);
                editorId = editor?.Id;
            }

            var posts = await db.Posts
                .Join(db.Users, post => post.UserId, user => user.UserId, (post, user) => new { post, user })
                .Join(db.UserFollows, d => d.user.Id, f => f.FollowUserId, (d, f) => new { d, f })
                .Where(w=> w.f.UserId == currUser.Id && (string.IsNullOrEmpty(editorId) || w.f.FollowUserId == editorId))
                .Select(s => new PostDto
                {
                    PostId = s.d.post.PostId,
                    UserId = s.d.post.UserId,
                    EditorName = $"{s.d.user.FirstName} {s.d.user.LastName}",
                    PostContent = s.d.post.PostContent,
                    Date = s.d.post.Date
                })
                .OrderByDescending(o => o.Date).Paginate(request).ToListAsync();

            HttpContext.InsertParametersPaginationInHeader(posts);

            return posts;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsEditor")]
        public async Task<ActionResult> Post([FromBody] PostCreationDto postDto)
        {
            Post post = new()
            {
                PostId = Guid.NewGuid().ToString(),
                PostContent = postDto.PostContent,
                Date = DateTime.Now,
                UserId = HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)
            };
            db.Posts.Add(post);

            (int single, int pairs) = VowelCounter.VowelProcessor(postDto.PostContent);
            var stat = db.StatVowels.SingleOrDefault(x => x.Date.Date == DateTime.Now.Date);
            if (stat != null)
            {
                stat.SingleVowelCount += single;
                stat.PairVowelCount += pairs;
                stat.TotalWordCount += postDto.PostContent.Split(' ')?.Length ?? 0;
                db.Entry(stat).State = EntityState.Modified;
            }
            else
            {
                stat = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    Date = DateTime.Now,
                    SingleVowelCount = single,
                    PairVowelCount = pairs,
                    TotalWordCount = postDto.PostContent.Split(' ')?.Length ?? 0,
                };
                db.StatVowels.Add(stat);
            }

            await db.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("listStatData")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsEditor")]
        public async Task<ActionResult<List<StatVowels>>> ListStatData([FromQuery] PaginationDto paginationDto)
        {
            var stats = await db.StatVowels
                .OrderByDescending(o => o.Date)
                .Paginate(paginationDto).ToListAsync();

            HttpContext.InsertParametersPaginationInHeader(stats);

            return stats;
        }
    }
}
