using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.Extensions.Logging;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using SchoolRegister.DAL.EF;

namespace SchoolRegister.Services.Services
{
  public class EmailSenderService : BaseService, IEmailSenderService
  {
    public EmailSenderService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger) : base(dbContext, mapper,
        logger)
    {
    }





  }
}