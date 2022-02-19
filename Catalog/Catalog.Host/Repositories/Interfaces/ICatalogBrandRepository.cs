namespace Catalog.Host.Repositories.Interfaces
{
    public interface ICatalogBrandRepository
    {
        Task Add(string brand);
        Task Remove(int id);
        Task Update(int id, string brand);
    }
}
