namespace Storybook.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class GetGroupsSP : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("GetGroups", p =>
                new
                {
                    Page = p.Int(),
                    PageSize = p.Int()
                },
                @"  SELECT  tt.Id ,
                            tt.Name ,
                            tt.[Description],
				            ( SELECT COUNT(*) FROM dbo.Groups ) AS TotalRecords ,
				            ( SELECT COUNT(*) FROM dbo.UsersGroups WHERE GroupId = tt.Id ) AS MembersCount ,
				            ( SELECT COUNT(*) FROM dbo.StoriesGroups WHERE GroupId = tt.Id ) AS StoriesCount
                    FROM    ( SELECT    t.rowNum ,
                                        t.Id ,
                                        t.Name ,
                                        t.Description
                              FROM      ( SELECT    ROW_NUMBER() OVER ( ORDER BY g.Id DESC) AS rowNum ,
                                                    g.*
                                          FROM      dbo.Groups g
                                        ) t
                            ) tt
                    WHERE   tt.rowNum > ( @Page - 1 ) * @PageSize
                            AND tt.rowNum <= @Page * @PageSize"
                );
        }

        public override void Down()
        {
            DropStoredProcedure("GetGroups");
        }
    }
}