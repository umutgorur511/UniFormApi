using Microsoft.EntityFrameworkCore;
using UniForm.Entity;
using UniForm.Models;

namespace UniForm.Data
{
    public class DataContext : DbContext {
        
        public DataContext(DbContextOptions<DataContext> options) : base(options) {

        }

        public DbSet<Models.Action> Actions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Action>()
            .HasOne<User>()  // Action modelinde User ile bir ilişki kuruyoruz
            .WithMany()  // User modelinde birden fazla Action olabilir, ancak User'dan herhangi bir ilişkiyi almıyoruz
            .HasForeignKey(a => a.UserId)  // Foreign key, Action tablosundaki UserId alanını kullanacak
            .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<User>()
            .HasKey(u => u.Id);
            modelBuilder.Entity<Comment>()
            .HasKey(u => u.Id);
            modelBuilder.Entity<Post>()
            .HasKey(u => u.Id);
        }
    }
}
