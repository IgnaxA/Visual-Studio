namespace Practice.Data.Interface
{
    public interface IFaculties : IBaseEntity<Faculty>
    {
        Task<IEnumerable<Faculty>> GetEntities();

    }
}
