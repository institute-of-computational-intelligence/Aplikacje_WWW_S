using AutoMapper;
using SchoolRegister.DAL.EF;

namespace SchoolRegister.Tests.UnitTests {
    public abstract class BaseUnitTests {
        protected readonly ApplicationDbContext DbContext;
        public BaseUnitTests (ApplicationDbContext dbContext) {
        DbContext = dbContext;;
        }
    }
}