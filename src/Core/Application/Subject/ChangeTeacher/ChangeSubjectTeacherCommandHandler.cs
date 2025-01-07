using TaSked.Application.Data;
using TaSked.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace TaSked.Application;

public class ChangeSubjectTeacherCommandHandler : IRequestHandler<ChangeSubjectTeacherCommand, UpdateSubjectDTO>
{
    private readonly IApplicationDbContext _context;

    public ChangeSubjectTeacherCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UpdateSubjectDTO> Handle(ChangeSubjectTeacherCommand request, CancellationToken cancellationToken)
    {
        var user = _context.Users.FindOrThrow(request.UserId);
        var group = _context.Groups.Include(g => g.Subjects).FindOrThrow(user.GroupId.Value);
        var subject = group.Subjects.FindOrThrow(request.SubjectId);

        subject.Teacher = request.NewSubjectTeacher;

        await _context.SaveChangesAsync(cancellationToken);
        return UpdateSubjectDTO.From(subject);
    }
}