using Microsoft.EntityFrameworkCore;
using Practice.Data.Interface;

namespace Practice.Data.Mocks
{
    public class MockDegrees : IDegrees
    {
        private readonly InformationSystemToRecordProjectActivitiesDatabaseContext db;

        public MockDegrees(InformationSystemToRecordProjectActivitiesDatabaseContext db)
        {
            this.db = db;
        }

        public async Task<bool> AddEntity(Degree entity)
        {
            await db.Degrees.AddAsync(entity);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteEntity(Degree entity)
        {
            db.Degrees.Remove(entity);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<Degree> GetEntity(int id) => await db.Degrees.FirstOrDefaultAsync(degree => degree.Id == id);

        public async Task<bool> UpdateEntity(Degree entity)
        {
            db.Degrees.Update(entity);
            await db.SaveChangesAsync();
            return true;
        }
    }
}
