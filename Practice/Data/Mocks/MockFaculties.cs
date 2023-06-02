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

        public Task<bool> AddEntity(Faculty entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteEntity(Faculty entity)
        {
            throw new NotImplementedException();
        }

        public async Task<Faculty> GetEntity(int id) => await db.Faculties.FirstOrDefaultAsync(faculty => faculty.Id == id);

        public Task<bool> UpdateEntity(Faculty entity)
        {
            throw new NotImplementedException();
        }
    }
}
