using TaSked.Application.Data;
using TaSked.Domain;
using MediatR;

namespace TaSked.Application;

public class ChangeSubjectTeacherCommandHandler : IRequestHandler<ChangeSubjectTeacherCommand, SubjectDTO>
{
    private readonly IApplicationDbContext _context;

    public ChangeSubjectTeacherCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SubjectDTO> Handle(ChangeSubjectTeacherCommand request, CancellationToken cancellationToken)
    {
        var user = _context.Users.FindById(request.UserId);
        var group = _context.Groups.FindById(user.GroupId.Value);
        var subject = group.Subjects.FindById(request.SubjectId);

        subject.Teacher = request.NewSubjectTeacher;

        await _context.SaveChangesAsync(cancellationToken);
        return SubjectDTO.From(subject);
    }
}