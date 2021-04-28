using System.Collections.Generic;
using SchoolRegister.Model.DataModels;

namespace SchoolRegister.ViewModels.VM
{
    public class GradesReportVm
    {
        public string StudentFirstName { get; set; }
        public string StudentLastName { get; set; }
        public string ParentName { get; set; }
        public string GroupName { get; set; }
        public IDictionary<string, List<GradeScale>> StudentGradesPerSubject { get; set; }
        public double AverageGrade { get; set; }
        public IDictionary<string, double> AverageGradePerSubject { get; set; }
    }
}
