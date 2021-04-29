﻿using AutoMapper;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using System;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SchoolRegister.Services.Services
{
    public class GradeService : BaseService, IGradeService
    {     
        private UserManager<User> userManager;
   

        public GradeService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger, UserManager<User> userManager) : base(dbContext, mapper, logger) { this.userManager = userManager; }

        public async Task<IEnumerable<Grade>> ShowGrades(GradesVm gradesVm)
        {
            try
            {
                var user = await DbContext.Users.FirstOrDefaultAsync(u => u.Id == gradesVm.CallerId);

                if (user == null)
                {
                    throw new ArgumentNullException($"Could not find user with id: {gradesVm.SubjectId}");
                }

                if (await userManager.IsInRoleAsync(user, "Parent") || await userManager.IsInRoleAsync(user, "Student"))
                {
                    var grades = DbContext.Grades.Where(g => g.StudentId == gradesVm.StudentId).ToList();

                    return grades;
                }
                else
                {
                    throw new ArgumentException("Current user does not have required permissions to performe this action.");
                }

            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

      

   
    }
}