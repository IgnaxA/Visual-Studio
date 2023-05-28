using System;
using System.Collections.Generic;

namespace Practice;

public partial class Deadline
{
    public int Id { get; set; }

    public int TeamId { get; set; }

    public DateTime DeadLineDate { get; set; }

    public string Commentary { get; set; } = null!;

    public byte AttendanceMark { get; set; }

    public virtual ICollection<Consultation> Consultations { get; set; } = new List<Consultation>();

    public virtual Team Team { get; set; } = null!;
}
