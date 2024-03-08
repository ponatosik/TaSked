using TaSked.Application.Data;
using TaSked.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace TaSked.Application;

public class ChangeHomeworkDescriptionCommandHandler : IRequestHandler<ChangeHomeworkDescriptionCommand, Homework>
{
    private readonly IApplicationDbContext _context;

    public ChangeHomeworkDescriptionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Homework> Handle(ChangeHomeworkDescriptionCommand request, CancellationToken cancellationToken)
    {
        var user = _context.Users.FindById(request.UserId);
        var group = _context.Groups.Include(group => group.Subjects).FindById(user.GroupId.Value);
        var subject = group.Subjects.FindById(request.SubjectId);
        var homework = subject.Homeworks.FindById(request.HomeworkId);

        homework.Description = request.HomeworkDescription;

        await _context.SaveChangesAsync(cancellationToken);
        return homework;
    }
}