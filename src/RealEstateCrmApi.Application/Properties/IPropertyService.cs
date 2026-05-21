namespace RealEstateCrmApi.Application.Properties;

public interface IPropertyService
{
    Task<IReadOnlyList<PropertyDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<PropertyDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<PropertyDto> CreateAsync(CreatePropertyRequest request, CancellationToken cancellationToken = default);
}
