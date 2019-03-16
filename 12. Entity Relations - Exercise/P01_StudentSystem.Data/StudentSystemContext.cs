namespace P01_StudentSystem.Data
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Models;


    public class StudentSystemContext : DbContext
    {
        public StudentSystemContext()
        {

        }

        public StudentSystemContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<Student> Students { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Resource> Resources { get; set; }

        public DbSet<Homework> HomeworkSubmissions { get; set; }

        public DbSet<StudentCourse> StudentCourses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(Config.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnConfiguringStudent(modelBuilder);
            OnConfiguringCourse(modelBuilder);
            OnConfiguringResource(modelBuilder);
            OnConfiguringHomework(modelBuilder);

            OnConfiguringStudentCourses(modelBuilder);
        }

        private void OnConfiguringStudentCourses(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<StudentCourse>(e => 
                {
                    e.HasKey(sc => new { sc.StudentId, sc.CourseId });

                    e
                    .HasOne(sc => sc.Student)
                    .WithMany(s => s.CourseEnrollments)
                    .HasForeignKey(sc => sc.StudentId);

                    e
                    .HasOne(sc => sc.Course)
                    .WithMany(c => c.StudentsEnrolled)
                    .HasForeignKey(sc => sc.CourseId);
                });
        }

        private void OnConfiguringHomework(ModelBuilder modelBuilder)
        {
            modelBuilder
               .Entity<Homework>(e =>
               {
                   e.HasKey(h => h.HomeworkId);

                   e
                   .Property(h => h.Content)
                   .IsRequired();

                   e
                   .HasOne(h => h.Student)
                   .WithMany(s => s.HomeworkSubmissions);

                   e
                   .HasOne(h => h.Course)
                   .WithMany(c => c.HomeworkSubmissions);
               });
        }

        private void OnConfiguringResource(ModelBuilder modelBuilder)
        {
            modelBuilder
               .Entity<Resource>(e =>
               {
                   e.HasKey(r => r.ResourceId);

                   e
                   .Property(r => r.Name)
                   .HasMaxLength(50)
                   .IsUnicode()
                   .IsRequired();

                   e
                   .Property(r => r.Url)
                   .IsUnicode();

                   e
                   .HasOne(r => r.Course)
                   .WithMany(c => c.Resources);
               });
        }

        private void OnConfiguringCourse(ModelBuilder modelBuilder)
        {
            modelBuilder
               .Entity<Course>(e =>
               {
                   e.HasKey(c => c.CourseId);

                   e
                   .Property(c => c.Name)
                   .HasMaxLength(80)
                   .IsUnicode()
                   .IsRequired();

                   e
                   .Property(c => c.Description)
                   .IsUnicode();

               });
        }

        private static void OnConfiguringStudent(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Student>(e =>
                {
                    e.HasKey(s => s.StudentId);

                    e
                    .Property(s => s.Name)
                    .HasMaxLength(100)
                    .IsUnicode()
                    .IsRequired();

                    e
                    .Property(s => s.PhoneNumber)
                    .HasColumnType("CHAR(10)");

                    e
                    .Property(s => s.RegisteredOn)
                    .HasDefaultValueSql("GETDATE()");
                });
        }
    }
}
