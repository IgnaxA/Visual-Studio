using System;
using System.Collections.Generic;

namespace Practice
    ;

public partial class Faculty
{
    public int Id { get; set; }

    public string FacultyName { get; set; } = null!;

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
