using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
namespace SchoolRegister.Api.Controllers
{

    [Authorize]
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public abstract class BaseApiController : Controller
    {
        protected readonly ILogger Logger;
        protected readonly IMapper Mapper;
        protected BaseApiController(ILogger logger, IMapper mapper)
        {
            Logger = logger;
            Mapper = mapper;
        }
    }
}