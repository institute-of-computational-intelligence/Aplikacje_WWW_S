using SchoolRegister.DAL.EntityFramework;

namespace SchoolRegister.Tests
{
    public abstract class BaseUnitTests
    {
        protected readonly ApplicationDbContext DbContext;

        public BaseUnitTests(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }
    }
}