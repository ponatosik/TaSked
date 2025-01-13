using TaSked.Application.Data;
using TaSked.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaSked.Application.Exceptions;

namespace TaSked.Application;

public class ChangeSubjectNameCommandHandler : IRequestHandler<ChangeSubjectNameCommand, UpdateSubjectDTO>
{
    private readonly IApplicationDbContext _context;

    public ChangeSubjectNameCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UpdateSubjectDTO> Handle(ChangeSubjectNameCommand request, CancellationToken cancellationToken)
    {
        var user = _context.Users.FindOrThrow(request.UserId);
        var groupId = user.GroupId ?? throw new UserIsNotGroupMemberException(user.Id, Guid.Empty);
        var group = _context.Groups
            .Include(g => g.Subjects)
            .FindOrThrow(groupId);
        var subject = group.Subjects.FindOrThrow(request.SubjectId);

        subject.Name = request.NewSubjectName;

        await _context.SaveChangesAsync(cancellationToken);
        return UpdateSubjectDTO.From(subject);
    }
}