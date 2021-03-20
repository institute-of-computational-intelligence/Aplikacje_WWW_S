using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SchoolRegister.Model.DataModels
{
    public class SubjectGroup
    {
        
        [Key]
        public int GroupId { get; set; }
        public virtual Group Group { get; set; }
        
        [Key]
        public int SubjectId { get; set; }
        public virtual Subject Subject { get; set; }
    }
}
