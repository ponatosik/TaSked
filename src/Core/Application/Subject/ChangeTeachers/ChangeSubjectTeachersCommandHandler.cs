using MediatR;
using Microsoft.EntityFrameworkCore;
using TaSked.Application.Data;
using TaSked.Application.Exceptions;
using TaSked.Domain;

namespace TaSked.Application;

public class ChangeSubjectTeacherCommandHandler : IRequestHandler<ChangeSubjectTeachersCommand, UpdateSubjectDTO>
{
    private readonly IApplicationDbContext _context;

    public ChangeSubjectTeacherCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UpdateSubjectDTO> Handle(ChangeSubjectTeachersCommand request,
	    CancellationToken cancellationToken)
    {
        var user = _context.Users.FindOrThrow(request.UserId);
        var groupId = user.GroupId ?? throw new UserIsNotGroupMemberException(user.Id, Guid.Empty);
        var group = _context.Groups
            .Include(g => g.Subjects)
            .FindOrThrow(groupId);
        var subject = group.Subjects.FindOrThrow(request.SubjectId);

        subject.Teachers.Clear();
        subject.Teachers.AddRange(request.NewSubjectTeachers.Select(t =>
	        Teacher.Create(t.FullName, t.Description, t.Email, t.PhoneNumber, t.OnlineMeetingUrl)
        ));

        await _context.SaveChangesAsync(cancellationToken);
        return UpdateSubjectDTO.From(subject);
    }
}