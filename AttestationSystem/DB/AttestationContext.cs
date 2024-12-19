using Microsoft.EntityFrameworkCore;

namespace AttestationSystem.DB;

public partial class AttestationContext : DbContext
{
    public AttestationContext()
    {
    }

    public AttestationContext(DbContextOptions<AttestationContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Аттестации> Аттестацииs { get; set; }

    public virtual DbSet<ВидыРабот> ВидыРаботs { get; set; }

    public virtual DbSet<Группы> Группыs { get; set; }

    public virtual DbSet<Даты> Датыs { get; set; }

    public virtual DbSet<Кафедры> Кафедрыs { get; set; }

    public virtual DbSet<Предметы> Предметыs { get; set; }

    public virtual DbSet<Преподаватели> Преподавателиs { get; set; }

    public virtual DbSet<Семестры> Семестрыs { get; set; }

    public virtual DbSet<Студенты> Студентыs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=attestation;Username=postgres;Password=root");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Аттестации>(entity =>
        {
            entity.HasKey(a => a.Id).HasName("Аттестации_pkey"); 
            entity.ToTable("Аттестации");
            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.ВидыРаботВидРаботы).HasColumnName("видыРабот_видРаботы");
            entity.Property(e => e.ДатыДата).HasColumnName("даты_дата");
            entity.Property(e => e.Оценка).HasColumnName("оценка");
            entity.Property(e => e.ПредметыНазваниеДисциплины).HasColumnName("предметы_названиеДисциплины");
            entity.Property(e => e.ПреподавателиНомерСотрудника).HasColumnName("преподаватели_номерСотрудника");
            entity.Property(e => e.СеместрыСеместр).HasColumnName("семестры_семестр");
            entity.Property(e => e.СтудентыНомерСтуденческого).HasColumnName("студенты_номерСтуденческого");

            entity.HasOne(d => d.ВидыРаботВидРаботыNavigation).WithMany()
                .HasForeignKey(d => d.ВидыРаботВидРаботы)
                .HasConstraintName("видыРабот_FK");

            entity.HasOne(d => d.ДатыДатаNavigation).WithMany()
                .HasForeignKey(d => d.ДатыДата)
                .HasConstraintName("даты_FK");

            entity.HasOne(d => d.ПредметыНазваниеДисциплиныNavigation).WithMany()
                .HasForeignKey(d => d.ПредметыНазваниеДисциплины)
                .HasConstraintName("предметы_FK");

            entity.HasOne(d => d.ПреподавателиНомерСотрудникаNavigation).WithMany()
                .HasForeignKey(d => d.ПреподавателиНомерСотрудника)
                .HasConstraintName("преподаватели_FK");

            entity.HasOne(d => d.СеместрыСеместрNavigation).WithMany()
                .HasForeignKey(d => d.СеместрыСеместр)
                .HasConstraintName("семестры_FK");

            entity.HasOne(d => d.СтудентыНомерСтуденческогоNavigation).WithMany()
                .HasForeignKey(d => d.СтудентыНомерСтуденческого)
                .HasConstraintName("студенты_FK");
        });

        modelBuilder.Entity<ВидыРабот>(entity =>
        {
            entity.HasKey(e => e.ВидРаботы).HasName("ВидыРабот_pkey");

            entity.ToTable("ВидыРабот");

            entity.Property(e => e.ВидРаботы).HasColumnName("видРаботы");
        });

        modelBuilder.Entity<Группы>(entity =>
        {
            entity.HasKey(e => e.НомерГруппы).HasName("группа_pkey");

            entity.ToTable("Группы");

            entity.Property(e => e.НомерГруппы).HasColumnName("номерГруппы");
        });

        modelBuilder.Entity<Даты>(entity =>
        {
            entity.HasKey(e => e.Дата).HasName("Даты_pkey");

            entity.ToTable("Даты");

            entity.Property(e => e.Дата).HasColumnName("дата");
        });

        modelBuilder.Entity<Кафедры>(entity =>
        {
            entity.HasKey(e => e.НазваниеКафедры).HasName("Кафедры_pkey");

            entity.ToTable("Кафедры");

            entity.Property(e => e.НазваниеКафедры).HasColumnName("названиеКафедры");
        });

        modelBuilder.Entity<Предметы>(entity =>
        {
            entity.HasKey(e => e.НазваниеДисциплины).HasName("Предметы_pkey");

            entity.ToTable("Предметы");

            entity.Property(e => e.НазваниеДисциплины).HasColumnName("названиеДисциплины");
        });

        modelBuilder.Entity<Преподаватели>(entity =>
        {
            entity.HasKey(e => e.НомерСотрудника).HasName("Преподаватели_pkey");

            entity.ToTable("Преподаватели");

            entity.Property(e => e.НомерСотрудника)
                .ValueGeneratedNever()
                .HasColumnName("номерСотрудника");
            entity.Property(e => e.Имя).HasColumnName("имя");
            entity.Property(e => e.КафедрыНазваниеКафедры).HasColumnName("кафедры_названиеКафедры");
            entity.Property(e => e.Отчество).HasColumnName("отчество");
            entity.Property(e => e.Пароль).HasColumnName("пароль");
            entity.Property(e => e.Фамилия).HasColumnName("фамилия");

            entity.HasOne(d => d.КафедрыНазваниеКафедрыNavigation).WithMany(p => p.Преподавателиs)
                .HasForeignKey(d => d.КафедрыНазваниеКафедры)
                .HasConstraintName("кафедры_FK");
        });

        modelBuilder.Entity<Семестры>(entity =>
        {
            entity.HasKey(e => e.Семестр).HasName("Семестры_pkey");

            entity.ToTable("Семестры");

            entity.Property(e => e.Семестр)
                .ValueGeneratedNever()
                .HasColumnName("семестр");
        });

        modelBuilder.Entity<Студенты>(entity =>
        {
            entity.HasKey(e => e.НомерСтуденческого).HasName("Студенты_pkey");

            entity.ToTable("Студенты");

            entity.Property(e => e.НомерСтуденческого)
                .ValueGeneratedNever()
                .HasColumnName("номерСтуденческого");
            entity.Property(e => e.ГруппыНомерГруппы).HasColumnName("группы_номерГруппы");
            entity.Property(e => e.Имя).HasColumnName("имя");
            entity.Property(e => e.КафедрыНазваниеКафедры).HasColumnName("кафедры_названиеКафедры");
            entity.Property(e => e.Отчество).HasColumnName("отчество");
            entity.Property(e => e.Пароль).HasColumnName("пароль");
            entity.Property(e => e.Фамилия).HasColumnName("фамилия");

            entity.HasOne(d => d.ГруппыНомерГруппыNavigation).WithMany(p => p.Студентыs)
                .HasForeignKey(d => d.ГруппыНомерГруппы)
                .HasConstraintName("группы_FK");

            entity.HasOne(d => d.КафедрыНазваниеКафедрыNavigation).WithMany(p => p.Студентыs)
                .HasForeignKey(d => d.КафедрыНазваниеКафедры)
                .HasConstraintName("кафедры_FK");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
