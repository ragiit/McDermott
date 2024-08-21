using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EsemkaHeHo.Models;

public partial class Project
{
    public Guid Id { get; set; }

    public Guid ProjectManagerId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int Status { get; set; }

    public virtual User ProjectManager { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
