using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories.Interfaces;

namespace Catalog.Host.Repositories;

public class CatalogItemRepository : ICatalogItemRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<CatalogItemRepository> _logger;

    public CatalogItemRepository(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<CatalogItemRepository> logger)
    {
        _dbContext = dbContextWrapper.DbContext;
        _logger = logger;
    }

    public async Task<PaginatedItems<CatalogItem>> GetByPageAsync(int pageIndex, int pageSize, int? brandFilter, int? typeFilter)
    {
        IQueryable<CatalogItem> query = _dbContext.CatalogItems;

        if (brandFilter.HasValue)
        {
            query = query.Where(w => w.CatalogBrandId == brandFilter.Value);
        }

        if (typeFilter.HasValue)
        {
            query = query.Where(w => w.CatalogTypeId == typeFilter.Value);
        }

        var totalItems = await query.LongCountAsync();

        var itemsOnPage = await query.OrderBy(c => c.Name)
           .Include(i => i.CatalogBrand)
           .Include(i => i.CatalogType)
           .Skip(pageSize * pageIndex)
           .Take(pageSize)
           .ToListAsync();

        return new PaginatedItems<CatalogItem>() { TotalCount = totalItems, Data = itemsOnPage };
    }

    public async Task<CatalogItem> GetItemByIdAsync(int id)
    {
        var item = await _dbContext.CatalogItems
            .Include(i => i.CatalogBrand)
            .Include(i => i.CatalogType)
            .FirstOrDefaultAsync(i => i.Id == id);

        return item!;
    }

    public async Task<IReadOnlyCollection<CatalogItem>> GetItemsByBrandAsync(string brand)
    {
        var items = await _dbContext.CatalogItems
            .Include(i => i.CatalogBrand)
            .Include(i => i.CatalogType)
            .OrderBy(c => c.Name)
            .Where(c => c.CatalogBrand.Brand == brand)
            .ToListAsync();

        return new List<CatalogItem>(items);
    }

    public async Task<IReadOnlyCollection<CatalogItem>> GetItemsByTypeAsync(string type)
    {
        var items = await _dbContext.CatalogItems
            .Include(i => i.CatalogBrand)
            .Include(i => i.CatalogType)
            .OrderBy(c => c.Name)
            .Where(c => c.CatalogType.Type == type)
            .ToListAsync();

        return new List<CatalogItem>(items);
    }

    public async Task<IReadOnlyCollection<CatalogBrand>> GetBrandsAsync()
    {
        var items = await _dbContext.CatalogBrands
            .ToListAsync();

        return new List<CatalogBrand>(items);
    }

    public async Task<IReadOnlyCollection<CatalogType>> GetTypesAsync()
    {
        var items = await _dbContext.CatalogTypes
            .ToListAsync();

        return new List<CatalogType>(items);
    }

    public async Task<int?> Add(string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, string pictureFileName)
    {
        var item1 = new CatalogItem
        {
            CatalogBrandId = catalogBrandId,
            CatalogTypeId = catalogTypeId,
            Description = description,
            Name = name,
            PictureFileName = pictureFileName,
            Price = price
        };
        var item = await _dbContext.AddAsync(item1);

        await _dbContext.SaveChangesAsync();

        return item.Entity.Id;
    }

    public async Task Remove(int id)
    {
        var item = new CatalogItem { Id = id };

        _dbContext.CatalogItems.Remove(item);

        await _dbContext.SaveChangesAsync();
    }

    public async Task Update(int id, string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, string pictureFileName)
    {
        var item = await _dbContext.CatalogItems
            .FirstOrDefaultAsync(i => i.Id == id);

        if (item != null)
        {
            item.CatalogBrandId = catalogBrandId;
            item.CatalogTypeId = catalogTypeId;
            item.Description = description;
            item.Name = name;
            item.PictureFileName = pictureFileName;
            item.Price = price;

            await _dbContext.SaveChangesAsync();
        }
    }
}