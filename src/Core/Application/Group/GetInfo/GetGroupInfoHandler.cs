﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using TaSked.Application.Data;
using TaSked.Domain;

namespace TaSked.Application;

public class GetGroupInfoHandler : IRequestHandler<GetGroupInfoQuery, GroupDTO>
{
    private readonly IApplicationDbContext _context;

    public GetGroupInfoHandler(IApplicationDbContext context)
    {
        _context = context;
    }

	public Task<GroupDTO> Handle(GetGroupInfoQuery request, CancellationToken cancellationToken)
	{
		var group = _context.Groups
			.AsNoTracking()
			.FindOrThrow(request.GroupId);

		return Task.FromResult(GroupDTO.From(group));
	}
 }