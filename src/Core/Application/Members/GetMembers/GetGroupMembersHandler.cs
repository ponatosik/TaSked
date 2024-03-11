using MediatR;
using Microsoft.EntityFrameworkCore;
using TaSked.Application.Data;
using TaSked.Domain;

namespace TaSked.Application;

public class GetGroupMembersHandler : IRequestHandler<GetGroupMembersQuery, List<User>>
{
    private readonly IApplicationDbContext _context;

    public GetGroupMembersHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task<List<User>> Handle(GetGroupMembersQuery request, CancellationToken cancellationToken)
    {
        var user = _context.Users.FindById(request.UserId);
        var group = _context.Groups.Include(e => e.Members).FindById(user.GroupId.Value);

        return Task.FromResult(group.Members.ToList());
    }
}