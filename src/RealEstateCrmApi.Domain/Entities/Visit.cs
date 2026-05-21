using RealEstateCrmApi.Domain.Common;
using RealEstateCrmApi.Domain.Enums;

namespace RealEstateCrmApi.Domain.Entities;

public class Visit : BaseEntity
{
    public Guid PropertyId { get; set; }

    public Guid ClientId { get; set; }

    public Guid AgentId { get; set; }

    public DateTime ScheduledAt { get; set; }

    public VisitStatus Status { get; set; }

    public string Notes { get; set; } = string.Empty;
}
