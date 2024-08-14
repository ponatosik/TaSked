using TaSked.Application.Data;
using TaSked.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace TaSked.Application;

public class UpdateHomeworkCommandHandler : IRequestHandler<UpdateHomeworkCommand, Homework>
{
    private readonly IApplicationDbContext _context;

    public UpdateHomeworkCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Homework> Handle(UpdateHomeworkCommand request, CancellationToken cancellationToken)
    {
        var user = _context.Users.FindById(request.UserId);
        var group = _context.Groups
            .Include(group => group.Subjects)
            .ThenInclude(subject => subject.Homeworks)
            .FindById(user.GroupId!.Value);
        var subject = group.Subjects.FindById(request.SubjectId);
        var homework = subject.Homeworks.FindById(request.HomeworkId);

        request.HomeworkTitle.IfSet(title => homework.Title = title);
        request.HomeworkDescription.IfSet(description => homework.Description = description);
        request.HomeworkDeadline.IfSet(deadline => homework.Deadline = deadline);
        request.HomeworkSourceUrl.IfSet(sourceUrl => homework.SourceUrl = sourceUrl);

        await _context.SaveChangesAsync(cancellationToken);
        return homework;
    }
}