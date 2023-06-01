using Microsoft.EntityFrameworkCore;
using Practice.Data.Interface;

namespace Practice.Data.Mocks
{
    public class MockCourses : ICourses
    {
        private readonly InformationSystemToRecordProjectActivitiesDatabaseContext db;

        public MockCourses(InformationSystemToRecordProjectActivitiesDatabaseContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<Course>> GetEntities() => await db.Courses.ToListAsync();

        public Task<bool> AddEntity(Course entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteEntity(Course entity)
        {
            throw new NotImplementedException();
        }

        public Task<Course> GetEntity(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateEntity(Course entity)
        {
            throw new NotImplementedException();
        }
    }
}
