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

        public async Task<bool> AddEntity(Team entity)
        {
            await db.Teams.AddAsync(entity);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteEntity(Team entity)
        {
            db.Teams.Remove(entity);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<Team> GetEntity(int id) => await db.Teams.FirstOrDefaultAsync(team => team.Id == id);

        public async Task<bool> UpdateEntity(Team entity)
        {
            db.Teams.Update(entity);
            await db.SaveChangesAsync();
            return true;
        }
    }
}
