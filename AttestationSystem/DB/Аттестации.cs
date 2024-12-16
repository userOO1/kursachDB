using System;
using System.Collections.Generic;

namespace AttestationSystem;

public partial class Аттестации
{
    public int? СтудентыНомерСтуденческого { get; set; }

    public DateOnly? ДатыДата { get; set; }

    public int? ПреподавателиНомерСотрудника { get; set; }

    public string? ПредметыНазваниеДисциплины { get; set; }

    public int? СеместрыСеместр { get; set; }

    public string? ВидыРаботВидРаботы { get; set; }

    public int? Оценка { get; set; }

    public virtual ВидыРабот? ВидыРаботВидРаботыNavigation { get; set; }

    public virtual Даты? ДатыДатаNavigation { get; set; }

    public virtual Предметы? ПредметыНазваниеДисциплиныNavigation { get; set; }

    public virtual Преподаватели? ПреподавателиНомерСотрудникаNavigation { get; set; }

    public virtual Семестры? СеместрыСеместрNavigation { get; set; }

    public virtual Студенты? СтудентыНомерСтуденческогоNavigation { get; set; }
}
