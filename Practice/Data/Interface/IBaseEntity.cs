using Practice.ViewModels;

namespace Practice.Data.Interface
{
    public interface IBaseEntity<T>
    {
        Task<bool> AddEntity(T entity);

        Task<T> GetEntity(int id);

        Task<bool> DeleteEntity(T entity);

        Task<bool> UpdateEntity(T entity);
    }
}
