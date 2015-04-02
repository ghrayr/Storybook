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
                @"  SELECT  tt.Id,
							tt.UserId ,
                            tt.Title ,
                            tt.PostedOn ,
                            tt.Content ,
                            tt.[Description] ,
                            ( SELECT DISTINCT CAST(GroupId AS VARCHAR(32)) + ',' FROM dbo.StoriesGroups WHERE StoryId = tt.Id FOR XML PATH('') ) AS GroupIdsString ,
                            ( SELECT COUNT(*) FROM dbo.Stories ) AS TotalRecords
                    FROM    ( SELECT    t.rowNum ,
                                        t.Id,
							            t.UserId ,
                                        t.Title ,
                                        t.PostedOn ,
                                        t.Content ,
                                        t.[Description]
                                FROM    ( SELECT    ROW_NUMBER() OVER ( ORDER BY story.Id DESC) AS rowNum ,
                                                    story.*
                                          FROM      dbo.Stories story
                                          WHERE     story.UserId = @UserId
                                        ) t
                            ) tt
                    WHERE   tt.rowNum > ( @Page - 1 ) * @PageSize
                            AND tt.rowNum <= @Page * @PageSize"
                );
        }

        public override void Down()
        {
            DropStoredProcedure("GetStories");
        }
    }
}