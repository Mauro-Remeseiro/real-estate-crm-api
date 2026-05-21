namespace RealEstateCrmApi.Application.Visits;

public class CreateVisitRequest
{
    public Guid PropertyId { get; set; }

    public Guid ClientId { get; set; }

    public DateTime ScheduledAt { get; set; }

    public string? Notes { get; set; }
}
