using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Storybook.DataModel.Models;

namespace Storybook.DAL.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<Storybook.DAL.DBContext.StorybookContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Storybook.DAL.DBContext.StorybookContext";
        }

        protected override void Seed(Storybook.DAL.DBContext.StorybookContext context)
        {
            //  This method will be called after migrating to the latest version.

            if (context.Users.Any()) return;

            #region Users population

            var userManager = new UserManager<User, int>(new UserStore<User, Role, int, UserLogin, UserRole, UserClaim>(context));
            userManager.Create(new User { Name = "Jim L. Ellis", UserName = "Jim" }, "qwerty123");
            userManager.Create(new User { Name = "Rose Klein", UserName = "Rose" }, "qwerty123");
            userManager.Create(new User { Name = "Andrew Peters", UserName = "Andrew" }, "qwerty123");
            //userManager.Create(new User { Name = "Jerry F. Ellison", UserName = "Jerry" }, "qwerty123");
            //userManager.Create(new User { Name = "Brice Lambson", UserName = "Brice" }, "qwerty123");
            //userManager.Create(new User { Name = "Richard K. Snyder", UserName = "Richard" }, "qwerty123");
            //userManager.Create(new User { Name = "Rowan Miller", UserName = "Rowan" }, "qwerty123");
            #endregion

            #region Groups population

            var groupSQL = @"DECLARE @count INT = 100 , @loopCount INT = 1        
                             WHILE @loopCount <= @count
                                 BEGIN
                                     INSERT  INTO dbo.Groups
                                             ( Name ,
                                               [Description]
                                             )
                                     VALUES  ( '.Net Developers ' + CONVERT(VARCHAR, @loopCount) ,
                                               '.NET Developers group is an independent user group formed by developers for developers.'
                                             )
                                     SET @loopCount = @loopCount + 1

                                     INSERT INTO dbo.UsersGroups ( UserId, GroupId )
                                     SELECT  (SELECT FLOOR(RAND()*(3-1)+1)) , SCOPE_IDENTITY()
                                 END";

            context.Database.ExecuteSqlCommand(groupSQL);
            context.SaveChanges();
            #endregion

            #region Stories population

            var storySQL = @"DECLARE @count INT = 1000 , @loopCount INT = 1, @StoryId INT
                             WHILE @loopCount <= @count
                                 BEGIN
                                     INSERT  INTO dbo.Stories
                                             ( Title ,
                                               [Description] ,
                                               Content ,
                                               PostedOn ,
                                               UserId
                                             )
                                     VALUES  ( '.NET Framework and .NET SDKs. ' + CONVERT(VARCHAR, @loopCount) ,
                                               'The .NET Framework is the easiest way to build apps on the Microsoft platform.' ,
                                               'The .NET Framework is the easiest way to build apps on the Microsoft platform. You can download Visual Studio Express for free, and be coding in just a few minutes. You can also use the .NET SDKs and targeting packs to build apps for a given Microsoft platform, such as Microsoft Azure.' ,
                                               GETDATE() ,
                                               {0}
                                             )
                                     SET @loopCount = @loopCount + 1
                                     
                                     INSERT  INTO dbo.StoriesGroups ( GroupId , StoryId )
                                     SELECT  (SELECT FLOOR(RAND()*(100-1)+1)) , SCOPE_IDENTITY()
                                 END";

            context.Database.ExecuteSqlCommand(string.Format(storySQL, 1));
            context.Database.ExecuteSqlCommand(string.Format(storySQL, 2));
            context.Database.ExecuteSqlCommand(string.Format(storySQL, 3));
            context.SaveChanges();


            #endregion
        }
    }
}