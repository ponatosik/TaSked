using Microsoft.EntityFrameworkCore;
using TaSked.Application.Exceptions;
using TaSked.Domain;

namespace TaSked.Application;

internal static class DbSetExtentions
{
	public static User FindById(this DbSet<User> query, Guid id)
	{
		return query.Find(id) ?? throw new EntityNotFoundException(id, nameof(User));
	}

	public static Group FindById(this DbSet <Group> query, Guid id)
	{
		return query.Find(id) ?? throw new EntityNotFoundException(id, nameof(Group));
	}

	public static Subject FindById(this DbSet <Subject> query, Guid id)
	{
		return query.Find(id) ?? throw new EntityNotFoundException(id, nameof(Subject));
	}

	public static Homework FindById(this DbSet <Homework> query, Guid id)
	{
		return query.Find(id) ?? throw new EntityNotFoundException(id, nameof(Homework));
	}

	public static Lesson FindById(this DbSet <Lesson> query, Guid id)
	{
		return query.Find(id) ?? throw new EntityNotFoundException(id, nameof(Lesson));
	}

	public static Invitation FindById(this DbSet <Invitation> query, Guid id)
	{
		return query.Find(id) ?? throw new EntityNotFoundException(id, nameof(Invitation));
	}

	public static Report FindById(this DbSet <Report> query, Guid id)
	{
		return query.Find(id) ?? throw new EntityNotFoundException(id, nameof(Report));
	}
}
