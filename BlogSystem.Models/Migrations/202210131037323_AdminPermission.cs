namespace BlogSystem.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdminPermission : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdminsPermissions",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        RolesId = c.Guid(nullable: false),
                        SystemMenuId = c.Guid(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        IsRemoved = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.RolesId)
                .ForeignKey("dbo.SystemMenus", t => t.SystemMenuId)
                .Index(t => t.RolesId)
                .Index(t => t.SystemMenuId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AdminsPermissions", "SystemMenuId", "dbo.SystemMenus");
            DropForeignKey("dbo.AdminsPermissions", "RolesId", "dbo.Roles");
            DropIndex("dbo.AdminsPermissions", new[] { "SystemMenuId" });
            DropIndex("dbo.AdminsPermissions", new[] { "RolesId" });
            DropTable("dbo.AdminsPermissions");
        }
    }
}
