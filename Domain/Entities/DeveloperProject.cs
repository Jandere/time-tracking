using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities;

public class DeveloperProject : BaseEntity<int>
{
    public string DeveloperId { get; set; } = null!;

    public int ProjectId { get; set; }

    [ForeignKey(nameof(DeveloperId))]
    public AppUser? Developer { get; set; }

    [ForeignKey(nameof(ProjectId))]
    public Project Project { get; set; } = null!;
}