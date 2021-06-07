using AutoMapper;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolRegister.Services.Services
{
    public abstract class BaseService
    {
        protected readonly ApplicationDbContext DbContext;
        protected readonly ILogger Logger;
        protected readonly IMapper Mapper;
        private ApplicationDbContext dbContext;
        private IMapper mapper;
        private ILogger logger;

        public BaseService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger)
        {
            DbContext = dbContext;
            Logger = logger;
            Mapper = mapper;
        }


    }
}

