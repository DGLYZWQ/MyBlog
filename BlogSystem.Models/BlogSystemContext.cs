using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace BlogSystem.Models
{
    public class BlogSystemContext : DbContext
    {
        public BlogSystemContext():base("con")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
             
        }

        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Blog> Blog { get; set; }
        public virtual DbSet<Draft> Drafts { get; set; }
        public virtual DbSet<ThumbCollect> ThumbCollects { get; set; }
        public virtual DbSet<Comments> Comments { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Messages> Messages { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<Views> Views { get; set; }
        public virtual DbSet<UserFocus> UserFocus { get; set; }
        public virtual DbSet<UserMsg> UserMsg { get; set; }

    }
}