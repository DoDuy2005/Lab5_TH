using Lab5_TH.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Lab5_TH.Data
{
    public class SchoolContext : DbContext
    {
        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options) { }

        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Learner> Learners { get; set; }
        public virtual DbSet<Enrollment> Enrollments { get; set; }
        public virtual DbSet<Major> Majors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Major>().ToTable("Major");
            modelBuilder.Entity<Course>().ToTable("Course");
            modelBuilder.Entity<Learner>().ToTable("Learner");
            modelBuilder.Entity<Enrollment>().ToTable("Enrollment");

            modelBuilder.Entity<Learner>()
                .HasOne(l => l.Major)
                .WithMany(m => m.Learners)
                .HasForeignKey(l => l.MajorID);

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Learner)
                .WithMany(l => l.Enrollments)
                .HasForeignKey(e => e.LearnerID);

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseID);
        }
    }
}