using System;
using System.Collections.Generic;

namespace Practice;

public partial class Role
{
    public int Id { get; set; }

    public string Role1 { get; set; } = null!;

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
