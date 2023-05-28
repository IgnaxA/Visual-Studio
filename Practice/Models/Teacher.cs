using System;
using System.Collections.Generic;

namespace Practice;

public partial class Teacher
{
    public int Id { get; set; }

    public string Initials { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int? DegreeId { get; set; }

    public int? EmploymentTypeId { get; set; }

    public virtual Degree? Degree { get; set; }

    public virtual EmploymentType? EmploymentType { get; set; }

    public virtual ICollection<Theme> Themes { get; set; } = new List<Theme>();
}
