using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Storybook.DataModel.Models;
using Storybook.DAL.DBContext;

namespace Storybook.DAL.Managers
{
    public class AccountManager : ManagerBase
    {
        public static async Task<ClaimsIdentity> GenerateUserIdentityAsync(User user)
        {
            using (var db = new StorybookContext())
            {
                return
                    await new UserManager<User, int>(new UserStore<User, Role, int, UserLogin, UserRole, UserClaim>(db))
                        .CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            }
        }

        public static async Task<User> FindAsync(string userName, string password)
        {
            using (var db = new StorybookContext())
            {
                return
                    await new UserManager<User, int>(new UserStore<User, Role, int, UserLogin, UserRole, UserClaim>(db))
                        .FindAsync(userName, password);
            }
        }

        public static async Task<IdentityResult> CreateAsync(User user, string password)
        {
            using (var db = new StorybookContext())
            {
                return
                    await new UserManager<User, int>(new UserStore<User, Role, int, UserLogin, UserRole, UserClaim>(db))
                        .CreateAsync(user, password);
            }
        }
    }
}
