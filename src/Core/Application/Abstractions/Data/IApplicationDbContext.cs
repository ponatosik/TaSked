using TaSked.Domain;
using Microsoft.EntityFrameworkCore;

namespace TaSked.Application.Data;

public interface IApplicationDbContext
{
    DbSet<Group> Groups { get; set; }
    DbSet<User> Users { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
