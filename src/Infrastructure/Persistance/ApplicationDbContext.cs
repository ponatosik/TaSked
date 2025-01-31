﻿using Microsoft.EntityFrameworkCore;
using TaSked.Application.Data;
using TaSked.Domain;

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

		modelBuilder.Entity<Subject>().OwnsMany(
			e => e.RelatedLinks,
			navigation => navigation.ToJson());
		modelBuilder.Entity<Homework>().OwnsMany(
			e => e.RelatedLinks,
			navigation => navigation.ToJson());
		modelBuilder.Entity<Lesson>().OwnsOne<RelatedLink>(
			e => e.OnlineLessonUrl,
			navigation => navigation.ToJson());

		modelBuilder.Entity<Subject>().Property(e => e.Id).ValueGeneratedNever();
		modelBuilder.Entity<Homework>().Property(e => e.Id).ValueGeneratedNever();
		modelBuilder.Entity<Lesson>().Property(e => e.Id).ValueGeneratedNever();
		modelBuilder.Entity<Invitation>().Property(e => e.Id).ValueGeneratedNever();
		modelBuilder.Entity<Report>().Property(e => e.Id).ValueGeneratedNever();
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
