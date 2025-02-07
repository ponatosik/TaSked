using FirebaseAdmin.Messaging;
using TaSked.Application;
using TaSked.Domain;

namespace PushNotifications.Common;

public static class Helpers
{
	public static string GetGroupTopicName(Guid groupId) => $"Group.{groupId}";

	public static Notification GetReportCreatedNotification(Announcement announcement)
		=> new Notification()
		{
			Title = announcement.Title, Body = announcement.Message
		};

	public static Notification GetHomeworkCreatedNotification(Homework homework)
		=> new Notification()
		{
			Title = $"New homework created: {homework.Title}",
			Body = homework.Description
		};

	public static Notification GetHomeworkDeletedNotification(Homework homework)
		=> new Notification()
		{
			Title = $"Homework deleted: {homework.Title}",
			Body = homework.Description
		};

	public static Notification GetLessonCreatedNotification(Lesson lesson)
		=> new Notification()
		{
			Title = $"New lesson created: {lesson.Time}",
			Body = "lesson created"
		};

	public static Notification GetLessonDeletedNotification(Lesson lesson)
		=> new Notification()
		{
			Title = $"Lesson deleted: {lesson.Time}",
			Body = "lesson deleted"
		};

	public static Notification GetSubjectCreatedNotification(SubjectDTO subject)
		=> new Notification()
		{
			Title = $"New subject created: {subject.Name}",
			Body = "Subject created"
		};

	public static Notification GetSubjectDeletedNotification(SubjectDTO subject)
		=> new Notification()
		{
			Title = $"Subject deleted: {subject.Name}",
			Body = $"Subject with {subject.HomeworksCount} tasks deleted"
		};
}
