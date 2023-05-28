using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Practice;

public partial class InformationSystemToRecordProjectActivitiesDatabaseContext : DbContext
{
    public InformationSystemToRecordProjectActivitiesDatabaseContext()
    {
    }

    public InformationSystemToRecordProjectActivitiesDatabaseContext(DbContextOptions<InformationSystemToRecordProjectActivitiesDatabaseContext> options) : base(options)
    {
    }

    public virtual DbSet<Consultation> Consultations { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Deadline> Deadlines { get; set; }

    public virtual DbSet<Degree> Degrees { get; set; }

    public virtual DbSet<EmploymentType> EmploymentTypes { get; set; }

    public virtual DbSet<Faculty> Faculties { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<Theme> Themes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer(new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json").Build().GetConnectionString("Connect"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Consultation>(entity =>
        {
            entity.ToTable("Consultation");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.DeadlineId).HasColumnName("DeadlineID");

            entity.HasOne(d => d.Deadline).WithMany(p => p.Consultations)
                .HasForeignKey(d => d.DeadlineId)
                .HasConstraintName("FK_Consultation_Deadline");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.ToTable("Course");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Course1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Course");
        });

        modelBuilder.Entity<Deadline>(entity =>
        {
            entity.ToTable("Deadline");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Commentary).IsUnicode(false);
            entity.Property(e => e.DeadLineDate).HasColumnType("date");
            entity.Property(e => e.TeamId).HasColumnName("TeamID");

            entity.HasOne(d => d.Team).WithMany(p => p.Deadlines)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("FK_Deadline_Team");
        });

        modelBuilder.Entity<Degree>(entity =>
        {
            entity.ToTable("Degree");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Formulation)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<EmploymentType>(entity =>
        {
            entity.ToTable("EmploymentType");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Formulation)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Faculty>(entity =>
        {
            entity.ToTable("Faculty");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.FacultyName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Role1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Role");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.ToTable("Student");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FacultyId).HasColumnName("FacultyID");
            entity.Property(e => e.Initials)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.TeamId).HasColumnName("TeamID");

            entity.HasOne(d => d.Course).WithMany(p => p.Students)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK_Student_Course");

            entity.HasOne(d => d.Faculty).WithMany(p => p.Students)
                .HasForeignKey(d => d.FacultyId)
                .HasConstraintName("FK_Student_Faculty");

            entity.HasOne(d => d.Role).WithMany(p => p.Students)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Student_Role");

            entity.HasOne(d => d.Team).WithMany(p => p.Students)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("FK_Student_Team");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.ToTable("Teacher");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DegreeId).HasColumnName("DegreeID");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EmploymentTypeId).HasColumnName("EmploymentTypeID");
            entity.Property(e => e.Initials)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Degree).WithMany(p => p.Teachers)
                .HasForeignKey(d => d.DegreeId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Teacher_Degree");

            entity.HasOne(d => d.EmploymentType).WithMany(p => p.Teachers)
                .HasForeignKey(d => d.EmploymentTypeId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Teacher_EmploymentType");
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.ToTable("Team");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MaterialsLink).IsUnicode(false);
            entity.Property(e => e.ThemeId).HasColumnName("ThemeID");

            entity.HasOne(d => d.Theme).WithMany(p => p.Teams)
                .HasForeignKey(d => d.ThemeId)
                .HasConstraintName("FK_Team_Theme");
        });

        modelBuilder.Entity<Theme>(entity =>
        {
            entity.ToTable("Theme");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.TeacherId).HasColumnName("TeacherID");
            entity.Property(e => e.ThemeFormulation)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Teacher).WithMany(p => p.Themes)
                .HasForeignKey(d => d.TeacherId)
                .HasConstraintName("FK_Theme_Teacher");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
