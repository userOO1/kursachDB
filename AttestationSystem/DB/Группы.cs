using System;
using System.Collections.Generic;

namespace AttestationSystem;

public partial class Группы
{
    public string НомерГруппы { get; set; } = null!;

    public virtual ICollection<Студенты> Студентыs { get; set; } = new List<Студенты>();
}
