﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace SchoolRegister.BLL.DataModels
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<Student> Students { get; set; }
        public IList<SubjectGroup> SubjectGroups { get; set; }
    }
}
