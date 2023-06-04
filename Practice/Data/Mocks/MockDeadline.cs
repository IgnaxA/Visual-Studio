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

        public Task<bool> AddEntity(Deadline entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteEntity(Deadline entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Deadline>> GetEntities() => await db.Deadlines.ToListAsync();

        public Task<Deadline> GetEntity(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateEntity(Deadline entity)
        {
            throw new NotImplementedException();
        }
    }
}
