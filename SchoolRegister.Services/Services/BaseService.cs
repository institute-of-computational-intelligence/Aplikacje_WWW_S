using System;
using AutoMapper;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using Microsoft.AspNetCore.Identity;


namespace SchoolRegister.Services.Services
{
    public abstract class BaseService
    {
        protected readonly ApplicationDbContext DbContext;
        protected readonly ILogger Logger;
        protected readonly IMapper Mapper;  
        
        public BaseService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger)
        {
            DbContext = dbContext;
            Mapper = mapper;
            Logger = logger;
         
        }

    }
}