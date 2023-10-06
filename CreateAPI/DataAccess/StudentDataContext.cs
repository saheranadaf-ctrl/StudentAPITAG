using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CreateAPI.DataAccess;

public partial class StudentDataContext : DbContext
{
    public StudentDataContext()
    {
    }

    public StudentDataContext(DbContextOptions<StudentDataContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Student> Students { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__Student__A2F4E9ACFA492539");

            entity.ToTable("Student");

            entity.Property(e => e.StudentId)
                .HasMaxLength(255)
                .HasColumnName("Student_ID");
            entity.Property(e => e.Class).HasMaxLength(255);
            entity.Property(e => e.Gender)
                .HasMaxLength(255)
                .HasColumnName("gender");
            entity.Property(e => e.GradeId)
                .HasMaxLength(255)
                .HasColumnName("GradeID");
            entity.Property(e => e.NationalIty)
                .HasMaxLength(255)
                .HasColumnName("NationalITy");
            entity.Property(e => e.ParentAnsweringSurvey).HasMaxLength(255);
            entity.Property(e => e.ParentschoolSatisfaction).HasMaxLength(255);
            entity.Property(e => e.PlaceofBirth).HasMaxLength(255);
            entity.Property(e => e.Raisedhands).HasColumnName("raisedhands");
            entity.Property(e => e.Relation).HasMaxLength(255);
            entity.Property(e => e.SectionId)
                .HasMaxLength(255)
                .HasColumnName("SectionID");
            entity.Property(e => e.Semester).HasMaxLength(255);
            entity.Property(e => e.StageId)
                .HasMaxLength(255)
                .HasColumnName("StageID");
            entity.Property(e => e.StudentAbsenceDays).HasMaxLength(255);
            entity.Property(e => e.StudentMarks).HasColumnName("Student Marks");
            entity.Property(e => e.Topic).HasMaxLength(255);
            entity.Property(e => e.VisItedResources).HasColumnName("VisITedResources");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
