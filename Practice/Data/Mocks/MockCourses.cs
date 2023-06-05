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

        public async Task<bool> AddEntity(Course entity)
        {
            await db.Courses.AddAsync(entity);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteEntity(Course entity)
        {
            db.Courses.Remove(entity);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<Course> GetEntity(int id) => await db.Courses.SingleOrDefaultAsync(course => course.Id == id);

        public async Task<bool> UpdateEntity(Course entity)
        {
            db.Courses.Update(entity);
            await db.SaveChangesAsync();
            return true;
        }
    }
}
