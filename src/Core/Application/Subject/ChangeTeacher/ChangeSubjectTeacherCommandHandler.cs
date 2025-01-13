using TaSked.Application.Data;
using TaSked.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaSked.Application.Exceptions;

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
        var groupId = user.GroupId ?? throw new UserIsNotGroupMemberException(user.Id, Guid.Empty);
        var group = _context.Groups
            .Include(g => g.Subjects)
            .FindOrThrow(groupId);
        var subject = group.Subjects.FindOrThrow(request.SubjectId);

        subject.Teacher = request.NewSubjectTeacher;

        await _context.SaveChangesAsync(cancellationToken);
        return UpdateSubjectDTO.From(subject);
    }
}