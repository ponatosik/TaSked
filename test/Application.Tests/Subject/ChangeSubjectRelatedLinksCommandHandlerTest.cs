using Application.Tests;
using TaSked.Application;
using TaSked.Application.Data;
using TaSked.Domain;

namespace Application.SubjectTests;

[Collection("Database tests")]
public class ChangeSubjectRelatedLinksCommandHandlerTest
{
	private readonly IApplicationDbContext _context;
	private readonly ChangeSubjectLinksCommandHandler _handler;

	private readonly Guid _userId, _groupId, _subjectId;

	public ChangeSubjectRelatedLinksCommandHandlerTest(DbTestFixture dbTestFixture)
	{
		_context = dbTestFixture.GetDbContext();
		_handler = new ChangeSubjectLinksCommandHandler(_context);

		var user = User.Create("Test user");
		var group = Group.Create("Test group", user);
		var subject = group.CreateSubject("Test subject");

		_userId = user.Id;
		_groupId = group.Id;
		_subjectId = subject.Id;

		_context.Users.Add(user);
		_context.Groups.Add(group);
		_context.SaveChangesAsync(CancellationToken.None).Wait();
	}

	[Fact]
	public async Task Handle_ValidCommand_ShouldPersistChanges()
	{
		List<RelatedLink> newSubjectRelatedLinks =
		[
			RelatedLink.Create(new Uri("https://tasked.test/Groups")),
			RelatedLink.Create(new Uri("https://drive.google.com/Lectures"), "Google drive")
		];

		var command = new ChangeSubjectRelatedLinksCommand(_userId, _subjectId, newSubjectRelatedLinks);

		await _handler.Handle(command, CancellationToken.None);

		Assert.Equal(newSubjectRelatedLinks,
			_context
				.Groups.First(group => group.Id == _groupId)
				.Subjects.First(subject => subject.Id == _subjectId)
				.RelatedLinks);
	}
}