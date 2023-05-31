using Microsoft.EntityFrameworkCore;
using Practice.Data.Interface;
using Practice.ViewModels;

namespace Practice.Data.Mocks
{
    public class MockThemes : IThemes
    {
        private readonly InformationSystemToRecordProjectActivitiesDatabaseContext db;

        public MockThemes(InformationSystemToRecordProjectActivitiesDatabaseContext db)
        {
            this.db = db;
        }

        public async Task<bool> AddEntity(Theme theme)
        {
            await db.Themes.AddAsync(theme);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<Theme> GetEntity(int id) => await db.Themes.Include(theme => theme.Teams).FirstOrDefaultAsync(theme => theme.Id == id);

        public async Task<bool> UpdateEntity(Theme entity)
        {
            db.Themes.Update(entity);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteEntity(Theme theme)
        {
            db.Themes.Remove(theme);
            await db.SaveChangesAsync();
            return true;
        }
    }
}
