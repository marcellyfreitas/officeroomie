namespace WebApi.Models.Dto;

public class UpdateConvenioDto
{
    public string? Name { get; set; }
    public string? Provider { get; set; }
    public int? HealthPlanId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}