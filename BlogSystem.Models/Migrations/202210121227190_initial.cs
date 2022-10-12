namespace BlogSystem.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Blogs",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(nullable: false, maxLength: 50, unicode: false),
                        Content = c.String(nullable: false, unicode: false, storeType: "text"),
                        UsersId = c.Guid(nullable: false),
                        CategoryId = c.Guid(nullable: false),
                        Views = c.Int(nullable: false),
                        Comments = c.Int(nullable: false),
                        IsPublic = c.Boolean(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        IsRemoved = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId)
                .ForeignKey("dbo.Users", t => t.UsersId)
                .Index(t => t.UsersId)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(nullable: false, maxLength: 50, unicode: false),
                        Description = c.String(nullable: false, maxLength: 1000, unicode: false),
                        CreateTime = c.DateTime(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        IsRemoved = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        NickName = c.String(nullable: false, maxLength: 50, unicode: false),
                        Email = c.String(nullable: false, maxLength: 100, unicode: false),
                        Password = c.String(nullable: false, maxLength: 30, unicode: false),
                        Avatar = c.String(maxLength: 255, unicode: false),
                        Image = c.String(maxLength: 255, unicode: false),
                        BlogCount = c.Int(nullable: false),
                        CategoryCount = c.Int(nullable: false),
                        FollowsCount = c.Int(nullable: false),
                        FansCount = c.Int(nullable: false),
                        RolesId = c.Guid(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        IsRemoved = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.RolesId)
                .Index(t => t.RolesId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(nullable: false, maxLength: 100, unicode: false),
                        CreateTime = c.DateTime(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        IsRemoved = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Content = c.String(nullable: false, maxLength: 50, unicode: false),
                        BlogId = c.Guid(nullable: false),
                        IsChecked = c.Boolean(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        IsRemoved = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Blogs", t => t.BlogId)
                .Index(t => t.BlogId);
            
            CreateTable(
                "dbo.DataShows",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(nullable: false, maxLength: 50, unicode: false),
                        Icons = c.String(nullable: false, maxLength: 50, unicode: false),
                        Views = c.Int(nullable: false),
                        Comments = c.Int(nullable: false),
                        Messages = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        IsRemoved = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50, unicode: false),
                        Email = c.String(nullable: false, maxLength: 100, unicode: false),
                        Tel = c.String(nullable: false, maxLength: 50, unicode: false),
                        Content = c.String(nullable: false, maxLength: 1000, unicode: false),
                        IsRead = c.Boolean(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        IsRemoved = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SystemMenus",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(nullable: false, maxLength: 255, unicode: false),
                        Link = c.String(nullable: false, maxLength: 255, unicode: false),
                        Icon = c.String(maxLength: 255, unicode: false),
                        ParentId = c.Guid(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        IsRemoved = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WebMenus",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(nullable: false, maxLength: 255, unicode: false),
                        Link = c.String(nullable: false, maxLength: 255, unicode: false),
                        Icon = c.String(maxLength: 255, unicode: false),
                        ParentId = c.Guid(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        IsRemoved = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "BlogId", "dbo.Blogs");
            DropForeignKey("dbo.Blogs", "UsersId", "dbo.Users");
            DropForeignKey("dbo.Users", "RolesId", "dbo.Roles");
            DropForeignKey("dbo.Blogs", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Comments", new[] { "BlogId" });
            DropIndex("dbo.Users", new[] { "RolesId" });
            DropIndex("dbo.Blogs", new[] { "CategoryId" });
            DropIndex("dbo.Blogs", new[] { "UsersId" });
            DropTable("dbo.WebMenus");
            DropTable("dbo.SystemMenus");
            DropTable("dbo.Messages");
            DropTable("dbo.DataShows");
            DropTable("dbo.Comments");
            DropTable("dbo.Roles");
            DropTable("dbo.Users");
            DropTable("dbo.Categories");
            DropTable("dbo.Blogs");
        }
    }
}
