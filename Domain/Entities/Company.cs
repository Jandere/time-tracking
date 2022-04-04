using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities;

public class Company : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;

    public string AdministratorId { get; set; } = null!;

    [ForeignKey(nameof(AdministratorId))]
    public AppUser? Administrator { get; set; }

    public IList<Project> Projects { get; set; } = new List<Project>();

    public IList<AppUser> Developers { get; set; } = new List<AppUser>();
}