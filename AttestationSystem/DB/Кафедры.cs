using System;
using System.Collections.Generic;

namespace AttestationSystem;

public partial class Кафедры
{
    public string НазваниеКафедры { get; set; } = null!;

    public virtual ICollection<Преподаватели> Преподавателиs { get; set; } = new List<Преподаватели>();

    public virtual ICollection<Студенты> Студентыs { get; set; } = new List<Студенты>();
}
