using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StoryBookApi.DTOs;
using StoryBookApi.Entities;
using StoryBookApi.Helpers;
using Microsoft.EntityFrameworkCore;

namespace StoryBookApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<UserInfo> userManager;
        private readonly SignInManager<UserInfo> signInManager;
        private readonly IConfiguration configuration;
        private readonly ApplicationDbContext db;

        public AccountsController(
            UserManager<UserInfo> userManager,
            SignInManager<UserInfo> signInManager,
            IConfiguration configuration,
            ApplicationDbContext DB
            )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.db = DB;
        }

        [HttpGet("list")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsModerator")]
        public async Task<ActionResult<List<UserDto>>> GetUsers([FromQuery] PaginationDto paginationDto)
        {
            var users = await db.Users.OrderBy(o => o.UserName).Paginate(paginationDto).ToListAsync();

            HttpContext.InsertParametersPaginationInHeader(users);

            return users.Adapt<List<UserDto>>();
        }

        [HttpPost("makeWriter")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsModerator")]
        public async Task<ActionResult> MakeWriter([FromBody] string userId)
        {
            var user = await userManager.FindByNameAsync(userId);
            user.IsEditor = "TRUE";
            await db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("removeWriter")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsModerator")]
        public async Task<ActionResult> RemoveWriter([FromBody] string userId)
        {
            var user = await userManager.FindByNameAsync(userId);
            user.IsEditor = "FALSE";
            await db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("banUser")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsModerator")]
        public async Task<ActionResult> Ban([FromBody] string userId)
        {
            var user = await userManager.FindByNameAsync(userId);
            user.IsBanned = "TRUE";
            await db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("unBanUser")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsModerator")]
        public async Task<ActionResult> UnBan([FromBody] string userId)
        {
            var user = await userManager.FindByNameAsync(userId);
            user.IsBanned = "FALSE";
            await db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("create")]
        public async Task<ActionResult<AuthResponse>> Create([FromBody] UserCreationDto credentials)
        {
            var user = new UserInfo 
            { 
                UserId= credentials.UserId,
                UserName = credentials.UserId, 
                Email = credentials.EmailAddress,
                FirstName = credentials.FirstName,
                LastName = credentials.LastName,
                EmailAddress = credentials.EmailAddress,
                UserRole = UserRole.NormalUser
            };

            var result = await userManager.CreateAsync(user, credentials.Password);

            if (result.Succeeded)
            {
                return await BuildToken(new UserCredentials { UserId = credentials.UserId , Password = credentials.Password });
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> LogIn([FromBody] UserCredentials credentials)
        {
            var result = await signInManager.PasswordSignInAsync(credentials.UserId, credentials.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return await BuildToken(credentials);
            }
            else
            {
                return BadRequest("Incorrect Login.");
            }
        }

        private async Task<AuthResponse> BuildToken(UserCredentials credentials)
        {
            var user = await userManager.FindByNameAsync(credentials.UserId);

            Claim userRole = new("userRole", ((char)user.UserRole).ToString());
            if (user.UserRole == UserRole.NormalUser && user.IsEditor == "TRUE")
            {
                userRole = new("userRole", "W");
            }  

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, credentials.UserId),
                new Claim("email",user.EmailAddress),
                userRole,
                new Claim("isEditor",user.IsEditor),
                new Claim("isBanned",user.IsBanned),
            };

            //var claimsDB = await userManager.GetClaimsAsync(user);
            //claims.AddRange(claimsDB);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["keyJwt"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddMinutes(60);

            var token = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiration, signingCredentials: creds);

            return new AuthResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }
    }
}
