using System;
using System.Collections.Generic;

namespace Practice;

public partial class Theme
{
    public int Id { get; set; }

    public int TeacherId { get; set; }

    public string ThemeFormulation { get; set; } = null!;

    public virtual Teacher Teacher { get; set; } = null!;

    public virtual ICollection<Team> Teams { get; set; } = new List<Team>();
}
