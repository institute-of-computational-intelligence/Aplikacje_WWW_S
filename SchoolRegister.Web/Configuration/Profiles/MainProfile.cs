using AutoMapper;
using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;
using System.Linq;
using System;

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

            CreateMap<Teacher, TeacherVm>();
            CreateMap<Student, StudentVm>()
                .ForMember(dest => dest.ParentName, x => x.MapFrom(src => $"{src.Parent.FirstName} {src.Parent.LastName}"));

            CreateMap<Grade, DisplayGradeVm>()
                .ForMember(dest => dest.StudentName, x => x.MapFrom(src => $"{src.Student.FirstName} {src.Student.LastName}"))
                .ForMember(dest => dest.TeacherName, x => x.MapFrom(src => $"{src.Subject.Teacher.FirstName} {src.Subject.Teacher.LastName}"))
                .ForMember(dest => dest.SubjectName, x => x.MapFrom(src => src.Subject.Name));



            CreateMap<Group, GroupVm>()
                .ForMember(dest => dest.Id, x => x.MapFrom(src => src.Id));
            //.ForMember(dest => dest.Students, x => x.MapFrom(src => src.Students))

            CreateMap<GroupVm, Group>()
                .ForMember(dest => dest.Id, x => x.MapFrom(src => src.Id.HasValue ? src.Id : 0));

            CreateMap<AddGradeToStudentVm, Grade>()
                .ForMember(dest => dest.DateOfIssue, x => x.MapFrom(srcc => DateTime.Now));

            CreateMap<SubjectVm, AddOrUpdateSubjectVm>()
                .ForMember(dest => dest.Id, x => x.MapFrom(src => src.Id))
                .ForMember(dest => dest.TeacherId, x => x.MapFrom(src => src.TeacherId));

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



        }

    }


}