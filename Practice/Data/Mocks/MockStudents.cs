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

        public async Task<Student> GetEntity(int id) => await db.Students.FirstOrDefaultAsync(student => student.Id == id);

        public Task<bool> UpdateEntity(Student entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteEntity(Student student)
        {
            db.Students.Remove(student);
            await db.SaveChangesAsync();
            return true;
        }
    }
}
