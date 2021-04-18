using System;
using AutoMapper;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using Microsoft.AspNetCore.Identity;
using SchoolRegister.Model.DataModels;

namespace SchoolRegister.Services.Services
{
    public abstract class BaseService
    {
        protected readonly ApplicationDbContext DbContext;
        protected readonly ILogger Logger;
        protected readonly IMapper Mapper;
        protected readonly UserManager<User> UserManager;

        public BaseService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger)
        {
            DbContext = dbContext;
            Logger = logger;
            Mapper = mapper;
        }

        public BaseService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger, UserManager<User> userManager)
        {
            DbContext = dbContext;
            Logger = logger;
            Mapper = mapper;
            UserManager = userManager;
        }
    }
}
