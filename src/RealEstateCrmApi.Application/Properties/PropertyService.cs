using Microsoft.EntityFrameworkCore;
using RealEstateCrmApi.Application.Common.Exceptions;
using RealEstateCrmApi.Application.Common.Interfaces;
using RealEstateCrmApi.Domain.Entities;
using RealEstateCrmApi.Domain.Enums;

namespace RealEstateCrmApi.Application.Properties;

public class PropertyService : IPropertyService
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public PropertyService(
        IApplicationDbContext context,
        ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<IReadOnlyList<PropertyDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var properties = await _context.Properties
            .AsNoTracking()
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(cancellationToken);

        return properties.Select(MapToDto).ToList();
    }

    public async Task<PropertyDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var property = await _context.Properties
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        return property is null ? null : MapToDto(property);
    }

    public async Task<PropertyDto> CreateAsync(
        CreatePropertyRequest request,
        CancellationToken cancellationToken = default)
    {
        ValidateCreateRequest(request);

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

        var property = MapToEntity(request, user.Id);

        _context.Properties.Add(property);
        await _context.SaveChangesAsync(cancellationToken);

        return MapToDto(property);
    }

    private static void ValidateCreateRequest(CreatePropertyRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            throw new ValidationException("Title is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Address))
        {
            throw new ValidationException("Address is required.");
        }

        if (string.IsNullOrWhiteSpace(request.City))
        {
            throw new ValidationException("City is required.");
        }

        if (request.Price <= 0)
        {
            throw new ValidationException("Price must be greater than zero.");
        }

        if (request.SquareMeters <= 0)
        {
            throw new ValidationException("SquareMeters must be greater than zero.");
        }
    }

    private static Property MapToEntity(CreatePropertyRequest request, Guid assignedUserId)
    {
        return new Property
        {
            Id = Guid.NewGuid(),
            Title = request.Title.Trim(),
            Description = request.Description?.Trim() ?? string.Empty,
            Address = request.Address.Trim(),
            City = request.City.Trim(),
            Price = request.Price,
            Type = request.Type,
            Status = request.Status ?? PropertyStatus.Available,
            Bedrooms = request.Bedrooms,
            Bathrooms = request.Bathrooms,
            SquareMeters = request.SquareMeters,
            AssignedUserId = assignedUserId,
            CreatedAt = DateTime.UtcNow
        };
    }

    private static PropertyDto MapToDto(Property property)
    {
        return new PropertyDto
        {
            Id = property.Id,
            Title = property.Title,
            Description = property.Description,
            Address = property.Address,
            City = property.City,
            Price = property.Price,
            Type = property.Type,
            Status = property.Status,
            Bedrooms = property.Bedrooms,
            Bathrooms = property.Bathrooms,
            SquareMeters = property.SquareMeters,
            AssignedUserId = property.AssignedUserId,
            CreatedAt = property.CreatedAt,
            UpdatedAt = property.UpdatedAt
        };
    }
}
