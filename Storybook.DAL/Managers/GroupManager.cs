using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Storybook.DataModel.Models;
using Storybook.Common.Extensions;
using Storybook.Common.Utility;
using Storybook.DAL.DBContext;

namespace Storybook.DAL.Managers
{
    public class GroupManager : ManagerBase
    {
        public static PagedList<GroupEx> GetGroups(int page, int pageSize)
        {
            using (var db = new StorybookContext())
            {
                var groups = db.Database.SqlQuery<GroupEx>("GetGroups @Page, @PageSize",
                new SqlParameter("@Page", page),
                new SqlParameter("@PageSize", pageSize)).ToList();

                return groups.ToPagedList(page, pageSize);
            }
        }

        public static List<Group> GetAllGroups()
        {
            using (var db = new StorybookContext())
            {
                return db.Groups.ToList();
            }
        }

        public static async Task<List<Group>> GetAllGroupsAsync()
        {
            using (var db = new StorybookContext())
            {
                return await db.Groups.ToListAsync();
            }
        }

        public static async Task<Group> FindAsync(int id)
        {
            using (var db = new StorybookContext())
            {
                return await db.Groups.FindAsync(id);
            }
        }

        public static async Task SaveGroupAsync(Group group)
        {
            using (var db = new StorybookContext())
            {
                if (group.Id <= 0)
                    db.Groups.Add(group);
                else
                    db.Entry(group).State = EntityState.Modified;

                await db.SaveChangesAsync();
            }
        }

        public static async Task DeleteAsync(int id)
        {
            using (var db = new StorybookContext())
            {
                Group group = await db.Groups.FindAsync(id);
                db.Groups.Remove(group);
                await db.SaveChangesAsync();
            }
        }

        public static async Task JoinAsync(int groupId, int userId)
        {
            using (var db = new StorybookContext())
            {
                db.Database.ExecuteSqlCommand(@"IF EXISTS ( SELECT * FROM dbo.UsersGroups WHERE UserId = @UserId AND GroupId = @GroupId )
                                                BEGIN
                                                  DELETE FROM dbo.UsersGroups WHERE UserId = @UserId AND GroupId = @GroupId 
                                                END
                                                ELSE
                                                BEGIN
                                                  INSERT  INTO dbo.UsersGroups ( UserId, GroupId )
                                                  VALUES  ( @UserId, @GroupId )
                                                END",
                    new SqlParameter("@UserId", userId),
                    new SqlParameter("@GroupId", groupId));

                await db.SaveChangesAsync();
            }
        }
    }
}
