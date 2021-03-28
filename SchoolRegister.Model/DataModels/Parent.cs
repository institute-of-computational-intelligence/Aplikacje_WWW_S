using System.Collections.Generic;


namespace SchoolRegister.Model.DataModels{
    public class Parent
    {
        public virtual IList<Student> Students{get;set;}
    }
}