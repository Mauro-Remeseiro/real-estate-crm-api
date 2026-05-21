using RealEstateCrmApi.Domain.Common;
using RealEstateCrmApi.Domain.Enums;

namespace RealEstateCrmApi.Domain.Entities;

public class Client : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public ClientType Type { get; set; }

    public string? Notes { get; set; }
}
