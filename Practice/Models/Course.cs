using System;
using System.Collections.Generic;

namespace Practice;

public partial class Course
{
    public int Id { get; set; }

    public string Course1 { get; set; } = null!;

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
