using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<Test> Tests { get; set; }

    protected override void OnConfiguring(
        DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite("DataSource=app.db;Cache=Shared");
   
}