using Application.Tests;
using TaSked.Application;
using TaSked.Application.Data;
using TaSked.Domain;

namespace Application.HomeworkTests;

[Collection("Database tests")]
public class GetHomeworkCommentsQueryHandlerTest
{
	private readonly IApplicationDbContext _context;
	private readonly GetHomeworkCommentsHandler _handler;

	private readonly Guid _userId, _subjectId, _homeworkId;

	private readonly List<CommentDTO> _comments;

	public GetHomeworkCommentsQueryHandlerTest(DbTestFixture dbTestFixture)
	{
		_context = dbTestFixture.GetDbContext();
		_handler = new GetHomeworkCommentsHandler(_context);

		var user1 = User.Create("Test user (admin)");
		var user2 = User.Create("Guest user");
		var group = Group.Create("Test group", user1);
		var subject = group.CreateSubject("test subject 1");
		var homework = subject.CreateHomework("Homework 1", "Do something");

		_userId = user1.Id;
		_subjectId = subject.Id;
		_homeworkId = homework.Id;

		var comment1 = homework.LeaveComment(user1, "I like it");
		var comment2 = homework.LeaveComment(user2, "I do not");
		_comments = [CommentDTO.From(comment1), CommentDTO.From(comment2)];

		_context.Users.AddRange(user1, user2);
		_context.Groups.Add(group);
		_context.SaveChangesAsync(CancellationToken.None).Wait();
	}

	[Fact]
	public async Task Handle_ValidCommand_ShouldReturnSubjects()
	{
		var request = new GetHomeworkCommentsQuery(_userId, _subjectId, _homeworkId);

		var result = await _handler.Handle(request, CancellationToken.None);

		Assert.Equal(_comments.Count, result.Count);
		Assert.Contains(result,
			c => c.AuthorUsername == _comments[0].AuthorUsername && c.Content == _comments[0].Content);
		Assert.Contains(result,
			c => c.AuthorUsername == _comments[1].AuthorUsername && c.Content == _comments[1].Content);
	}
}