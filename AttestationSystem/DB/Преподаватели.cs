using System;
using System.Collections.Generic;

namespace AttestationSystem;

public partial class Преподаватели
{
    public int НомерСотрудника { get; set; }

    public string? Фамилия { get; set; }

    public string? Имя { get; set; }

    public string? Отчество { get; set; }

    public string? КафедрыНазваниеКафедры { get; set; }

    public string? Пароль { get; set; }

    public virtual Кафедры? КафедрыНазваниеКафедрыNavigation { get; set; }
}
