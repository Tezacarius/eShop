namespace Catalog.Host.Repositories.Interfaces
{
    public interface ICatalogTypeRepository
    {
        Task<int?> Add(string type);
        Task<int?> Remove(int id);
        Task<int?> Update(int id, string type);
    }
}
