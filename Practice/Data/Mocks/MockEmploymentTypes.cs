using Microsoft.EntityFrameworkCore;
using Practice.Data.Interface;

namespace Practice.Data.Mocks
{
    public class MockEmploymentTypes : IEmploymentTypes
    {
        private readonly InformationSystemToRecordProjectActivitiesDatabaseContext db;

        public MockEmploymentTypes(InformationSystemToRecordProjectActivitiesDatabaseContext db)
        {
            this.db = db;
        }

        public async Task<bool> AddEntity(EmploymentType entity)
        {
            await db.EmploymentTypes.AddAsync(entity);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteEntity(EmploymentType entity)
        {
            db.EmploymentTypes.Remove(entity);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<EmploymentType> GetEntity(int id) => await db.EmploymentTypes.FirstOrDefaultAsync(employmentType => employmentType.Id == id);

        public async Task<bool> UpdateEntity(EmploymentType entity)
        {
            db.EmploymentTypes.Update(entity);
            await db.SaveChangesAsync();
            return true;
        }
    }
}
