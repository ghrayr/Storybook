using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Storybook.DataModel.Models
{
    [Table("Users")]
    public class User : IdentityUser<int, UserLogin, UserRole, UserClaim>
    {
        [StringLength(256, MinimumLength = 3)]
        public string Name { get; set; }

        public virtual ICollection<Group> Groups { get; set; }
    }

    public class UserRole : IdentityUserRole<int>
    {
    }

    public class UserClaim : IdentityUserClaim<int>
    {
    }

    public class UserLogin : IdentityUserLogin<int>
    {
    }

    public class Role : IdentityRole<int, UserRole>
    {
        public Role() { }
        public Role(string name) { Name = name; }
    }
}
