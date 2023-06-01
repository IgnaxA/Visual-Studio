namespace Practice.Data.Interface
{
    public interface IRoles : IBaseEntity<Role>
    {
        Task<IEnumerable<Role>> GetEntities();
    }
}
