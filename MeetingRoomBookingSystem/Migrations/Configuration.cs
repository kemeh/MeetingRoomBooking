using MeetingRoomBookingSystem.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MeetingRoomBookingSystem.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MeetingRoomBookingSystem.Models.MeetingRoomBookingSystemDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(MeetingRoomBookingSystem.Models.MeetingRoomBookingSystemDbContext context)
        {
            if (!context.Roles.Any())
            {
                this.CreateRole(context, "Admin");
                this.CreateRole(context, "User");
            }

            if (!context.Users.Any())
            {
                this.CreateUser(context, "First", "Second", "admin@admin.com", "123456", null);
                this.SetRoleToUser(context, "admin@admin.com", "Admin");
            }
        }

        private void SetRoleToUser(MeetingRoomBookingSystemDbContext context, string email, string role)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            var user = context.Users.Where(u => u.Email == email).First();
            var result = userManager.AddToRole(user.Id, role);

            if (!result.Succeeded)
            {
                throw new Exception(string.Join(";", result.Errors));
            }
        }

        private void CreateUser(MeetingRoomBookingSystemDbContext context, string firstName, string lastName, string email, string password, int? officeId)
        {
            // Create User Manager
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            // Ser User manager password validator
            userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireDigit = false,
                RequireLowercase = false,
                RequireNonLetterOrDigit = false,
                RequireUppercase = false,
            };

            //Create user object
            var admin = new ApplicationUser
            {
                UserName = email,
                FirstName = firstName,
                LastName = lastName,
                IsActive = true,
                Email = email,
                OfficeId = null
            };

            //Create user
            var result = userManager.Create(admin, password);

            //Validate result
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(";", result.Errors));
            }

        }
        //  This method will be called after migrating to the latest version.

        //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
        //  to avoid creating duplicate seed data. E.g.
        //
        //    context.People.AddOrUpdate(
        //      p => p.FullName,
        //      new Person { FullName = "Andrew Peters" },
        //      new Person { FullName = "Brice Lambson" },
        //      new Person { FullName = "Rowan Miller" }
        //    );
        //
    

        private void CreateRole(MeetingRoomBookingSystemDbContext context, string roleName)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            var result = roleManager.Create(new IdentityRole(roleName));

            if (!result.Succeeded)
            {
                throw new Exception(string.Join(";", result.Errors));
            }
        }
    }
}
