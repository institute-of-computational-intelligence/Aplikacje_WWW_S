
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore; 
using Microsoft.EntityFrameworkCore;
using SchoolRegister.Model.DataModels;

namespace SchoolRegister.DAL.EF {
    public class ApplicationDbContext : IdentityDbContext<User,Role,int> 
    {
        // Table properties e.g
        // public virtual DbSet<Entity> TableName { get; set; } 
        public virtual DbSet<Grade> Grades { get; set; }
        public virtual DbSet<Group> Group { get; set; }
        public virtual DbSet<Subject> Subject { get; set; }
        public virtual DbSet<SubjectGroup> SubjectGroup { get; set; }
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { 

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        {
            base.OnConfiguring(optionsBuilder);
            //configuration commands
            optionsBuilder.UseLazyLoadingProxies(); //enable lazy loading proxies
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder); 
            // Fluent API commands 
            modelBuilder.Entity<User>()
                .ToTable("AspNetUsers") 
                .HasDiscriminator<int>("UserType") 
                .HasValue<User>((int)RoleValue.User) 
                .HasValue<Student>((int)RoleValue.Student) 
                .HasValue<Parent>((int)RoleValue.Parent) 
                .HasValue<Teacher>((int)RoleValue.Teacher);

            modelBuilder.Entity<Group>()
                .HasKey(g => new {g.Id});

            modelBuilder.Entity<Group>()
                .HasMany(g => g.Students)
                .WithOne(s => s.Group)
                .HasForeignKey(x => x.GroupId)
                .IsRequired();

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

            modelBuilder.Entity<Subject>() 
                .HasKey(s => new {s.Id});

            modelBuilder.Entity<Subject>()
                .HasOne(t => t.Teacher)
                .WithMany(s => s.Subjects)
                .HasForeignKey(t => t.TeacherId);

            modelBuilder.Entity<Grade>()
                .HasKey(g => new {g.DateOfIssue, g.SubjectId, g.StudentId});
            
            modelBuilder.Entity<Grade>()
                .HasOne(g => g.Student)
                .WithMany(s => s.Grades)
                .HasForeignKey(g => g.StudentId);

            modelBuilder.Entity<Grade>()
                .HasOne(g => g.Subject)
                .WithMany(s => s.Grades)
                .HasForeignKey(g => g.SubjectId);
        }
    }
}
        