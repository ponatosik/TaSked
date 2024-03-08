using TaSked.Application.Data;
using TaSked.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace TaSked.Application;

public class ChangeHomeworkDeadlineCommandHandler : IRequestHandler<ChangeHomeworkDeadlineCommand, Homework>
{
    private readonly IApplicationDbContext _context;

    public ChangeHomeworkDeadlineCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Homework> Handle(ChangeHomeworkDeadlineCommand request, CancellationToken cancellationToken)
    {
        var user = _context.Users.FindById(request.UserId);
        var group = _context.Groups.Include(group => group.Subjects).FindById(user.GroupId.Value);
        var subject = group.Subjects.FindById(request.SubjectId);
        var homework = subject.Homeworks.FindById(request.HomeworkId);

        homework.Deadline = request.HomeworkDeadline;

        await _context.SaveChangesAsync(cancellationToken);
        return homework;
    }
}