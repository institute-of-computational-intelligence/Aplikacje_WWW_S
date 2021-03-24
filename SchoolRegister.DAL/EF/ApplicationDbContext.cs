using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolRegister.Model.DataModels;


namespace SchoolRegister.DAL.EF
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, int>
    {
        //Table properties
        public virtual DbSet<Grade> Grades { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<SubjectGroup> SubjectGroups { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            //configuration commands
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //fluent API commands
            modelBuilder.Entity<User>()
                .ToTable("AspNetUsers")
                .HasDiscriminator<int>("UserType")
                .HasValue<User>((int)RoleValue.User)
                .HasValue<Student>((int)RoleValue.Student)
                .HasValue<Parent>((int)RoleValue.Parent)
                .HasValue<Teacher>((int)RoleValue.Teacher);

            modelBuilder.Entity<SubjectGroup>()
                .HasKey(sg => new {sg.GroupId, sg.SubjectId});
            modelBuilder.Entity<SubjectGroup>()
                .HasOne(g => g.Group)
                .WithMany(sg => sg.SubjectGroups)
                .HasForeignKey(g => g.GroupId);
            modelBuilder.Entity<SubjectGroup>()
                .HasOne(s => s.Subject)
                .WithMany(sg => sg.SubjectGroups)
                .HasForeignKey(s => s.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Grade>()
                .HasKey(g => new {g.DateOfIssue, g.SubjectId, g.StudentId});
            modelBuilder.Entity<Grade>()
                .HasOne(s => s.Subject)
                .WithMany(g => g.Grades)
                .HasForeignKey(s => s.SubjectId);

            modelBuilder.Entity<Group>()
                .HasKey(g => new {g.Id});
            modelBuilder.Entity<Group>()
                .HasMany(g => g.Students)
                .WithOne(s => s.Group)
                .HasForeignKey(x => x.GroupId)
                .IsRequired();

            modelBuilder.Entity<Subject>()
                .HasKey(s => new {s.Id});
            modelBuilder.Entity<Subject>()
                .HasMany(sg => sg.SubjectGroups)
                .WithOne(s => s.Subject)
                .HasForeignKey(s => s.SubjectId);
            modelBuilder.Entity<Subject>()
                .HasMany(g => g.Grades)
                .WithOne(s => s.Subject)
                .HasForeignKey(s => s.SubjectId);
        }
    }
}