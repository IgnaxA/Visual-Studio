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

        public async Task<bool> AddEntity(Teacher entity)
        {
            await db.Teachers.AddAsync(entity);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<Teacher> GetEntity(int id) => await db.Teachers.Include(teacher => teacher.Themes)
            .ThenInclude(theme => theme.Teams)
            .ThenInclude(team => team.Students)
            .ThenInclude(students => students.Faculty)
            .FirstOrDefaultAsync(teacher => teacher.Id == id);


        public async Task<Teacher> GetEntityDeadline(int id) => await db.Teachers.Include(teacher => teacher.Themes)
            .ThenInclude(theme => theme.Teams)
            .ThenInclude(team => team.Deadlines)
            .ThenInclude(deadline => deadline.Consultations)
            .FirstOrDefaultAsync(teacher => teacher.Id == id);



        public async Task<bool> UpdateEntity(Teacher entity)
        {
            db.Teachers.Update(entity);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteEntity(Teacher entity)
        {
            db.Teachers.Remove(entity);
            await db.SaveChangesAsync();
            return true;
        }
    }
}
