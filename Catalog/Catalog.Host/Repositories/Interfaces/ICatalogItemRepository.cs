using Catalog.Host.Data;
using Catalog.Host.Data.Entities;

namespace Catalog.Host.Repositories.Interfaces;

public interface ICatalogItemRepository
{
    Task<PaginatedItems<CatalogItem>> GetByPageAsync(int pageIndex, int pageSize, int? brandFilter, int? typeFilter);
    Task<CatalogItem> GetItemByIdAsync(int id);
    Task<IReadOnlyCollection<CatalogItem>> GetItemsByBrandAsync(string brand);
    Task<IReadOnlyCollection<CatalogItem>> GetItemsByTypeAsync(string type);
    Task<IReadOnlyCollection<CatalogBrand>> GetBrandsAsync();
    Task<IReadOnlyCollection<CatalogType>> GetTypesAsync();
    Task<int?> Add(string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, string pictureFileName);
    Task Remove(int id);
    Task Update(int id, string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, string pictureFileName);
}