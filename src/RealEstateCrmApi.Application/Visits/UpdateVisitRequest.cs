using RealEstateCrmApi.Domain.Enums;

namespace RealEstateCrmApi.Application.Visits;

public class UpdateVisitRequest
{
    public DateTime ScheduledAt { get; set; }

    public VisitStatus Status { get; set; }

    public string? Notes { get; set; }
}
