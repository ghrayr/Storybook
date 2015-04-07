using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

using Storybook.DAL.DBContext;
using Storybook.DataModel.Models;


namespace Storybook.DAL.Managers
{
    public class AccountManager : ManagerBase
    {
        /// <summary>
        /// Creates ClaimsIdentity representing the user
        /// </summary>
        /// <param name="user">user to create</param>
        /// <returns>Task ClaimsIdentity asynchronously</returns>
        public static async Task<ClaimsIdentity> GenerateUserIdentityAsync(User user)
        {
            using (var db = new StorybookContext())
            {
                return
                    await new UserManager<User, int>(new UserStore<User, Role, int, UserLogin, UserRole, UserClaim>(db))
                        .CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            }
        }

        /// <summary>
        /// Returns a user with the specified username and password or null if there is no match.
        /// </summary>
        /// <param name="userName">UserName</param>
        /// <param name="password">Password</param>
        /// <returns>Task User asynchronously</returns>
        public static async Task<User> FindAsync(string userName, string password)
        {
            using (var db = new StorybookContext())
            {
                return
                    await new UserManager<User, int>(new UserStore<User, Role, int, UserLogin, UserRole, UserClaim>(db))
                        .FindAsync(userName, password);
            }
        }

        /// <summary>
        /// Creates a user with the given password
        /// </summary>
        /// <param name="user">User to Create</param>
        /// <param name="password">Password</param>
        /// <returns>Task IdentityResult asynchronously</returns>
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
