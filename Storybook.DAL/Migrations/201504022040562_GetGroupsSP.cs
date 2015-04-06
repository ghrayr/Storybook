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
                @"  WITH    GroupsCTE
                              AS ( SELECT * FROM dbo.Groups )
                        SELECT  g.* ,
                                ( SELECT COUNT(*) FROM GroupsCTE WITH ( NOLOCK ) ) AS TotalRecords ,
					            ( SELECT COUNT(*) FROM dbo.UsersGroups WITH ( NOLOCK ) WHERE GroupId = g.Id ) AS MembersCount ,
					            ( SELECT COUNT(*) FROM dbo.StoriesGroups WITH ( NOLOCK ) WHERE GroupId = g.Id ) AS StoriesCount
                        FROM    GroupsCTE g
                        ORDER BY Id DESC
                                OFFSET ( @Page - 1 ) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY"
                );
        }

        public override void Down()
        {
            DropStoredProcedure("GetGroups");
        }
    }
}