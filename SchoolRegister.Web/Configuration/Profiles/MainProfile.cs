using AutoMapper;
using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;
using System.Linq;

namespace SchoolRegister.Web.Configuration.Profiles{
    public class MainProfile : Profile
    {
        public MainProfile()
        {
            CreateMap<Subject, SubjectVm>()
                .ForMember(dest => dest.TeacherName, x => x.MapFrom(src => $"{src.Teacher.FirstName} {src.Teacher.LastName}"))
                .ForMember(dest => dest.Groups, x => x.MapFrom(src => src.SubjectGroups.Select(y => y.Group)));

            CreateMap<AddOrUpdateSubjectVm, Subject>();
            CreateMap<Group, GroupVm>();
            CreateMap<SubjectVm, AddOrUpdateSubjectVm>();
        }
    }
}