using TaSked.Application.Data;
using TaSked.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace TaSked.Application;

public class ChangeHomeworkTitleCommandHandler : IRequestHandler<ChangeHomeworkTitleCommand, Homework>
{
    private readonly IApplicationDbContext _context;

    public ChangeHomeworkTitleCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Homework> Handle(ChangeHomeworkTitleCommand request, CancellationToken cancellationToken)
    {
        var user = _context.Users.FindById(request.UserId);
        var group = _context.Groups.Include(group => group.Subjects).FindById(user.GroupId.Value);
        var subject = group.Subjects.FindById(request.SubjectId);
        var homework = subject.Homeworks.FindById(request.HomeworkId);

        homework.Title = request.HomeworkTitle;

        await _context.SaveChangesAsync(cancellationToken);
        return homework;
    }
}