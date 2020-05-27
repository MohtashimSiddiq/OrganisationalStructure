using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Ruag.Common;
using Ruag.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ruag.Data
{
    public class AppUserManager : UserManager<AppUser>
    {
        public AppUserManager(IUserStore<AppUser> store)
            : base(store)
        {
        }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(AppUser user, string authenticationType)
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            var userIdentity = await this.CreateIdentityAsync(user, authenticationType);
            // Add custom user claims here
            AppLogger.Instance.LogEnd(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            return userIdentity;
        }

        public static AppUserManager Create(IdentityFactoryOptions<AppUserManager> options, IOwinContext context)
        {
            AppLogger.Instance.LogBegin("AppUserManager", System.Reflection.MethodInfo.GetCurrentMethod().Name);
            var appDbContext = context.Get<AppDBContext>();
            var appUserManager = new AppUserManager(new UserStore<AppUser>(appDbContext));
            AppLogger.Instance.LogEnd("AppUserManager", System.Reflection.MethodInfo.GetCurrentMethod().Name);
            return appUserManager;
        }
    }
}
