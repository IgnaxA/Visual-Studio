using Microsoft.EntityFrameworkCore;
using Practice.Data.Interface;
using Practice.ViewModels;

namespace Practice.Data.Mocks
{
    public class MockTeachers : ITeachers
    {
        private readonly InformationSystemToRecordProjectActivitiesDatabaseContext db;

        public MockTeachers(InformationSystemToRecordProjectActivitiesDatabaseContext db)
        {
            this.db = db;
        }

        public async Task<Teacher> GetTeacher(int id) => await db.Teachers.Include(teacher => teacher.Themes)
            .ThenInclude(theme => theme.Teams)
            .ThenInclude(team => team.Students)
            .FirstOrDefaultAsync(teacher => teacher.Id == id);

        public async Task<Student> GetStudent(int id) => await db.Students.FirstOrDefaultAsync(student => student.Id == id);

        public async Task<bool> DeleteStudent(Student student)
        {
            db.Students.Remove(student);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<Theme> GetTheme(int id) => await db.Themes.FirstOrDefaultAsync(theme => theme.Id == id);

        public async Task<bool> AddTheme(CreateThemeViewModel theme)
        {
            Theme _theme = new Theme()
            {
                ThemeFormulation = theme.ThemeFormulation,
                TeacherId = 1
            };

            await db.Themes.AddAsync(_theme);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteTheme(Theme theme)
        {
            db.Themes.Remove(theme);
            await db.SaveChangesAsync();
            return true;
        }
    }
}
