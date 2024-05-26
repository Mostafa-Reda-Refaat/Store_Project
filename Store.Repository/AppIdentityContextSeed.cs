using Microsoft.AspNetCore.Identity;
using Store.Data.Entities.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository
{
    public class AppIdentityContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if(!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Mostafa Reda",
                    Email = "mostafa@gmail.com",
                    UserName = "mostafareda",
                    Address = new Address
                    {
                        FristName = "Mostafa",
                        LastName = "Reda",
                        City = "Maadi",
                        State = "Cairo",
                        Street = "77",
                        ZipCode = "12345"
                    }
                };

                await userManager.CreateAsync(user, "Password123!");
            }
        }
    }
}
