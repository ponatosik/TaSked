using Microsoft.EntityFrameworkCore;
using TaSked.Application.Data;
using TaSked.Domain;

namespace TaSked.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
	public DbSet<Group> Groups { get; set; }
	public DbSet<User> Users { get; set; }

	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
	public ApplicationDbContext() { }

	
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<User>().OwnsOne(e => e.Role);
		modelBuilder.Entity<Teacher>().OwnsOne(e => e.OnlineMeetingUrl);

		
		modelBuilder.Entity<Subject>().OwnsMany(e => e.Comments, navigation =>
		{
			navigation.ToTable("SubjectComments");
			navigation.HasKey(x => x.Id);
			navigation.Property(x => x.Id).ValueGeneratedNever();
		});
		modelBuilder.Entity<Homework>().OwnsMany(e => e.Comments, navigation =>
		{
			navigation.ToTable("HomeworkComments");
			navigation.HasKey(x => x.Id);
			navigation.Property(x => x.Id).ValueGeneratedNever();
		});
		

		modelBuilder.Entity<Subject>().OwnsMany(
			e => e.RelatedLinks,
			navigation => navigation.ToJson());
		modelBuilder.Entity<Homework>().OwnsMany(
			e => e.RelatedLinks,
			navigation => navigation.ToJson());
		modelBuilder.Entity<Lesson>().OwnsOne<RelatedLink>(
			e => e.OnlineLessonUrl,
			navigation => navigation.ToJson());

		modelBuilder.Entity<Subject>().HasMany(x => x.Teachers).WithOne().IsRequired();

		modelBuilder.Entity<Subject>().Property(e => e.Id).ValueGeneratedNever();
		modelBuilder.Entity<Homework>().Property(e => e.Id).ValueGeneratedNever();
		modelBuilder.Entity<Lesson>().Property(e => e.Id).ValueGeneratedNever();
		modelBuilder.Entity<Invitation>().Property(e => e.Id).ValueGeneratedNever();
		modelBuilder.Entity<Announcement>().Property(e => e.Id).ValueGeneratedNever();
		modelBuilder.Entity<Teacher>().Property(e => e.Id).ValueGeneratedNever();

		modelBuilder.Entity<User>().HasIndex(u => u.Nickname).IsUnique();
	}
}
