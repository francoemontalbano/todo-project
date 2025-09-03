using Microsoft.EntityFrameworkCore;   
using Todo.Api.Models;


namespace Todo.Api.Data
{
    public class TodoDbContext(DbContextOptions<TodoDbContext> options) : DbContext(options)
    {
        public DbSet<TaskItem> Tasks => Set<TaskItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var t = modelBuilder.Entity<TaskItem>();
            t.ToTable("Tasks");
            t.HasKey(x => x.Id);
            t.Property(x => x.Title).HasMaxLength(120).IsRequired();
            t.Property(x => x.IsDone).HasDefaultValue(false);
            t.Property(x => x.CreatedAt).HasDefaultValueSql("sysutcdatetime()").IsRequired();

            t.HasIndex(x => x.IsDone);
            t.HasIndex(x => x.DueDate);
        }
    }
}
