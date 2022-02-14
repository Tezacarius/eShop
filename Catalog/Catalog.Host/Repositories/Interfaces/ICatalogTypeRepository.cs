namespace Catalog.Host.Repositories.Interfaces
{
    public interface ICatalogTypeRepository
    {
        Task Add(string type);
        Task Remove(int id);
        Task Update(int id, string type);
    }
}
