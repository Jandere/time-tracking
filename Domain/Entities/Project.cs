using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities;

public class Project : BaseEntity<int>
{
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? ImgPath { get; set; }

    public string? TeamLeadId { get; set; }
    
    public int CompanyId { get; set; }
    
    [ForeignKey(nameof(TeamLeadId))]
    public AppUser? TeamLead { get; set; }

    [ForeignKey(nameof(CompanyId))] 
    public Company? Company { get; set; }

    public IList<DeveloperProject> Developers { get; set; } = new List<DeveloperProject>();
}