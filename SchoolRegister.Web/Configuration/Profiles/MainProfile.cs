using AutoMapper;
using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;
using System.Linq;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace SchoolRegister.Web.Configuration.Profiles
{
    public class MainProfile : Profile
    {
        public MainProfile()
        {
            CreateMap<Subject, SubjectVm>()
                .ForMember(dest => dest.TeacherName, x => x.MapFrom(src => $"{src.Teacher.FirstName} {src.Teacher.LastName}"))
                .ForMember(dest => dest.Groups, x => x.MapFrom(src => src.SubjectGroups.Select(y => y.Group)));

            CreateMap<AddOrUpdateSubjectVm, Subject>()
                .ForMember(dest => dest.Id, x => x.MapFrom(src => src.Id))
                .ForMember(dest => dest.TeacherId, x => x.MapFrom(src => src.TeacherId));

            CreateMap<Group, GroupVm>()
                .ForMember(dest => dest.Id, x => x.MapFrom(src => src.Id));
            
            CreateMap<Grade,GradeVm>();
            
            CreateMap<GroupVm, Group>()
                .ForMember(dest => dest.Id, x => x.MapFrom(src => src.Id));  
                
            CreateMap<SubjectVm,AddOrUpdateSubjectVm>()
                .ForMember(dest => dest.Id, x => x.MapFrom(src => src.Id))
                .ForMember(dest => dest.TeacherId, x => x.MapFrom(src => src.TeacherId));;

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
            
            CreateMap<Teacher, TeacherVm> ();

            CreateMap<Student, StudentVm> ()
                .ForMember (dest => dest.GroupName, x => x.MapFrom (src => src.Group.Name))
                .ForMember (dest => dest.ParentName, x => x.MapFrom (src => $"{src.Parent.FirstName} {src.Parent.LastName}"));

            CreateMap<GroupVm, SelectListItem>()
                .ForMember (x => x.Text, y => y.MapFrom (z => z.Name))
                .ForMember (x => x.Value, y => y.MapFrom (z => z.Id));

            CreateMap<Student, GradesRaportVm> ()
                .ForMember (dest => dest.StudentLastName, y => y.MapFrom (src => src.LastName))
                .ForMember (dest => dest.StudentFirstName, y => y.MapFrom (src => src.FirstName))
                .ForMember (dest => dest.GroupName, y => y.MapFrom (src => src.Group.Name))
                .ForMember (dest => dest.ParentName, y => y.MapFrom (src => $"{src.Parent.FirstName} {src.Parent.LastName}"))
                .ForMember (dest => dest.StudentGradesPerSubject, y => y.MapFrom (src => src.Grades
                    .GroupBy (g => g.Subject.Name)
                    .Select (g => new { SubjectName = g.Key, Grades = g.Select (gl => gl.GradeValue).ToList () })
                    .ToDictionary (x => x.SubjectName, x => x.Grades)));
        
        }
    }
}