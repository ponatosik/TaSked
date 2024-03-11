using TaSked.Application.Data;
using TaSked.Domain;
using MediatR;

namespace TaSked.Application;

public class ChangeSubjectNameCommandHandler : IRequestHandler<ChangeSubjectNameCommand, SubjectDTO>
{
    private readonly IApplicationDbContext _context;

    public ChangeSubjectNameCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SubjectDTO> Handle(ChangeSubjectNameCommand request, CancellationToken cancellationToken)
    {
        var user = _context.Users.FindById(request.UserId);
        var group = _context.Groups.FindById(user.GroupId.Value);
        var subject = group.Subjects.FindById(request.SubjectId);

        subject.Name = request.NewSubjectName;

        await _context.SaveChangesAsync(cancellationToken);
        return SubjectDTO.From(subject);
    }
}