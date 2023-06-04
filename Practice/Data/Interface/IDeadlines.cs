namespace Practice.Data.Interface
{
    public interface IDeadlines : IBaseEntity<Deadline>
    {
        Task<IEnumerable<Deadline>> GetEntities();
    }
}
