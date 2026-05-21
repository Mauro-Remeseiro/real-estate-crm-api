using RealEstateCrmApi.Domain.Enums;

namespace RealEstateCrmApi.Application.Properties;

public class PropertyDto
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public PropertyType Type { get; set; }

    public PropertyStatus Status { get; set; }

    public int Bedrooms { get; set; }

    public int Bathrooms { get; set; }

    public int SquareMeters { get; set; }

    public Guid? AssignedUserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
