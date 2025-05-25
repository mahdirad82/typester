using Microsoft.EntityFrameworkCore;
using TextAnalyzer.Models;

namespace TextAnalyzer.Data;

public class SqliteDbContext : DbContext
{
    public DbSet<WordModel> Words { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite("Data Source=words.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WordModel>()
            .HasKey(w => w.Id);
        modelBuilder.Entity<WordModel>()
            .Property(w => w.Id)
            .ValueGeneratedOnAdd(); // Configure Id for auto-increment
    }
}