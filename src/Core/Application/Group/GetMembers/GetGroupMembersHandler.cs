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
        var user = _context.Users.FirstOrDefault(user => user.Id == request.UserId);
        if (user == null)
        {
            throw new Exception("No such user found");
        }
        if (user.GroupId == null)
        {
            throw new Exception("User is not a member of a group");
        }

        var group = _context.Groups.Include(e => e.Members).FirstOrDefault(group => group.Id == user.GroupId);
        if (group == null)
        {
            throw new Exception("Unknown Group");
        }

        return Task.FromResult(group.Members.ToList());
    }
}