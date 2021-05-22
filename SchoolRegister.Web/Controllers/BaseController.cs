using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace SchoolRegister.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly IStringLocalizer Localizer;
        protected readonly ILogger Logger;
        protected readonly IMapper Mapper;
        public BaseController(ILogger logger, IMapper mapper, IStringLocalizer localizer)
        {
            Localizer = localizer;
            Logger = logger;
            Mapper = mapper;
        }
    }
}