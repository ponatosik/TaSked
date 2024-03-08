using TaSked.Application.Exceptions;
using TaSked.Domain;

namespace TaSked.Application;

internal static class EnumerableExtentions
{
	public static User FindById(this IEnumerable <User> query, Guid id)
	{
		return query.FirstOrDefault(user => user.Id == id) ?? throw new EntityNotFoundException(id, nameof(User));
	}

	public static Group FindById(this IEnumerable <Group> query, Guid id)
	{
		return query.FirstOrDefault(group => group.Id == id) ?? throw new EntityNotFoundException(id, nameof(Group));
	}

	public static Subject FindById(this IEnumerable <Subject> query, Guid id)
	{
		return query.FirstOrDefault(subject => subject.Id == id) ?? throw new EntityNotFoundException(id, nameof(Subject));
	}

	public static Homework FindById(this IEnumerable <Homework> query, Guid id)
	{
		return query.FirstOrDefault(homework => homework.Id == id) ?? throw new EntityNotFoundException(id, nameof(Homework));
	}

	public static Lesson FindById(this IEnumerable <Lesson> query, Guid id)
	{
		return query.FirstOrDefault(lesson => lesson.Id == id) ?? throw new EntityNotFoundException(id, nameof(Lesson));
	}

	public static Invitation FindById(this IEnumerable <Invitation> query, Guid id)
	{
		return query.FirstOrDefault(invitation => invitation.Id == id) ?? throw new EntityNotFoundException(id, nameof(Invitation));
	}

	public static Report FindById(this IEnumerable <Report> query, Guid id)
	{
		return query.FirstOrDefault(Report => Report.Id == id) ?? throw new EntityNotFoundException(id, nameof(Report));
	}
}
