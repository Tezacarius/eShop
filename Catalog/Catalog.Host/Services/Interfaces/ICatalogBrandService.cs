namespace Catalog.Host.Services.Interfaces
{
    public interface ICatalogBrandService
    {
        Task Add(string brand);
        Task Remove(int id);
        Task Update(int id, string brand);
    }
}
