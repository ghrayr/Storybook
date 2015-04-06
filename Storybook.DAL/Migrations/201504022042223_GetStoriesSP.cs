namespace Storybook.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class GetStoriesSP : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("GetStories", p =>
                new
                {
                    UserId = p.Int(),
                    Page = p.Int(),
                    PageSize = p.Int()
                },
                @"  WITH    StoriesCTE
                              AS ( SELECT *
                                   FROM   dbo.Stories
                                   WHERE  UserId = @UserId
                                 )
                        SELECT  * ,
                                ( SELECT  COUNT(*) FROM StoriesCTE WITH ( NOLOCK ) ) AS TotalRecords ,
                                ( SELECT  CAST(GroupId AS VARCHAR(32)) + ','
                                  FROM    dbo.StoriesGroups
                                  WHERE   StoryId = s.Id
                                  FOR XML PATH('')
                                ) AS GroupIdsString
                        FROM    StoriesCTE s
                        ORDER BY Id DESC
                                OFFSET ( @Page - 1 ) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY"
                );
        }

        public override void Down()
        {
            DropStoredProcedure("GetStories");
        }
    }
}