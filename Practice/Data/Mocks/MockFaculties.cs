using Microsoft.EntityFrameworkCore;
using Practice.Data.Interface;

namespace Practice.Data.Mocks
{
    public class MockFaculties : IFaculties
    {
        private readonly InformationSystemToRecordProjectActivitiesDatabaseContext db;

        public MockFaculties(InformationSystemToRecordProjectActivitiesDatabaseContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<Faculty>> GetEntities() => await db.Faculties.ToListAsync(); 

        public async Task<bool> AddEntity(Faculty entity)
        {
            await db.Faculties.AddAsync(entity);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteEntity(Faculty entity)
        {
            db.Faculties.Remove(entity);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<Faculty> GetEntity(int id) => await db.Faculties.FirstOrDefaultAsync(faculty => faculty.Id == id);

        public async Task<bool> UpdateEntity(Faculty entity)
        {
            db.Faculties.Update(entity);
            await db.SaveChangesAsync();
            return true;
        }
    }
}
