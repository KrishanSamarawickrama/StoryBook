using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StoryBookApi.DTOs;
using StoryBookApi.Entities;
using StoryBookApi.Helpers;

namespace StoryBookApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EditorController : ControllerBase
    {
        private readonly ILogger<StoryController> logger;
        private readonly ApplicationDbContext db;

        public EditorController(ILogger<StoryController> logger,
            ApplicationDbContext Db)
        {
            this.logger = logger;
            db = Db;
        }

        [HttpGet("list")]
        public async Task<ActionResult<List<EditorDto>>> ListWriters([FromQuery] PaginationDto paginationDto)
        {
            var currUserId = HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var currUser = await db.Users.SingleAsync(x => x.UserId == currUserId);
            var fllowing = await db.UserFollows.Where(x => x.UserId == currUser.Id).ToListAsync();

            var writers = await db.Users.Where(x => x.IsEditor == "TRUE")
                .Paginate(paginationDto).ToListAsync();

            HttpContext.InsertParametersPaginationInHeader(writers);

            return writers.Select(u => new EditorDto
            {
                EditorId = u.UserId,
                EditorName = $"{u.FirstName} {u.LastName}",
                Email = u.EmailAddress,
                IsFlowing = fllowing.Any(a => a.FollowUserId == u.Id)
            }).OrderBy(o => o.EditorName).ToList();

        }

        [HttpPost("followWriter")]
        public async Task<ActionResult> FollowWriter([FromBody] string userId)
        {
            var currUserId = HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var currUser = await db.Users.SingleAsync(x => x.UserId == currUserId);
            var writer = await db.Users.SingleOrDefaultAsync(x => x.UserId == userId);
            if (writer == null) return BadRequest();

            db.UserFollows.Add(new() { UserId = currUser.Id, FollowUserId = writer.Id });
            await db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("unFollowWriter")]
        public async Task<ActionResult> UnFollowWriter([FromBody] string userId)
        {
            var currUserId = HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var currUser = await db.Users.SingleAsync(x => x.UserId == currUserId);
            var writer = await db.Users.SingleOrDefaultAsync(x => x.UserId == userId);

            var userFollow = await db.UserFollows.SingleOrDefaultAsync(x => x.FollowUserId == writer.Id && x.UserId == currUser.Id);
            if (writer == null) return BadRequest();

            db.UserFollows.Remove(userFollow);
            await db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("searchByName")]
        public async Task<ActionResult<List<EditorSearchDto>>> SearchByName([FromBody] string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return new List<EditorSearchDto>();

            var currUserId = HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var currUser = await db.Users.SingleAsync(x => x.UserId == currUserId);

            return await db.Users.Where(x => x.IsEditor == "TRUE" && (x.FirstName.Contains(value) || x.LastName.Contains(value)))
                .Join(db.UserFollows , u => u.Id, f => f.FollowUserId, (u,f) => new { u, f })
                .Where(s => s.f.UserId == currUser.Id)
                .Select(x => new EditorSearchDto
                {
                    EditorId = x.u.UserId,
                    EditorName = $"{x.u.FirstName} {x.u.LastName}"
                })
                .Take(5)
                .ToListAsync();

        }

    }
}
