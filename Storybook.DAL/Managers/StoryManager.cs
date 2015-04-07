using System;
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
    public class StoryManager : ManagerBase
    {
        /// <summary>
        /// Returns paged stories posted by the given user
        /// </summary>
        /// <param name="userId">Identifier of the user</param>
        /// <param name="page">Current page</param>
        /// <param name="pageSize">Rows count in the page</param>
        /// <returns>PagedList of StoryEx</returns>
        public static PagedList<StoryEx> GetStories(int userId, int page, int pageSize)
        {
            using (var db = new StorybookContext())
            {
                //var stories = db.Stories.Include(s => s.User).Where(s => s.UserId == userId).OrderBy(x => x.Id).Skip(page).Take(pageSize).ToList();
                var stories = db.Database.SqlQuery<StoryEx>("GetStories @UserId, @Page, @PageSize",
                    new SqlParameter("@UserId", userId),
                    new SqlParameter("@Page", page),
                    new SqlParameter("@PageSize", pageSize)).ToList();

                foreach (var story in stories.Where(storyEx => !string.IsNullOrEmpty(storyEx.GroupIdsString)))
                    story.GroupIds = story.GroupIdsString.TrimEnd(',').Split(',').Select(int.Parse).ToList();

                return stories.ToPagedList(page, pageSize);
            }
        }

        /// <summary>
        /// Returns a story with the specified id asynchronously
        /// </summary>
        /// <param name="id">Identifier of the story</param>
        /// <param name="includeGroupIds">Determines whether to include all joined group ids or not</param>
        /// <returns>Task Story asynchronously</returns>
        public static async Task<Story> FindAsync(int id, bool includeGroupIds = false)
        {
            using (var db = new StorybookContext())
            {
                if (includeGroupIds)
                    return await db.Stories.Include(x => x.Groups).Where(x => x.Id == id).FirstOrDefaultAsync();
                
                return await db.Stories.FindAsync(id);
            }
        }

        /// <summary>
        /// Inserts or updates the story asynchronously
        /// </summary>
        /// <param name="story">Story to insert/update</param>
        /// <returns>Null if success or exception</returns>
        public static async Task SaveStoryAsync(Story story)
        {
            using (var db = new StorybookContext())
            {
                using (var dbTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        // INSERT
                        if (story.Id <= 0)
                        {
                            var query = @"INSERT INTO dbo.Stories ( Title , Description , Content , PostedOn , UserId )
                                          VALUES  ( @Title , @Description , @Content , GETDATE() , @UserId ) ";

                            if (story.GroupIds.Any())
                            {
                                query += string.Format(
                                    @"
                                      DECLARE @StoryId INT = SCOPE_IDENTITY()
                          
                                      INSERT  INTO dbo.StoriesGroups ( GroupId , StoryId )
                                              SELECT  [group].Id , @StoryId
                                              FROM    ( SELECT Id FROM dbo.Groups WHERE Id IN ( {0} ) ) AS [group]",
                                    string.Join(",", story.GroupIds));
                            }
                            db.Database.ExecuteSqlCommand(query,
                                new SqlParameter("@Title", story.Title),
                                new SqlParameter("@Description", story.Description ?? string.Empty),
                                new SqlParameter("@Content", story.Content ?? string.Empty),
                                new SqlParameter("@UserId", story.UserId));
                        }
                        // UPDATE
                        else
                        {
                            var query = @"UPDATE dbo.Stories
                                          SET Title = @Title , Description = @Description , Content = @Content , PostedOn = GETDATE(), UserId = @UserId 
                                          WHERE  Id = @StoryId
                                      
                                          DELETE FROM dbo.StoriesGroups WHERE StoryId = @StoryId ";

                            if (story.GroupIds.Any())
                            {
                                query += string.Format(
                                    @"
                                      INSERT  INTO dbo.StoriesGroups ( GroupId , StoryId )
                                      SELECT  [group].Id , @StoryId
                                      FROM    ( SELECT Id FROM dbo.Groups WHERE Id IN ( {0} ) ) AS [group]",
                                    string.Join(",", story.GroupIds));
                            }

                            db.Database.ExecuteSqlCommand(query,
                                new SqlParameter("@Title", story.Title),
                                new SqlParameter("@Description", story.Description ?? string.Empty),
                                new SqlParameter("@Content", story.Content ?? string.Empty),
                                new SqlParameter("@UserId", story.UserId),
                                new SqlParameter("@StoryId", story.Id));
                        }

                        await db.SaveChangesAsync();
                        dbTransaction.Commit();
                    }
                    catch (Exception)
                    {
                        dbTransaction.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Removes the story asynchronously
        /// </summary>
        /// <param name="id">Identifier of the story to delete</param>
        /// <returns>Null if success or exception</returns>
        public static async Task DeleteAsync(int id)
        {
            using (var db = new StorybookContext())
            {
                Story story = await db.Stories.FindAsync(id);
                db.Stories.Remove(story);
                await db.SaveChangesAsync();
            }
        }
    }
}
