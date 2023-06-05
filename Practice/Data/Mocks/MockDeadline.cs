using Microsoft.EntityFrameworkCore;
using Practice.Data.Interface;

namespace Practice.Data.Mocks
{
    public class MockDeadline : IDeadlines
    {
        private readonly InformationSystemToRecordProjectActivitiesDatabaseContext db;

        public MockDeadline(InformationSystemToRecordProjectActivitiesDatabaseContext db)
        {
            this.db = db;
        }

        public async Task<bool> AddEntity(Deadline entity)
        {
            await db.Deadlines.AddAsync(entity);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteEntity(Deadline entity)
        {
            db.Deadlines.Remove(entity);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Deadline>> GetEntities() => await db.Deadlines.ToListAsync();

        public async Task<Deadline> GetEntity(int id) => await db.Deadlines.FirstOrDefaultAsync(deadline => deadline.Id == id);

        public async Task<bool> UpdateEntity(Deadline entity)
        {
            db.Deadlines.Update(entity);
            await db.SaveChangesAsync();
            return true;
        }
    }
}
