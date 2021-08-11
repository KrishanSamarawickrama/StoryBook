using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using StoryBookApi.Entities;

namespace StoryBookApi
{
    public static class ApplicationDbContextSeed
    {
        public static void SeedDefaultUserAsync(UserManager<UserInfo> userManager)
        {
            var defaultUser1 = new UserInfo
            {
                UserId = "shakes",
                UserName = "shakes",
                FirstName = "William",
                LastName = "Shakespear",
                Email = "will@theshakes.mnd",
                EmailAddress = "will@theshakes.mnd",
                UserRole = UserRole.Moderator
            };

            var u1 = userManager.Users.SingleOrDefault(u => u.UserName == defaultUser1.UserName);
            userManager.DeleteAsync(u1).Wait();

            if (userManager.Users.All(u => u.UserName != defaultUser1.UserName))
            {
                userManager.CreateAsync(defaultUser1, "Abc@1255376235").Wait();
            }

            var defaultUser2 = new UserInfo
            {
                UserId = "maups",
                UserName = "maups",
                FirstName = "Guy De",
                LastName = "Maupesant",
                Email = "mau@paro.mnd",
                EmailAddress = "mau@paro.mnd",
                UserRole = UserRole.NormalUser
            };

            var u2 = userManager.Users.SingleOrDefault(u => u.UserName == defaultUser2.UserName);
            userManager.DeleteAsync(u2).Wait();

            if (userManager.Users.All(u => u.UserName != defaultUser2.UserName))
            {
                userManager.CreateAsync(defaultUser2, "Abc@1255376235").Wait();
            }

            var defaultUser3 = new UserInfo
            {
                UserId = "mws",
                UserName = "mws",
                FirstName = "Martin",
                LastName = "Wickramasinghe",
                Email = "simon@kaballana.mnd",
                EmailAddress = "simon@kaballana.mnd",
                UserRole = UserRole.NormalUser,
                IsEditor = "TRUE",
                IsBanned = "FALSE"
            };

            var u3 = userManager.Users.SingleOrDefault(u => u.UserName == defaultUser3.UserName);
            userManager.DeleteAsync(u3).Wait();

            if (userManager.Users.All(u => u.UserName != defaultUser3.UserName))
            {
                userManager.CreateAsync(defaultUser3, "Abc@1255376235").Wait();
            }
            
        }
    }
}
