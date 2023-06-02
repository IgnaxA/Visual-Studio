using Microsoft.EntityFrameworkCore;
using Practice.Data.Interface;

namespace Practice.Data.Mocks
{
    public class MockStudents : IStudents
    {
        private readonly InformationSystemToRecordProjectActivitiesDatabaseContext db;

        public MockStudents(InformationSystemToRecordProjectActivitiesDatabaseContext db)
        {
            this.db = db;
        }

        public async Task<bool> AddEntity(Student student)
        {
            await db.Students.AddAsync(student);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<Student> GetEntity(int id) => await db.Students.Include(student => student.Team).ThenInclude(team => team.Theme).FirstOrDefaultAsync(student => student.Id == id);

        public async Task<bool> UpdateEntity(Student entity)
        {
            db.Students.Update(entity);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteEntity(Student student)
        {
            db.Students.Remove(student);
            await db.SaveChangesAsync();
            return true;
        }
    }
}
