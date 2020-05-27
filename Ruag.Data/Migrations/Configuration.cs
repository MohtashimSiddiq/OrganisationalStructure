namespace Ruag.Data.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Ruag.Common;
    using Ruag.Common.Enums;
    using Ruag.Data.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Net.Http.Headers;

    internal sealed class Configuration : DbMigrationsConfiguration<Ruag.Data.AppDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Ruag.Data.AppDBContext context)
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            try
            {
                var manager = new UserManager<AppUser>(new UserStore<AppUser>(new AppDBContext()));

                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new AppDBContext()));
                if (manager.FindByName("SuperUser") == null)
                {
                    var user = new AppUser()
                    {
                        FirstName = "Mohtashim",
                        LastName = "Siddiq",
                        UserName = "SuperUser",
                        EmailConfirmed = true,
                        JoinDate = DateTime.Today,

                    };

                    manager.Create(user, "P@ssw0rd");
                    if (roleManager.Roles.Count() == 0)
                    {
                        roleManager.Create(new IdentityRole { Name = "SuperAdmin" });
                        roleManager.Create(new IdentityRole { Name = "Admin" });
                        roleManager.Create(new IdentityRole { Name = "User" });
                    }
                    var adminUser = manager.FindByName("SuperUser");
                    manager.AddToRoles(adminUser.Id, new string[] { "SuperAdmin", "Admin" });
                }


            }
            catch (DbEntityValidationException ex)
            {
                foreach (var errors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in errors.ValidationErrors)
                    {
                        // get the error message 
                        AppLogger.Instance.Log(eLogType.Error, validationError.ErrorMessage);
                    }
                }
            }
            AppLogger.Instance.LogEnd(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);

        }
    }
}
