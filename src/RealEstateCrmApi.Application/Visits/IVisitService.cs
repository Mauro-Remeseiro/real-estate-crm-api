namespace RealEstateCrmApi.Application.Visits;

public interface IVisitService
{
    Task<IReadOnlyList<VisitDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<VisitDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<VisitDto> CreateAsync(CreateVisitRequest request, CancellationToken cancellationToken = default);

    Task<VisitDto?> UpdateAsync(Guid id, UpdateVisitRequest request, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
