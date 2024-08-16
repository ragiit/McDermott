using System;
using System.Collections.Generic;

namespace EsemkaHeHo.Models;

public partial class Task
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime DueDate { get; set; }

    public int CategoryIssue { get; set; }

    public int Priority { get; set; }

    public int Status { get; set; }

    public virtual Project Project { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
