using Application.Tests;
using TaSked.Application;
using TaSked.Application.Data;
using TaSked.Domain;

namespace Application.HomeworkTests;

[Collection("Database tests")]
public class ChangeHomeworkRelatedLinksCommandHandlerTest
{
	private readonly IApplicationDbContext _context;
	private readonly ChangeHomeworkRelatedLinksCommandHandler _handler;

	private readonly Guid _userId, _groupId, _subjectId, _homeworkId;

	public ChangeHomeworkRelatedLinksCommandHandlerTest(DbTestFixture dbTestFixture)
	{
		_context = dbTestFixture.GetDbContext();
		_handler = new ChangeHomeworkRelatedLinksCommandHandler(_context);

		var user = User.Create(UserHelper.GenerateUniqueUserName());
		var group = Group.Create("Test group", user);
		var subject = group.CreateSubject("Test subject");
		var homework = subject.CreateHomework("Test homework", "Test description",
			relatedLinks:
			[
				RelatedLink.Create(new Uri("https://tasked.test")),
				RelatedLink.Create(new Uri("https://youtube.com"), "video")
			]);

		_userId = user.Id;
		_groupId = group.Id;
		_subjectId = subject.Id;
		_homeworkId = homework.Id;

		_context.Users.Add(user);
		_context.Groups.Add(group);
		_context.SaveChangesAsync(CancellationToken.None).Wait();
	}

	[Fact]
	public async Task Handle_ValidCommand_ShouldPersistChanges()
	{
		List<RelatedLink> newLinks =
		[
			RelatedLink.Create(new Uri("https://tasked.test")),
			RelatedLink.Create(new Uri("https://youtube.com"), "video")
		];
		var command = new ChangeHomeworkRelatedLinksCommand(_userId, _subjectId, _homeworkId, newLinks);

		await _handler.Handle(command, CancellationToken.None);

		Assert.Equal(newLinks,
			_context
				.Groups.First(group => group.Id == _groupId)
				.Subjects.First(subject => subject.Id == _subjectId)
				.Homeworks.First(homework => homework.Id == _homeworkId)
				.RelatedLinks);
	}
}