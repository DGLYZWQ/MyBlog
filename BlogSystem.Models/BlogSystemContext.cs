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
        public virtual DbSet<Comments> Comments { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Messages> Messages { get; set; }
        public virtual DbSet<DataShow> DataShow { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }

    }
}