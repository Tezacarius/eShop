namespace Catalog.Host.Services.Interfaces
{
    public interface ICatalogTypeService
    {
        Task<int?> Add(string type);
        Task<int?> Remove(int id);
        Task<int?> Update(int id, string typep);
    }
}
