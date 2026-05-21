using RealEstateCrmApi.Domain.Enums;

namespace RealEstateCrmApi.Application.Clients;

public class UpdateClientRequest
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public ClientType Type { get; set; }

    public string? Notes { get; set; }
}
