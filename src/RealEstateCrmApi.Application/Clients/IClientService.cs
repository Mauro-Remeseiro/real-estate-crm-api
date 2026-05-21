namespace RealEstateCrmApi.Application.Clients;

public interface IClientService
{
    Task<IReadOnlyList<ClientDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<ClientDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ClientDto> CreateAsync(CreateClientRequest request, CancellationToken cancellationToken = default);

    Task<ClientDto?> UpdateAsync(Guid id, UpdateClientRequest request, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
