using System;
using System.Collections.Generic;

namespace AttestationSystem;

public partial class Группы
{
    public string НомерГруппы { get; set; } = null!;
    public string? КафедрыНазваниеКафедры { get; set; }
    
    public virtual Кафедры? КафедрыНазваниеКафедрыNavigation { get; set; }

    public virtual ICollection<Студенты> Студентыs { get; set; } = new List<Студенты>();
}
