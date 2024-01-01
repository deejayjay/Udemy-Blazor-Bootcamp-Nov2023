using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tangy_Common;
using Tangy_DataAccess.Data;
using TangyWeb_Server.Service.IService;

namespace TangyWeb_Server.Service
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public DbInitializer(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext db)
        {
            // Injecting UserManager and RoleManager
            _userManager = userManager;
            _roleManager = roleManager;

            // Injecting ApplicationDbContext
            _db = db;
        }

        public async Task Initialize()
        {
            try
            {
                // If there are any pending migrations, apply them
                if ((await _db.Database.GetPendingMigrationsAsync()).Count() > 0)
                {
                    await _db.Database.MigrateAsync();
                }

                // If the "Admin" role doesn't exist, create both Admin and Customer roles
                if (!await _roleManager.RoleExistsAsync(Sd.Role_Admin))
                {
                    _roleManager.CreateAsync(new IdentityRole(Sd.Role_Admin)).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new IdentityRole(Sd.Role_Customer)).GetAwaiter().GetResult();
                    // OR
                    // await _roleManager.CreateAsync(new IdentityRole(Sd.Role_Admin));
                }
                else
                {
                    return;
                }

                // Create the Admin user
                IdentityUser user = new()
                { 
                    UserName = "deon@dotnetmastery.com",
                    Email = "deon@dotnetmastery.com",
                    EmailConfirmed = true
                };
                await _userManager.CreateAsync(user, "Test@1234");
                await _userManager.AddToRoleAsync(user, Sd.Role_Admin);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
