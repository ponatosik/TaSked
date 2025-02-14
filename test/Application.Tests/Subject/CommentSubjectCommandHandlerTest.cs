using Application.Tests;
using TaSked.Application;
using TaSked.Application.Data;
using TaSked.Domain;

namespace Application.SubjectTests;

[Collection("Database tests")]
public class CommentSubjectCommandHandlerTest
{
	private readonly IApplicationDbContext _context;
	private readonly CommentSubjectCommandHandler _handler;

	private readonly Guid _userId, _groupId, _subjectId;

	public CommentSubjectCommandHandlerTest(DbTestFixture dbTestFixture)
	{
		_context = dbTestFixture.GetDbContext();
		_handler = new CommentSubjectCommandHandler(_context);

		var user = User.Create(UserHelper.GenerateUniqueUserName());
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
		var commentContent = "Test comment";
		var command = new CommentSubjectCommand(_userId, _subjectId, commentContent);

		var comment = await _handler.Handle(command, CancellationToken.None);

		Assert.Equal(commentContent, comment.Content);
		Assert.Contains(
			_context
				.Groups.First(group => group.Id == _groupId)
				.Subjects.First(subject => subject.Id == _subjectId)
				.Comments, c => c.Id == comment.Id);
	}
}