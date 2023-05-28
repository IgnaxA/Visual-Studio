using Practice.ViewModels;

namespace Practice.Data.Interface
{
    public interface ITeachers
    {
        Task<Teacher> GetTeacher(int id);

        Task<Student> GetStudent(int id);

        Task<bool> DeleteStudent(Student student);

        Task<Theme> GetTheme(int id);

        Task<bool> AddTheme(CreateThemeViewModel theme);

        Task<bool> DeleteTheme(Theme theme);
    }
}
