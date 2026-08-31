using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace TodoApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<TaskItem> Tasks => Set<TaskItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var utcConverter = new ValueConverter<DateTime, DateTime>(
        v => v,
        v => DateTime.SpecifyKind(v, DateTimeKind.Utc)); //time conversion for iOS

        modelBuilder.Entity<TaskItem>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.TaskDescription).IsRequired();  //no empty task description
            entity.Property(t => t.CreatedDate).HasConversion(utcConverter);
            entity.Property(t => t.DueDate).HasConversion(utcConverter);
        });
    }
}