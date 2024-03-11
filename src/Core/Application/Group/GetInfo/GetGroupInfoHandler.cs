using MediatR;
using TaSked.Application.Data;
using TaSked.Domain;

namespace TaSked.Application;

public class GetGroupInfoHandler : IRequestHandler<GetGroupInfoQuery, Group>
{
    private readonly IApplicationDbContext _context;

    public GetGroupInfoHandler(IApplicationDbContext context)
    {
        _context = context;
    }

	public Task<Group> Handle(GetGroupInfoQuery request, CancellationToken cancellationToken)
	{
		var group = _context.Groups.FindById(request.GroupId);

		return Task.FromResult(group);
	}
 }