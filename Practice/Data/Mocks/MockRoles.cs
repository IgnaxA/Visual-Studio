using Microsoft.EntityFrameworkCore;
using Practice.Data.Interface;

namespace Practice.Data.Mocks
{
    public class MockRoles : IRoles
    {
        private readonly InformationSystemToRecordProjectActivitiesDatabaseContext db;

        public MockRoles(InformationSystemToRecordProjectActivitiesDatabaseContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<Role>> GetEntities() => await db.Roles.ToListAsync();

        public Task<bool> AddEntity(Role entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteEntity(Role entity)
        {
            throw new NotImplementedException();
        }

        public async Task<Role> GetEntity(int id) => await db.Roles.FirstOrDefaultAsync(role => role.Id == id);

        public Task<bool> UpdateEntity(Role entity)
        {
            throw new NotImplementedException();
        }
    }
}
