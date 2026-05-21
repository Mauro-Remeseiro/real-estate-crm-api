using RealEstateCrmApi.Domain.Enums;

namespace RealEstateCrmApi.Application.Visits;

public class VisitDto
{
    public Guid Id { get; set; }

    public Guid PropertyId { get; set; }

    public Guid ClientId { get; set; }

    public Guid AgentId { get; set; }

    public DateTime ScheduledAt { get; set; }

    public VisitStatus Status { get; set; }

    public string Notes { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
