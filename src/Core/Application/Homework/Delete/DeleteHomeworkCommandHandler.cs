using TaSked.Application.Data;
using TaSked.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace TaSked.Application;

public class DeleteHomeworkCommandHandler : IRequestHandler<DeleteHomeworkCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteHomeworkCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteHomeworkCommand request, CancellationToken cancellationToken)
    {
        var user = _context.Users.FindById(request.UserId);
        var group = _context.Groups
            .Include(group => group.Subjects)
            .ThenInclude(subject => subject.Homeworks)
            .FindById(user.GroupId.Value);
        var subject = group.Subjects.FindById(request.SubjectId);
        var homework = subject.Homeworks.FindById(request.HomeworkId);
        
        subject.Homeworks.Remove(homework);
        
        await _context.SaveChangesAsync(cancellationToken);
    }
}