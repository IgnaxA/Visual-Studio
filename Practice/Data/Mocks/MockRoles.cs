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

        public async Task<bool> AddEntity(Role entity)
        {
            await db.Roles.AddAsync(entity);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteEntity(Role entity)
        {
            db.Roles.Remove(entity);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<Role> GetEntity(int id) => await db.Roles.FirstOrDefaultAsync(role => role.Id == id);

        public async Task<bool> UpdateEntity(Role entity)
        {
            db.Roles.Update(entity);
            await db.SaveChangesAsync();
            return true;
        }
    }
}
