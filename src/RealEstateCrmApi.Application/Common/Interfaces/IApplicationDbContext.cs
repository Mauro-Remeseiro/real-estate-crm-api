using Microsoft.EntityFrameworkCore;
using RealEstateCrmApi.Domain.Entities;

namespace RealEstateCrmApi.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }

    DbSet<Client> Clients { get; }

    DbSet<Property> Properties { get; }

    DbSet<Visit> Visits { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
