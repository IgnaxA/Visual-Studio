using System;
using System.Collections.Generic;

namespace Practice;

public partial class Team
{
    public int Id { get; set; }

    public int ThemeId { get; set; }

    public string MaterialsLink { get; set; } = null!;

    public virtual ICollection<Deadline> Deadlines { get; set; } = new List<Deadline>();

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();

    public virtual Theme Theme { get; set; } = null!;
}
