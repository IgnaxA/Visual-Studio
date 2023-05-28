using System;
using System.Collections.Generic;

namespace Practice;

public partial class Degree
{
    public int Id { get; set; }

    public string Formulation { get; set; } = null!;

    public virtual ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
}
