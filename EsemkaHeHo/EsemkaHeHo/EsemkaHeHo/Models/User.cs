using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EsemkaHeHo.Models;

public partial class User
{
    public Guid Id { get; set; }

    public Guid? DepartmentId { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EnumRole Role { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public DateOnly? HireDate { get; set; }

    public decimal? Salary { get; set; }

    public bool IsActive { get; set; }

    public string? Photo { get; set; }

    public virtual Department? Department { get; set; }

    [JsonIgnore]
    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

    [JsonIgnore]
    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
public enum EnumRole
{
    Developer = 0,
    Designer = 1,
    Manager = 2,
    QA = 3,
    Admin = 4
}