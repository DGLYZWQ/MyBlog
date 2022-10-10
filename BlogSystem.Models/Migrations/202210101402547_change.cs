namespace BlogSystem.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class change : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "Avatar", c => c.String(maxLength: 255, unicode: false));
            AlterColumn("dbo.Users", "Image", c => c.String(maxLength: 255, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "Image", c => c.String(nullable: false, maxLength: 255, unicode: false));
            AlterColumn("dbo.Users", "Avatar", c => c.String(nullable: false, maxLength: 255, unicode: false));
        }
    }
}
