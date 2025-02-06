using TaSked.Application.Exceptions;
using TaSked.Domain;

namespace TaSked.Application;

internal static class EnumerableExtensions
{
	public static User FindOrThrow(this IEnumerable <User> query, Guid id)
	{
		return query.FirstOrDefault(user => user.Id == id) ?? throw new EntityNotFoundException(id, nameof(User));
	}

	public static Group FindOrThrow(this IEnumerable <Group> query, Guid id)
	{
		return query.FirstOrDefault(group => group.Id == id) ?? throw new EntityNotFoundException(id, nameof(Group));
	}

	public static Subject FindOrThrow(this IEnumerable <Subject> query, Guid id)
	{
		return query.FirstOrDefault(subject => subject.Id == id) ?? throw new EntityNotFoundException(id, nameof(Subject));
	}

	public static Homework FindOrThrow(this IEnumerable <Homework> query, Guid id)
	{
		return query.FirstOrDefault(homework => homework.Id == id) ?? throw new EntityNotFoundException(id, nameof(Homework));
	}

	public static Lesson FindOrThrow(this IEnumerable <Lesson> query, Guid id)
	{
		return query.FirstOrDefault(lesson => lesson.Id == id) ?? throw new EntityNotFoundException(id, nameof(Lesson));
	}

	public static Invitation FindOrThrow(this IEnumerable <Invitation> query, Guid id)
	{
		return query.FirstOrDefault(invitation => invitation.Id == id) ?? throw new EntityNotFoundException(id, nameof(Invitation));
	}

	public static Announcement FindOrThrow(this IEnumerable<Announcement> query, Guid id)
	{
		return query.FirstOrDefault(announcement => announcement.Id == id) ??
		       throw new EntityNotFoundException(id, nameof(Announcement));
	}
}
