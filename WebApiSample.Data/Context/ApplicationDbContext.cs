using Microsoft.EntityFrameworkCore;
using WebApiSample.Data.Entities.Users;

namespace WebApiSample.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<LoginHistoryItem> LoginHistory { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UserToken> UserTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureUser(modelBuilder);
            ConfigureLoginHistory(modelBuilder);
        }

        private void ConfigureUser(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property(b => b.Email).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<User>().Property(b => b.FirstName).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<User>().Property(b => b.LastName).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<User>().Property(b => b.Username).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<User>().Property(b => b.PasswordHash).HasMaxLength(100);
            modelBuilder.Entity<User>().Property(b => b.PasswordSalt).HasMaxLength(100);
            modelBuilder.Entity<User>()
                .HasOne(b => b.Token)
                .WithOne(t => t.User)
                .HasForeignKey<UserToken>(t => t.UserId);
        }

        private void ConfigureLoginHistory(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LoginHistoryItem>().ToTable("LoginHistory");
            modelBuilder.Entity<LoginHistoryItem>().Property(b => b.UserAgent).IsRequired();
            modelBuilder.Entity<LoginHistoryItem>().Property(b => b.RemoteIp).IsRequired();
        }
    }
}
