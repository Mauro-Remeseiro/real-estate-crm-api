using Microsoft.EntityFrameworkCore;
using RealEstateCrmApi.Application.Common.Exceptions;
using RealEstateCrmApi.Application.Common.Interfaces;
using RealEstateCrmApi.Domain.Entities;
using RealEstateCrmApi.Domain.Enums;

namespace RealEstateCrmApi.Application.Visits;

public class VisitService : IVisitService
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public VisitService(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<IReadOnlyList<VisitDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var visits = await _context.Visits
            .AsNoTracking()
            .OrderBy(v => v.ScheduledAt)
            .ToListAsync(cancellationToken);

        return visits.Select(MapToDto).ToList();
    }

    public async Task<VisitDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var visit = await _context.Visits
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);

        return visit is null ? null : MapToDto(visit);
    }

    public async Task<VisitDto> CreateAsync(
        CreateVisitRequest request,
        CancellationToken cancellationToken = default)
    {
        ValidateCreateRequest(request);

        var agent = await GetActiveAuthenticatedUserAsync(cancellationToken);

        await EnsurePropertyExistsAsync(request.PropertyId, cancellationToken);
        await EnsureClientExistsAsync(request.ClientId, cancellationToken);

        var visit = new Visit
        {
            Id = Guid.NewGuid(),
            PropertyId = request.PropertyId,
            ClientId = request.ClientId,
            AgentId = agent.Id,
            ScheduledAt = request.ScheduledAt,
            Status = VisitStatus.Scheduled,
            Notes = NormalizeNotes(request.Notes),
            CreatedAt = DateTime.UtcNow
        };

        _context.Visits.Add(visit);
        await _context.SaveChangesAsync(cancellationToken);

        return MapToDto(visit);
    }

    public async Task<VisitDto?> UpdateAsync(
        Guid id,
        UpdateVisitRequest request,
        CancellationToken cancellationToken = default)
    {
        ValidateUpdateRequest(request);

        var visit = await _context.Visits
            .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);

        if (visit is null)
        {
            return null;
        }

        visit.ScheduledAt = request.ScheduledAt;
        visit.Status = request.Status;
        visit.Notes = NormalizeNotes(request.Notes);
        visit.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return MapToDto(visit);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var visit = await _context.Visits
            .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);

        if (visit is null)
        {
            return false;
        }

        _context.Visits.Remove(visit);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    private async Task<User> GetActiveAuthenticatedUserAsync(CancellationToken cancellationToken)
    {
        if (!_currentUser.IsAuthenticated || _currentUser.UserId is null)
        {
            throw new UnauthorizedException("Authentication is required.");
        }

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == _currentUser.UserId.Value, cancellationToken);

        if (user is null || !user.IsActive)
        {
            throw new UnauthorizedException("User account is invalid or inactive.");
        }

        return user;
    }

    private async Task EnsurePropertyExistsAsync(Guid propertyId, CancellationToken cancellationToken)
    {
        var exists = await _context.Properties
            .AnyAsync(p => p.Id == propertyId, cancellationToken);

        if (!exists)
        {
            throw new ValidationException("Property does not exist.");
        }
    }

    private async Task EnsureClientExistsAsync(Guid clientId, CancellationToken cancellationToken)
    {
        var exists = await _context.Clients
            .AnyAsync(c => c.Id == clientId, cancellationToken);

        if (!exists)
        {
            throw new ValidationException("Client does not exist.");
        }
    }

    private static void ValidateCreateRequest(CreateVisitRequest request)
    {
        if (request.PropertyId == Guid.Empty)
        {
            throw new ValidationException("PropertyId is required.");
        }

        if (request.ClientId == Guid.Empty)
        {
            throw new ValidationException("ClientId is required.");
        }

        ValidateScheduledAtInFuture(request.ScheduledAt);
    }

    private static void ValidateUpdateRequest(UpdateVisitRequest request)
    {
        ValidateScheduledAtInFuture(request.ScheduledAt);

        if (!Enum.IsDefined(typeof(VisitStatus), request.Status))
        {
            throw new ValidationException("Status is invalid.");
        }
    }

    private static void ValidateScheduledAtInFuture(DateTime scheduledAt)
    {
        if (scheduledAt <= DateTime.UtcNow)
        {
            throw new ValidationException("ScheduledAt must be in the future.");
        }
    }

    private static string NormalizeNotes(string? notes) =>
        string.IsNullOrWhiteSpace(notes) ? string.Empty : notes.Trim();

    private static VisitDto MapToDto(Visit visit) =>
        new()
        {
            Id = visit.Id,
            PropertyId = visit.PropertyId,
            ClientId = visit.ClientId,
            AgentId = visit.AgentId,
            ScheduledAt = visit.ScheduledAt,
            Status = visit.Status,
            Notes = visit.Notes,
            CreatedAt = visit.CreatedAt,
            UpdatedAt = visit.UpdatedAt
        };
}
