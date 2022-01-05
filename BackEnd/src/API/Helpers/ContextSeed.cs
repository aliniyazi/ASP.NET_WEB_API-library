using API.Common;
using API.DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace API.DataAccess.Seeders
{
    public class ContextSeed
    {
        public static void MigrateDataBase(IConfiguration configuration)
        {
            string connString = configuration.GetConnectionString("DefaultConnection");
            DbContextOptionsBuilder<DBContext> contextOptionsBuilder = new DbContextOptionsBuilder<DBContext>();
            contextOptionsBuilder.UseSqlServer(connString);
            DBContext context = new DBContext(contextOptionsBuilder.Options);

            context.Database.Migrate();
        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            if (!roleManager.RoleExistsAsync(Roles.User).GetAwaiter().GetResult())
            {
                roleManager.CreateAsync(new IdentityRole(Roles.User)).GetAwaiter().GetResult();
            }
            if (!roleManager.RoleExistsAsync(Roles.Librarian).GetAwaiter().GetResult())
            {
                roleManager.CreateAsync(new IdentityRole(Roles.Librarian)).GetAwaiter().GetResult();
            }
            if (!roleManager.RoleExistsAsync(Roles.Admin).GetAwaiter().GetResult())
            {
                roleManager.CreateAsync(new IdentityRole(Roles.Admin)).GetAwaiter().GetResult();
            }
        }

        public static void SeedUser(UserManager<User> userManager)
        {
            User user = new User
            {
                FirstName = "FirstUser",
                Lastname = "Userstan",
                Email = "user@gmail.com",
                UserName = "user@gmail.com",
                PhoneNumber = "0894488533",
                Address = new Address
                {
                    Country = "userStan",
                    City = "userstvo",
                    StreetName = "userStreet",
                    StreetNumber = "34"
                }
            };

            if (userManager.Users.ContainsAsync(user).GetAwaiter().GetResult() == false)
            {
                string confirmEmailToken = userManager.GenerateEmailConfirmationTokenAsync(user).GetAwaiter().GetResult();
                userManager.ConfirmEmailAsync(user, confirmEmailToken).GetAwaiter().GetResult();

                IdentityResult result = userManager.CreateAsync(user, "Password1!").GetAwaiter().GetResult();
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, Roles.User).GetAwaiter().GetResult();
                }
            }
        }

        public static void SeedLibrarian(UserManager<User> userManager)
        {
            User librarian = new User
            {
                FirstName = "FirstLib",
                Lastname = "Librarianin",
                Email = "bibliothecary@gmail.com",
                UserName = "bibliothecary@gmail.com",
                PhoneNumber = "0896488533",
                Address = new Address
                {
                    Country = "librarianinstan",
                    City = "libnitza",
                    StreetName = "librarianSt",
                    StreetNumber = "24"
                }
            };

            if (userManager.Users.ContainsAsync(librarian).GetAwaiter().GetResult() == false)
            {
                string confirmEmailToken = userManager.GenerateEmailConfirmationTokenAsync(librarian).GetAwaiter().GetResult();
                userManager.ConfirmEmailAsync(librarian, confirmEmailToken).GetAwaiter().GetResult();

                IdentityResult result = userManager.CreateAsync(librarian, "Password1!").GetAwaiter().GetResult();
                if (result.Succeeded)
                {
                    string[] roles = new string[] {Roles.User,Roles.Librarian};
                    userManager.AddToRolesAsync(librarian, roles).GetAwaiter().GetResult();
                }
            }
        }

        public static void SeedAdmin(UserManager<User> userManager)
        {
            User admin = new User
            {
                FirstName = "Admin",
                Lastname = "Adminov",
                Email = "admin@gmail.com",
                UserName = "admin@gmail.com",
                PhoneNumber = "0894488533",
                Address = new Address
                {
                    Country = "Administan",
                    City = "Adminovo",
                    StreetName = "AdminStret",
                    StreetNumber = "35"
                }
            };

            if (userManager.Users.ContainsAsync(admin).GetAwaiter().GetResult() == false)
            {
                string confirmEmailToken = userManager.GenerateEmailConfirmationTokenAsync(admin).GetAwaiter().GetResult();
                userManager.ConfirmEmailAsync(admin, confirmEmailToken).GetAwaiter().GetResult();

                IdentityResult result = userManager.CreateAsync(admin, "Password1!").GetAwaiter().GetResult();
                if (result.Succeeded)
                {
                    string[] roles = new string[] { Roles.User, Roles.Librarian, Roles.Admin };
                    userManager.AddToRolesAsync(admin, roles).GetAwaiter().GetResult();
                }
            }
        }
    }
}
