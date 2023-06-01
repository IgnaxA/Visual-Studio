namespace Practice.Data.Interface
{
    public interface ICourses : IBaseEntity<Course>
    {
        Task<IEnumerable<Course>> GetEntities();
    }
}
