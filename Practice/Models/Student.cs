using System;
using System.Collections.Generic;

namespace Practice;

public partial class Student
{
    public int Id { get; set; }

    public int TeamId { get; set; }

    public string Initials { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int FacultyId { get; set; }

    public int CourseId { get; set; }

    public int? RoleId { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual Faculty Faculty { get; set; } = null!;

    public virtual Role? Role { get; set; }

    public virtual Team Team { get; set; } = null!;
}
