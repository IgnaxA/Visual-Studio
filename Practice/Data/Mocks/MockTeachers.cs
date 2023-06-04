using Microsoft.EntityFrameworkCore;
using Practice.Data.Interface;
using Practice.ViewModels;

namespace Practice.Data.Mocks
{
    public class MockTeachers : ITeachers
    {
        private readonly InformationSystemToRecordProjectActivitiesDatabaseContext db;

        public MockTeachers(InformationSystemToRecordProjectActivitiesDatabaseContext db)
        {
            this.db = db;
        }

        public Task<bool> AddEntity(Teacher entity)
        {
            throw new NotImplementedException();
        }

        public async Task<Teacher> GetEntity(int id) => await db.Teachers.Include(teacher => teacher.Themes)
            .ThenInclude(theme => theme.Teams)
            .ThenInclude(team => team.Students)
            .ThenInclude(students => students.Faculty)
            .FirstOrDefaultAsync(teacher => teacher.Id == id);

        public Task<bool> UpdateEntity(Teacher entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteEntity(Teacher entity)
        {
            throw new NotImplementedException();
        }
    }
}
