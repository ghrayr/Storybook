using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

using Storybook.Common.Extensions;
using Storybook.Common.Utility;
using Storybook.DAL.DBContext;
using Storybook.DataModel.Models;


namespace Storybook.DAL.Managers
{
    public class GroupManager : ManagerBase
    {
        /// <summary>
        /// Returns paged groups by the given page and pageSize
        /// </summary>
        /// <param name="page">Current page</param>
        /// <param name="pageSize">Rows count in the page</param>
        /// <returns>PagedList of GroupEx</returns>
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

        /// <summary>
        /// Returns all the groups
        /// </summary>
        /// <returns>List of Group</returns>
        public static List<Group> GetAllGroups()
        {
            using (var db = new StorybookContext())
            {
                return db.Groups.ToList();
            }
        }

        /// <summary>
        /// Returns all the groups asynchronously
        /// </summary>
        /// <returns>Task List of Group</returns>
        public static async Task<List<Group>> GetAllGroupsAsync()
        {
            using (var db = new StorybookContext())
            {
                return await db.Groups.ToListAsync();
            }
        }

        /// <summary>
        /// Returns a group with the specified id asynchronously
        /// </summary>
        /// <param name="id">Identifier of the group</param>
        /// <returns>Task Group asynchronously</returns>
        public static async Task<Group> FindAsync(int id)
        {
            using (var db = new StorybookContext())
            {
                return await db.Groups.FindAsync(id);
            }
        }

        /// <summary>
        /// Inserts or updates the group asynchronously
        /// </summary>
        /// <param name="group">Group to insert/update</param>
        /// <returns>Null if success or exception</returns>
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

        /// <summary>
        /// Removes the group asynchronously
        /// </summary>
        /// <param name="id">Identifier of the group to delete</param>
        /// <returns>Null if success or exception</returns>
        public static async Task DeleteAsync(int id)
        {
            using (var db = new StorybookContext())
            {
                Group group = await db.Groups.FindAsync(id);
                db.Groups.Remove(group);
                await db.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Joins the given user to the given group 
        /// </summary>
        /// <param name="groupId">Identifier of the Group</param>
        /// <param name="userId">Identifier of the User</param>
        /// <returns>Null if success or exception</returns>
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
