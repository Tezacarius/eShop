namespace Catalog.Host.Services.Interfaces
{
    public interface ICatalogTypeService
    {
        Task Add(string type);
        Task Remove(int id);
        Task Update(int id, string typep);
    }
}
