using Microsoft.EntityFrameworkCore;
using Practice.Data.Interface;

namespace Practice.Data.Mocks
{
    public class MockTeams : ITeams
    {
        private readonly InformationSystemToRecordProjectActivitiesDatabaseContext db;

        public MockTeams(InformationSystemToRecordProjectActivitiesDatabaseContext db)
        {
            this.db = db;
        }

        public Task<bool> AddEntity(Team entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteEntity(Team entity)
        {
            throw new NotImplementedException();
        }

        public async Task<Team> GetEntity(int id) => await db.Teams.FirstOrDefaultAsync(team => team.Id == id);

        public Task<bool> UpdateEntity(Team entity)
        {
            throw new NotImplementedException();
        }
    }
}
