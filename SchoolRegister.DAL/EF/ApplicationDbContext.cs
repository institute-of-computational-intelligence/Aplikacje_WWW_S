using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolRegister.Model.DataModels;
namespace SchoolRegister.DAL.EF
{
 public class ApplicationDbContext : IdentityDbContext<User,Role,int>
 {
 // Table properties e.g
 // public virtual DbSet<Entity> TableName { get; set; }
 public virtual DbSet<Grade> Grades { get; set; }
   public virtual DbSet<Group> Groups { get; set; }


     public virtual DbSet<Subject> Subjects { get; set; }
    public virtual DbSet<SubjectGroup> SubjectGroups { get; set; }
 // more properties need to added….
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
 protected override void OnModelCreating(ModelBuilder modelBuilder)
 {
 base.OnModelCreating(modelBuilder);
 // Fluent API commands
 modelBuilder.Entity<User>()
 .ToTable("AspNetUsers")
 .HasDiscriminator<int>("UserType")
 .HasValue<User>((int)RoleValue.User)
 .HasValue<Student>((int)RoleValue.Student)
 .HasValue<Parent>((int)RoleValue.Parent)
 .HasValue<Teacher>((int)RoleValue.Teacher);
 }
 }
}
