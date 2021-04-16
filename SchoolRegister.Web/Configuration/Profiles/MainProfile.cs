using AutoMapper;
using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;
using System.Linq;
namespace SchoolRegister.Web.Configuration.Profiles{
 public class MainProfile : Profile
 {
 public MainProfile()
 {
 //AutoMapper maps
 CreateMap<Subject, SubjectVm>() // map from Subject(src) to SubjectVm(dst)
 // custom mapping: FirstName and LastName concat string to TeacherName
 .ForMember(dest => dest.TeacherName, x => x.MapFrom(src => $"{src.Teacher.FirstName} {src.Teacher.LastName}"))
 // custom mapping: IList<Group> to IList<GroupVm>
 .ForMember(dest => dest.Groups, x => x.MapFrom(src => src.SubjectGroups.Select(y => y.Group)));

 CreateMap<AddOrUpdateSubjectVm, Subject>();
 CreateMap<Group, GroupVm>();
 CreateMap<SubjectVm, AddOrUpdateSubjectVm>();
 //other maps...
 }
 }
}
