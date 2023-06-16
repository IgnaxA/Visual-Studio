using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using Practice.ViewModels;

namespace Practice.Data.Interface
{
    public interface ITeachers : IBaseEntity<Teacher>
    {
        Task<Teacher> GetEntityDeadline(int id); 
    }
}
