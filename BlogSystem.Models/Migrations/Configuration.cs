﻿namespace BlogSystem.Models.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BlogSystem.Models.BlogSystemContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(BlogSystem.Models.BlogSystemContext context)
        {
            if (!context.Roles.Any())
            {
                context.Roles.AddOrUpdate(new Roles[]
                {
                    new Roles() { Title = "管理员"},
                    new Roles() { Title = "用户"}
                });

                context.SaveChanges();
            }
        }
    }
}
