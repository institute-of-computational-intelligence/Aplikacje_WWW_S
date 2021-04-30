using AutoMapper;
using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;
using System.Linq;

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
            //.ForMember(dest => dest.Students, x => x.MapFrom(src => src.Students))

            CreateMap<GroupVm, Group>()
                .ForMember(dest => dest.Id, x => x.MapFrom(src => src.Id.HasValue ? src.Id : 0));

            CreateMap<SubjectVm, AddOrUpdateSubjectVm>()
                .ForMember(dest => dest.Id, x => x.MapFrom(src => src.Id))
                .ForMember(dest => dest.TeacherId, x => x.MapFrom(src => src.TeacherId));

        }

    }


}