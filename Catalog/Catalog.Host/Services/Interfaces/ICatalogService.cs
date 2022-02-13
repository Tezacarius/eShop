using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Enums;
using Catalog.Host.Models.Response;

namespace Catalog.Host.Services.Interfaces;

public interface ICatalogService
{
    Task<PaginatedItemsResponse<CatalogItemDto>> GetCatalogItemsAsync(int pageSize, int pageIndex);
    Task<CatalogItemDto> GetItemByIdAsync(int id);
    Task<IReadOnlyCollection<CatalogItemDto>> GetItemsByBrandAsync(string brand);
    Task<IReadOnlyCollection<CatalogItemDto>> GetItemsByTypeAsync(string type);
    Task<IReadOnlyCollection<CatalogBrandDto>> GetBrandsAsync();
    Task<IReadOnlyCollection<CatalogTypeDto>> GetTypesAsync();
}