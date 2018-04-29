using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GenVue.Model
{
    public class DefaultDbContext : IdentityDbContext<ApplicationUser>
    {
        public DefaultDbContext(DbContextOptions<DefaultDbContext> options)
        : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        // for testing purpose
        public DbSet<Contact> Contacts { get; set; }

        // user groups
        public DbSet<Group> Groups { get; set; }

        // file categories
        public DbSet<FileCategory> FileCategories { get; set; }

        // user activity log
        public DbSet<ActivityLog> ActivityLogs { get; set; }

        public DbSet<StoredFile> StoredFiles { get; set; }

        public DbSet<GroupAccess> GroupAccess { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
