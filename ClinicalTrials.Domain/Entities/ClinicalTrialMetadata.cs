using System.ComponentModel.DataAnnotations;

namespace ClinicalTrials.Domain.Entities;

public class ClinicalTrialMetadata
{
    [Key]
    public required string TrialId { get; set; }
    public required string Title { get; set; }
    public required DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public int Participants { get; set; }
    public required string Status { get; set; }
    public int? Duration { get; set; }
}
