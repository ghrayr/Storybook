using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using Storybook.DataModel.Models;

namespace Storybook.DAL.DBContext
{
    public class StorybookContext : IdentityDbContext<User, Role, int, UserLogin, UserRole, UserClaim>
    {
        public DbSet<Group> Groups { get; set; }
        public DbSet<Story> Stories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<User>().ToTable("Users", "dbo")
                .HasMany(c => c.Groups).WithMany(i => i.Users)
                .Map(t => t.MapLeftKey("UserId").MapRightKey("GroupId")
                    .ToTable("UsersGroups", "dbo"));
            
            modelBuilder.Entity<Role>().ToTable("Roles", "dbo");
            modelBuilder.Entity<UserRole>().ToTable("UsersRoles", "dbo");
            modelBuilder.Entity<UserClaim>().ToTable("UserClaims", "dbo");
            modelBuilder.Entity<UserLogin>().ToTable("UserLogins", "dbo");

            modelBuilder.Entity<Group>().ToTable("Groups", "dbo")
                .HasMany(c => c.Stories).WithMany(i => i.Groups)
                .Map(t => t.MapLeftKey("GroupId").MapRightKey("StoryId")
                    .ToTable("StoriesGroups", "dbo"));
                    
            modelBuilder.Entity<Story>().ToTable("Stories", "dbo")
                .HasMany(c => c.Groups).WithMany(i => i.Stories)
                .Map(t => t.MapLeftKey("StoryId").MapRightKey("GroupId")
                    .ToTable("StoriesGroups", "dbo"));
        }
    }
}
