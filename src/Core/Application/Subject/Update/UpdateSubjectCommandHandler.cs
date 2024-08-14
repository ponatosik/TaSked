using TaSked.Application.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace TaSked.Application;

public class UpdateSubjectCommandHandler : IRequestHandler<UpdateSubjectCommand, UpdateSubjectDTO>
{
    private readonly IApplicationDbContext _context;

    public UpdateSubjectCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UpdateSubjectDTO> Handle(UpdateSubjectCommand request, CancellationToken cancellationToken)
    {
        var user = _context.Users.FindById(request.UserId);
        var group = _context.Groups.Include(g => g.Subjects).FindById(user.GroupId!.Value);
        var subject = group.Subjects.FindById(request.SubjectId);

        request.SubjectName.IfSet(name => subject.Name = name);
        request.SubjectTeacher.IfSet(teacher => subject.Teacher = teacher);

        await _context.SaveChangesAsync(cancellationToken);
        return UpdateSubjectDTO.From(subject);
    }
}