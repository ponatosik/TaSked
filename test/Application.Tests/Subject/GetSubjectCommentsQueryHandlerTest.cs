using Application.Tests;
using TaSked.Application;
using TaSked.Application.Data;
using TaSked.Domain;

namespace Application.SubjectTests;

[Collection("Database tests")]
public class GetSubjectCommentsCommandHandlerTest
{
	private readonly IApplicationDbContext _context;
	private readonly GetSubjectCommentsHandler _handler;

	private readonly Guid _userId, _subjectId;
	private readonly List<CommentDTO> _comments;

	public GetSubjectCommentsCommandHandlerTest(DbTestFixture dbTestFixture)
	{
		_context = dbTestFixture.GetDbContext();
		_handler = new GetSubjectCommentsHandler(_context);

		var user1 = User.Create("Test user (admin)");
		var user2 = User.Create("Guest user");
		var group = Group.Create("Test group", user1);
		var subject = group.CreateSubject("test subject 1");

		_userId = user1.Id;
		_subjectId = subject.Id;

		var comment1 = subject.LeaveComment(user1, "I like it");
		var comment2 = subject.LeaveComment(user2, "I do not");
		_comments = [CommentDTO.From(comment1), CommentDTO.From(comment2)];

		_context.Users.AddRange(user1, user2);
		_context.Groups.Add(group);
		_context.SaveChangesAsync(CancellationToken.None).Wait();
	}

	[Fact]
	public async Task Handle_ValidCommand_ShouldReturnSubjects()
	{
		var request = new GetSubjectCommentsQuery(_userId, _subjectId);

		var result = await _handler.Handle(request, CancellationToken.None);

		Assert.Equal(_comments.Count, result.Count);
		Assert.Contains(result,
			c => c.AuthorUsername == _comments[0].AuthorUsername && c.Content == _comments[0].Content);
		Assert.Contains(result,
			c => c.AuthorUsername == _comments[1].AuthorUsername && c.Content == _comments[1].Content);
	}
}