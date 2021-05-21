
namespace SchoolRegister.Model.DataModels
{
    public class SubjectGroup
    {
        public virtual Subject Subject { get; set; }
        public int SubjectId { get; set; }
        public virtual Group Group { get; set; }
        public int GroupId { get; set; }

    }
}
