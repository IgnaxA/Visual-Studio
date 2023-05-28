using System;
using System.Collections.Generic;

namespace Practice;

public partial class Consultation
{
    public int Id { get; set; }

    public int DeadlineId { get; set; }

    public DateTime Date { get; set; }

    public byte AttendanceMark { get; set; }

    public virtual Deadline Deadline { get; set; } = null!;
}
