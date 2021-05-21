using System;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Web.Configuration.Profiles {
    public class MainProfile : Profile {
        public MainProfile () {
            //AutoMapper maps
            CreateMap<Subject, SubjectVm> () // map from Subject(src) to SubjectVm(dst)
                // custom mapping: FirstName and LastName concat string to TeacherName
                .ForMember (dest => dest.TeacherName, x => x.MapFrom (src => $"{src.Teacher.FirstName} {src.Teacher.LastName}"))
                // custom mapping: IList<Group> to IList<GroupVm>
                .ForMember (dest => dest.Groups, x => x.MapFrom (src => src.SubjectGroups.Select (y => y.Group)));

            CreateMap<AddOrUpdateSubjectVm, Subject> ();
            CreateMap<Group, GroupVm> ()
                .ForMember (dest => dest.Students, x => x.MapFrom (src => src.Students))
                .ForMember (dest => dest.Subjects, x => x.MapFrom (src => src.SubjectGroups.Select (s => s.Subject)));
            CreateMap<SubjectVm, AddOrUpdateSubjectVm> ();
            CreateMap<Teacher, TeacherVm> ();
            CreateMap<AddOrUpdateGroupVm, Group> ();
            CreateMap<Student, StudentVm> ()
                .ForMember (dest => dest.GroupName, x => x.MapFrom (src => src.Group.Name))
                .ForMember (dest => dest.ParentName,
                    x => x.MapFrom (src => $"{src.Parent.FirstName} {src.Parent.LastName}"));
            CreateMap<AddGradeToStudentVm, Grade> ()
                .ForMember (dest => dest.DateOfIssue, y => y.MapFrom (src => DateTime.Now));
            CreateMap<Grade,GradeVm>();
            CreateMap<GroupVm, AddOrUpdateGroupVm> ();
            CreateMap<Student, GradesReportVm> ()
                .ForMember (dest => dest.StudentLastName, y => y.MapFrom (src => src.LastName))
                .ForMember (dest => dest.StudentFirstName, y => y.MapFrom (src => src.FirstName))
                .ForMember (dest => dest.GroupName, y => y.MapFrom (src => src.Group.Name))
                .ForMember (dest => dest.ParentName, y => y.MapFrom (src => $"{src.Parent.FirstName} {src.Parent.LastName}"))
                .ForMember (dest => dest.StudentGradesPerSubject, y => y.MapFrom (src => src.Grades
                    .GroupBy (g => g.Subject.Name)
                    .Select (g => new { SubjectName = g.Key, Grades = g.Select (gl => gl.GradeValue).ToList () })
                    .ToDictionary (x => x.SubjectName, x => x.Grades)));
            CreateMap<GroupVm, SelectListItem> ()
                .ForMember (x => x.Text, y => y.MapFrom (z => z.Name))
                .ForMember (x => x.Value, y => y.MapFrom (z => z.Id));
            CreateMap<StudentVm, SelectListItem> ()
                .ForMember (x => x.Text, y => y.MapFrom (z => z.UserName))
                .ForMember (x => x.Value, y => y.MapFrom (z => z.Id));
            CreateMap<RegisterNewUserVm, User>()
                .ForMember(dest => dest.UserName, y => y.MapFrom(src => src.Email))
                .ForMember(dest => dest.RegistrationDate, y => y.MapFrom(src => DateTime.Now));
            CreateMap<RegisterNewUserVm, Parent>()
                .ForMember(dest => dest.UserName, y => y.MapFrom(src => src.Email))
                .ForMember(dest => dest.RegistrationDate, y => y.MapFrom(src => DateTime.Now));
            CreateMap<RegisterNewUserVm, Student>()
                .ForMember(dest => dest.UserName, y => y.MapFrom(src => src.Email))
                .ForMember(dest => dest.RegistrationDate, y => y.MapFrom(src => DateTime.Now));
            CreateMap<RegisterNewUserVm, Teacher>()
                .ForMember(dest => dest.UserName, y => y.MapFrom(src => src.Email))
                .ForMember(dest => dest.RegistrationDate, y => y.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Title, y => y.MapFrom(src => src.TeacherTitles));
            //....... other maps.........
        }
    }
}