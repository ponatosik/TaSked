using TaSked.Application.Data;
using TaSked.Domain;
using Microsoft.EntityFrameworkCore;
namespace TaSked.Infrastructure.Persistance;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
	public DbSet<Group> Groups { get; set; }
	public DbSet<User> Users { get; set; }

	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
	public ApplicationDbContext() { }

	
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<User>().OwnsOne(e => e.Role);
		modelBuilder.Entity<Subject>().OwnsOne(e => e.Teacher);

		modelBuilder.Entity<Subject>().Property(e => e.Id).ValueGeneratedNever();
		modelBuilder.Entity<Homework>().Property(e => e.Id).ValueGeneratedNever();
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		if (!optionsBuilder.IsConfigured)
		{
			optionsBuilder.UseInMemoryDatabase("In-memory");
		}

		base.OnConfiguring(optionsBuilder);
	}
}
