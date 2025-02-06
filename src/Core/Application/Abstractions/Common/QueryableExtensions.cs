using TaSked.Application.Exceptions;
using TaSked.Domain;

namespace TaSked.Application;

internal static class QueryableExtensions
{
	public static User FindOrThrow(this IQueryable<User> query, Guid id)
	{
		return query.FirstOrDefault(user => user.Id == id) ?? throw new EntityNotFoundException(id, nameof(User));
	}

	public static Group FindOrThrow(this IQueryable<Group> query, Guid id)
	{
		return query.FirstOrDefault(group => group.Id == id) ?? throw new EntityNotFoundException(id, nameof(Group));
	}

	public static Subject FindOrThrow(this IQueryable<Subject> query, Guid id)
	{
		return query.FirstOrDefault(subject => subject.Id == id) ?? throw new EntityNotFoundException(id, nameof(Subject));
	}

	public static Homework FindOrThrow(this IQueryable<Homework> query, Guid id)
	{
		return query.FirstOrDefault(homework => homework.Id == id) ?? throw new EntityNotFoundException(id, nameof(Homework));
	}

	public static Lesson FindOrThrow(this IQueryable<Lesson> query, Guid id)
	{
		return query.FirstOrDefault(lesson => lesson.Id == id) ?? throw new EntityNotFoundException(id, nameof(Lesson));
	}

	public static Invitation FindOrThrow(this IQueryable<Invitation> query, Guid id)
	{
		return query.FirstOrDefault(invitation => invitation.Id == id) ?? throw new EntityNotFoundException(id, nameof(Invitation));
	}

	public static Announcement FindOrThrow(this IQueryable<Announcement> query, Guid id)
	{
		return query.FirstOrDefault(Announcement => Announcement.Id == id) ??
		       throw new EntityNotFoundException(id, nameof(Announcement));
	}
}
