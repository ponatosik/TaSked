using TaSked.Application.Exceptions;
using TaSked.Domain;

namespace TaSked.Application;

internal static class QuriableExtentions
{
	public static User FindById(this IQueryable<User> query, Guid id)
	{
		return query.FirstOrDefault(user => user.Id == id) ?? throw new EntityNotFoundException(id, nameof(User));
	}

	public static Group FindById(this IQueryable<Group> query, Guid id)
	{
		return query.FirstOrDefault(group => group.Id == id) ?? throw new EntityNotFoundException(id, nameof(Group));
	}

	public static Subject FindById(this IQueryable<Subject> query, Guid id)
	{
		return query.FirstOrDefault(subject => subject.Id == id) ?? throw new EntityNotFoundException(id, nameof(Subject));
	}

	public static Homework FindById(this IQueryable<Homework> query, Guid id)
	{
		return query.FirstOrDefault(homework => homework.Id == id) ?? throw new EntityNotFoundException(id, nameof(Homework));
	}

	public static Lesson FindById(this IQueryable<Lesson> query, Guid id)
	{
		return query.FirstOrDefault(lesson => lesson.Id == id) ?? throw new EntityNotFoundException(id, nameof(Lesson));
	}

	public static Invitation FindById(this IQueryable<Invitation> query, Guid id)
	{
		return query.FirstOrDefault(invitation => invitation.Id == id) ?? throw new EntityNotFoundException(id, nameof(Invitation));
	}

	public static Report FindById(this IQueryable<Report> query, Guid id)
	{
		return query.FirstOrDefault(Report => Report.Id == id) ?? throw new EntityNotFoundException(id, nameof(Report));
	}
}
