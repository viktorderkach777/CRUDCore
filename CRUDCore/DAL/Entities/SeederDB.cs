﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDCore.DAL.Entities
{
    public class SeederDB
    {
        public static void SeedData(UserManager<DbUser> userManager,
                   RoleManager<DbRole> roleManager)
        {
            var email = "admin@gmail.com";
            var roleName = "Admin";
            var findUser = userManager.FindByEmailAsync(email).Result;
            if (findUser == null)
            {
                var user = new DbUser
                {
                    Email = email,
                    UserName = email,
                    Image = "https://cdn.pixabay.com/photo/2017/07/28/23/34/fantasy-picture-2550222_960_720.jpg",
                    Age = 30,
                    Phone = "+380957476156",
                    Description = "PHP programmer"
                };
                var result = userManager.CreateAsync(user, "Qwerty1-").Result;

                var roleresult = roleManager.CreateAsync(new DbRole
                {
                    Name = roleName

                }).Result;

                result = userManager.AddToRoleAsync(user, roleName).Result;
            }
        }
        
        public static void SeedDataByAS(IServiceProvider services)
        {
            using (var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var manager = scope.ServiceProvider.GetRequiredService<UserManager<DbUser>>();
                var managerRole = scope.ServiceProvider.GetRequiredService<RoleManager<DbRole>>();
                SeederDB.SeedData(manager, managerRole);
            }
        }
    }

}
