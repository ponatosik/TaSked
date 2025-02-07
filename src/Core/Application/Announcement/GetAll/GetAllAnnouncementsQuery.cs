using MediatR;
using TaSked.Domain;

namespace TaSked.Application;

public record GetAllAnnouncementsQuery(Guid UserId) : IRequest<List<Announcement>>;