using Microsoft.EntityFrameworkCore;
using Practice.Data.Interface;

namespace Practice.Data.Mocks
{
    public class MockConsultations : IConsultations
    {
        private readonly InformationSystemToRecordProjectActivitiesDatabaseContext db;

        public MockConsultations(InformationSystemToRecordProjectActivitiesDatabaseContext db)
        {
            this.db = db;
        }

        public async Task<bool> AddEntity(Consultation entity)
        {
            await db.Consultations.AddAsync(entity);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteEntity(Consultation entity)
        {
            db.Consultations.Remove(entity);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<Consultation> GetEntity(int id) => await db.Consultations.FirstOrDefaultAsync(consultation => consultation.Id == id);

        public async Task<bool> UpdateEntity(Consultation entity)
        {
            db.Consultations.Update(entity);
            await db.SaveChangesAsync();
            return true;
        }
    }
}
