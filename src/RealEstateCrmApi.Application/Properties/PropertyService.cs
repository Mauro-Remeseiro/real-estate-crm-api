using Microsoft.EntityFrameworkCore;
using RealEstateCrmApi.Application.Common.Exceptions;
using RealEstateCrmApi.Application.Common.Interfaces;
using RealEstateCrmApi.Domain.Entities;
using RealEstateCrmApi.Domain.Enums;

namespace RealEstateCrmApi.Application.Properties;

public class PropertyService : IPropertyService
{
    private readonly IApplicationDbContext _context;

    public PropertyService(IApplicationDbContext context)
    {
        _context = context;
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

        if (request.AssignedUserId.HasValue)
        {
            var userExists = await _context.Users
                .AnyAsync(u => u.Id == request.AssignedUserId.Value, cancellationToken);

            if (!userExists)
            {
                throw new ValidationException("Assigned user does not exist.");
            }
        }

        var property = MapToEntity(request);

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

    private static Property MapToEntity(CreatePropertyRequest request)
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
            AssignedUserId = request.AssignedUserId,
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
