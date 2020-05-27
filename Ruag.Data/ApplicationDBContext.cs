using Microsoft.AspNet.Identity.EntityFramework;
using Ruag.Common;
using Ruag.Data.EnitityTypeConfiguration;
using Ruag.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruag.Data
{
    public class AppDBContext: IdentityDbContext<AppUser>
    {
        
        public AppDBContext()
            : base("name=DefaultConnection")
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<AppDBContext, Ruag.Data.Migrations.Configuration>());
            AppLogger.Instance.LogEnd(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<OrgRole> OrgRoles { get; set; }
       
        public static AppDBContext Create()
        {
            return new AppDBContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new EmployeeTypeConfig());
            modelBuilder.Configurations.Add(new OrgRoleTypeConfig());
        }


    }
}
