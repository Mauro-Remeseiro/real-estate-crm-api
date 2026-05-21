using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using RealEstateCrmApi.Application.Common.Exceptions;
using RealEstateCrmApi.Application.Common.Interfaces;
using RealEstateCrmApi.Domain.Entities;
using RealEstateCrmApi.Domain.Enums;

namespace RealEstateCrmApi.Application.Clients;

public partial class ClientService : IClientService
{
    private readonly IApplicationDbContext _context;

    public ClientService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<ClientDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var clients = await _context.Clients
            .AsNoTracking()
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync(cancellationToken);

        return clients.Select(MapToDto).ToList();
    }

    public async Task<ClientDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var client = await _context.Clients
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        return client is null ? null : MapToDto(client);
    }

    public async Task<ClientDto> CreateAsync(
        CreateClientRequest request,
        CancellationToken cancellationToken = default)
    {
        ValidateCreateRequest(request);

        var client = new Client
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Email = NormalizeOptionalString(request.Email),
            Phone = NormalizeOptionalString(request.Phone),
            Type = request.Type ?? ClientType.Buyer,
            Notes = NormalizeOptionalNotes(request.Notes),
            CreatedAt = DateTime.UtcNow
        };

        _context.Clients.Add(client);
        await _context.SaveChangesAsync(cancellationToken);

        return MapToDto(client);
    }

    public async Task<ClientDto?> UpdateAsync(
        Guid id,
        UpdateClientRequest request,
        CancellationToken cancellationToken = default)
    {
        ValidateUpdateRequest(request);

        var client = await _context.Clients
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (client is null)
        {
            return null;
        }

        client.FirstName = request.FirstName.Trim();
        client.LastName = request.LastName.Trim();
        client.Email = NormalizeOptionalString(request.Email);
        client.Phone = NormalizeOptionalString(request.Phone);
        client.Type = request.Type;
        client.Notes = NormalizeOptionalNotes(request.Notes);
        client.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return MapToDto(client);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var client = await _context.Clients
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (client is null)
        {
            return false;
        }

        _context.Clients.Remove(client);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    private static void ValidateCreateRequest(CreateClientRequest request)
    {
        ValidateName(request.FirstName, nameof(CreateClientRequest.FirstName));
        ValidateName(request.LastName, nameof(CreateClientRequest.LastName));
        ValidateEmail(request.Email);

        if (request.Type.HasValue && !Enum.IsDefined(typeof(ClientType), request.Type.Value))
        {
            throw new ValidationException("Type is invalid.");
        }
    }

    private static void ValidateUpdateRequest(UpdateClientRequest request)
    {
        ValidateName(request.FirstName, nameof(UpdateClientRequest.FirstName));
        ValidateName(request.LastName, nameof(UpdateClientRequest.LastName));
        ValidateEmail(request.Email);

        if (!Enum.IsDefined(typeof(ClientType), request.Type))
        {
            throw new ValidationException("Type is invalid.");
        }
    }

    private static void ValidateName(string value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ValidationException($"{fieldName} is required.");
        }
    }

    private static void ValidateEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return;
        }

        if (!EmailRegex().IsMatch(email.Trim()))
        {
            throw new ValidationException("Email format is invalid.");
        }
    }

    private static string NormalizeOptionalString(string? value) =>
        string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();

    private static string? NormalizeOptionalNotes(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static ClientDto MapToDto(Client client) =>
        new()
        {
            Id = client.Id,
            FirstName = client.FirstName,
            LastName = client.LastName,
            Email = client.Email,
            Phone = client.Phone,
            Type = client.Type,
            Notes = client.Notes,
            CreatedAt = client.CreatedAt,
            UpdatedAt = client.UpdatedAt
        };

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase)]
    private static partial Regex EmailRegex();
}
