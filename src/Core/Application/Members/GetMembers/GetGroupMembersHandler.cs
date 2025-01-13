using MediatR;
using Microsoft.EntityFrameworkCore;
using TaSked.Application.Data;
using TaSked.Application.Exceptions;
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
        var user = _context.Users.FindOrThrow(request.UserId);
        if(user.GroupId != request.GroupId)
        {
            throw new UserIsNotGroupMemberException(request.UserId, request.GroupId);
        }
        var group = _context.Groups
            .Include(e => e.Members)
            .AsNoTracking()
            .FindOrThrow(request.GroupId);

        return Task.FromResult(group.Members.ToList());
    }
}