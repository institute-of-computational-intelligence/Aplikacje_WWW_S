using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;

public static class Extensions {

    // Create sample data
    public static async void SeedData (this IServiceCollection services) {
        var serviceProvider = services.BuildServiceProvider ();
        var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext> ();
        var userManager = serviceProvider.GetRequiredService<UserManager<User>> ();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>> ();

        // Roles
        var teacherRole = new Role () {
            Id = 3,
            Name = "Teacher",
            RoleValue = RoleValue.Teacher
        };
        await roleManager.CreateAsync (teacherRole);

        var studentRole = new Role () {
            Id = 1,
            Name = "Student",
            RoleValue = RoleValue.Student
        };
        await roleManager.CreateAsync (studentRole);

        var parentRole = new Role () {
            Id = 2,
            Name = "Parent",
            RoleValue = RoleValue.Parent
        };
        await roleManager.CreateAsync (parentRole);

        var adminRole = new Role () {
            Id = 4,
            Name = "Admin",
            RoleValue = RoleValue.Admin
        };
        await roleManager.CreateAsync (adminRole);

        // Groups
        var groupIo = new Group () {
            Id = 1,
            Name = "IO"
        };
        await dbContext.Groups.AddAsync (groupIo);
        var groupPai = new Group () {
            Id = 2,
            Name = "PAI"
        };
        await dbContext.Groups.AddAsync (groupPai);
        var groupAipErasmus = new Group () {
            Id = 3,
            Name = "AIP Erasmus"
        };
        await dbContext.Groups.AddAsync (groupAipErasmus);

        // All users, including teachers, students and parents
        var userPassword = "User1234"; // in lab env we use the same pass for all users :)
        var t1 = new Teacher () {
            Id = 1,
            FirstName = "Adam",
            LastName = "Bednarski",
            UserName = "t1@eg.eg",
            Email = "real_email@eg.eg",
            Title = "mgr inż.",
            RegistrationDate = new DateTime (2010, 1, 1),
        };
        await userManager.CreateAsync (t1, userPassword);
        await userManager.AddToRoleAsync (t1, teacherRole.Name);
        
        var t2 = new Teacher () {
            Id = 2,
            FirstName = "Jan",
            LastName = "Nowak",
            UserName = "t2@eg.eg",
            Email = "t2@eg.eg",
            Title = "mgr",
            RegistrationDate = new DateTime (2010, 11, 12),
        };
        await userManager.CreateAsync (t2, userPassword);
        await userManager.AddToRoleAsync (t2, teacherRole.Name);

        var t3 = new Teacher () {
            Id = 12,
            FirstName = "Stanisław",
            LastName = "Nowakowski",
            UserName = "t11@eg.eg",
            Email = "t11@eg.eg",
            Title = "mgr inż.",
            RegistrationDate = new DateTime (2010, 11, 12),
        };
        await userManager.CreateAsync (t3, userPassword);
        await userManager.AddToRoleAsync (t3, teacherRole.Name);

        var p1 = new Parent () {
            Id = 3,
            FirstName = "Zbigniew",
            LastName = "Kowalski",
            UserName = "p1@eg.eg",
            Email = "real_email@eg.eg",
            RegistrationDate = new DateTime (2014, 03, 20),
        };
        await userManager.CreateAsync (p1, userPassword);
        await userManager.AddToRoleAsync (p1, parentRole.Name);

        var p2 = new Parent () {
            Id = 4,
            FirstName = "Anna",
            LastName = "Nowakowska",
            UserName = "p2@eg.eg",
            Email = "p2@eg.eg",
            RegistrationDate = new DateTime (2014, 06, 21),
        };
        await userManager.CreateAsync (p2, userPassword);
        await userManager.AddToRoleAsync (p2, parentRole.Name);

        var s1 = new Student () {
            Id = 5,
            FirstName = "Tomasz",
            LastName = "Kowalski",
            UserName = "s1@eg.eg",
            Email = "s1@eg.eg",
            RegistrationDate = new DateTime (2016, 05, 11),
            GroupId = 1,
            ParentId = 3
        };
        await userManager.CreateAsync (s1, userPassword);
        await userManager.AddToRoleAsync (s1, studentRole.Name);

        var s2 = new Student () {
            Id = 6,
            FirstName = "Krzysztof",
            LastName = "Kowalski",
            UserName = "s2@eg.eg",
            Email = "s2@eg.eg",
            RegistrationDate = new DateTime (2015, 09, 18),
            GroupId = 1,
            ParentId = 3
        };
        await userManager.CreateAsync (s2, userPassword);
        await userManager.AddToRoleAsync (s2, studentRole.Name);

        var s3 = new Student () {
            Id = 7,
            FirstName = "Natalia",
            LastName = "Kowalska",
            UserName = "s3@eg.eg",
            Email = "s3@eg.eg",
            RegistrationDate = new DateTime (2017, 07, 16),
            GroupId = 2,
            ParentId = 3
        };
        await userManager.CreateAsync (s3, userPassword);
        await userManager.AddToRoleAsync (s3, studentRole.Name);

        var s4 = new Student () {
            Id = 8,
            FirstName = "Magdalena",
            LastName = "Wiśniewska",
            UserName = "s4@eg.eg",
            Email = "s4@eg.eg",
            RegistrationDate = new DateTime (2018, 05, 14),
            GroupId = 2,
            ParentId = 4
        };
        await userManager.CreateAsync (s4, userPassword);
        await userManager.AddToRoleAsync (s4, studentRole.Name);

        var s5 = new Student () {
            Id = 9,
            FirstName = "Jan",
            LastName = "Wiśniewski",
            UserName = "s5@eg.eg",
            Email = "s5@eg.eg",
            RegistrationDate = new DateTime (2019, 02, 19),
            GroupId = 3,
            ParentId = 4
        };
        await userManager.CreateAsync (s5, userPassword);
        await userManager.AddToRoleAsync (s5, studentRole.Name);

        var s6 = new Student () {
            Id = 10,
            FirstName = "Krystian",
            LastName = "Wiśniewski",
            UserName = "s6@eg.eg",
            Email = "s6@eg.eg",
            RegistrationDate = new DateTime (2019, 05, 1),
            GroupId = 3,
            ParentId = 4
        };
        await userManager.CreateAsync (s6, userPassword);
        await userManager.AddToRoleAsync (s6, studentRole.Name);

        var a1 = new User () {
            Id = 11,
            FirstName = "Jacek",
            LastName = "Kowalczyk",
            UserName = "a1@eg.eg",
            Email = "a1@eg.eg",
            RegistrationDate = new DateTime (2009, 1, 1)
        };
        await userManager.CreateAsync (a1, userPassword);
        await userManager.AddToRoleAsync (a1, adminRole.Name);

        // Subject
        var subject1 = new Subject () {
            Id = 1,
            Name = "Aplikacje WWW",
            Description = "Aplikacje webowe",
            TeacherId = 1
        };
        await dbContext.AddAsync (subject1);

        var subject2 = new Subject () {
            Id = 2,
            Name = "Programowanie obiektowe",
            Description = "Programowanie obiektowe jest przedmiotem realizującym przykłady programowanie obiektowego",
            TeacherId = 1
        };
        await dbContext.AddAsync (subject2);

        var subject3 = new Subject () {
            Id = 3,
            Name = "Advanced Internet Programming",
            Description = "Advanced Internet Programming is a course for ERASMUS+ students",
            TeacherId = 2
        };
        await dbContext.AddAsync (subject3);

        var subject4 = new Subject () {
            Id = 4,
            Name = "Administracja Intenetowymi Systemami Baz Danych",
            Description = "Administracja Intenetowymi Systemami Baz Danych jest kontynuacją przedmiotu Bazy danych na studiach stacjonarnych I-go stopnia spec. PAI",
            TeacherId = 2,
        };
        await dbContext.AddAsync (subject4);

        var subject5 = new Subject () {
            Id = 5,
            Name = "Programowanie interaktywnej grafiki dla stron WWW",
            TeacherId = 12
        };
        await dbContext.AddAsync (subject5);

        //SubjectGroups
        var subjectGroup1 = new SubjectGroup () {
            SubjectId = 1,
            GroupId = 1,
        };
        await dbContext.SubjectGroups.AddAsync (subjectGroup1);

        var subjectGroup2 = new SubjectGroup () {
            SubjectId = 1,
            GroupId = 2,
        };
        await dbContext.SubjectGroups.AddAsync (subjectGroup2);

        var subjectGroup3 = new SubjectGroup () {
            SubjectId = 2,
            GroupId = 1,
        };
        await dbContext.SubjectGroups.AddAsync (subjectGroup3);

        var subjectGroup4 = new SubjectGroup () {
            SubjectId = 2,
            GroupId = 2,
        };
        await dbContext.SubjectGroups.AddAsync (subjectGroup4);

        var subjectGroup5 = new SubjectGroup () {
            SubjectId = 2,
            GroupId = 3,
        };
        await dbContext.SubjectGroups.AddAsync (subjectGroup5);

        var subjectGroup6 = new SubjectGroup () {
            SubjectId = 3,
            GroupId = 3,
        };
        await dbContext.SubjectGroups.AddAsync (subjectGroup6);

        var subjectGroup7 = new SubjectGroup () {
            SubjectId = 4,
            GroupId = 2,
        };
        await dbContext.SubjectGroups.AddAsync (subjectGroup7);

        var subjectGroup8 = new SubjectGroup () {
            SubjectId = 4,
            GroupId = 3,
        };
        await dbContext.SubjectGroups.AddAsync (subjectGroup8);

        var grade1 = new Grade () {
            DateOfIssue = new DateTime (2019, 03, 21, 17, 46, 38),
            StudentId = 5,
            SubjectId = 1,
            GradeValue = GradeScale.DB
        };
        await dbContext.Grades.AddAsync (grade1);

        await dbContext.SaveChangesAsync ();
    }
}