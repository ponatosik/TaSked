using TaSked.Application.Data;
using TaSked.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
        var group = _context.Groups.Include(g => g.Subjects).FindOrThrow(user.GroupId.Value);
        var subject = group.Subjects.FindOrThrow(request.SubjectId);

        subject.Name = request.NewSubjectName;

        await _context.SaveChangesAsync(cancellationToken);
        return UpdateSubjectDTO.From(subject);
    }
}