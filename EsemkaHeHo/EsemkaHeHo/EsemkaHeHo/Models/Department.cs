using System;
using System.Collections.Generic;

namespace EsemkaHeHo.Models;

public partial class Department
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
