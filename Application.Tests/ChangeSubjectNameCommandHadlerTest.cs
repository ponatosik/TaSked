using Application.Tests;
using TaSked.Application.Data;
using TaSked.Domain;

namespace TaSked.Application.Tests;

[Collection("Persistance tests")]
public class ChangeSubjectNameCommandHadlerTest
{
	private readonly IApplicationDbContext _context;
	private readonly ChangeSubjectNameCommandHandler _handler;

	private readonly Guid _userId, _groupId, _subjectId;

	public ChangeSubjectNameCommandHadlerTest(PersistanceFixture persistanceFixture)
	{
		_context = persistanceFixture.GetDbContext();
		_handler = new ChangeSubjectNameCommandHandler(_context);
	
		User user = User.Create("Test user");
		Group group = Group.Create("Test group", user);
		Subject subject = group.CreateSubject("Test subject");

		_userId = user.Id;
		_groupId = group.Id;
		_subjectId = subject.Id;

		_context.Users.Add(user);
		_context.Groups.Add(group);
		_context.SaveChangesAsync(new CancellationToken()).Wait();
	}

	[Fact]
	public async Task Handle_ValidCommand_ShouldPersistChanges()
	{
		var newSubjectName = "Updated subject name";
		var command = new ChangeSubjectNameCommand(_userId, _subjectId, newSubjectName);
		
		await _handler.Handle(command, new CancellationToken());

		Assert.Equal(newSubjectName, 
			_context
			.Groups.First(group => group.Id == _groupId)
			.Subjects.First(subject => subject.Id == _subjectId)
			.Name);
	}
}