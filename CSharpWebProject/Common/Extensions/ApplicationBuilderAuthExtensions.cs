using CSharpWebProject.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSharpWebProject.Common.Extensions
{
    public static class ApplicationBuilderAuthExtensions
    {
        private const string DefaultAdminRole = "Administrator";
        private const string DefaultAdminName = "admin@admin.admin";
        private const string DefaultAdminEmail = "admin@admin.admin";
        private const string DefaultAdminPassword = "asdasd";

        private static readonly IdentityRole[] roles =
        {
            new IdentityRole(DefaultAdminRole)
        };

        public static async void SeedDatabase(this IApplicationBuilder app)
        {
            var serviceFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            var scope = serviceFactory.CreateScope();
            using (scope)
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role.Name))
                    {
                        await roleManager.CreateAsync(role);
                    }
                }

                var user = await userManager.FindByNameAsync(DefaultAdminName);
                if (user == null)
                {
                    user = new User()
                    {
                        UserName = DefaultAdminName,
                        Email = DefaultAdminEmail,
                        SecurityStamp = Guid.NewGuid().ToString()
                    };

                    await userManager.CreateAsync(user, DefaultAdminPassword);
                    await userManager.AddToRoleAsync(user, roles[0].Name);
                }
            }
        }
    }
}
