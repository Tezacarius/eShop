using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Host.Repositories
{
    public class CatalogBrandRepository : ICatalogBrandRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<CatalogBrandRepository> _logger;

        public CatalogBrandRepository(
            IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
            ILogger<CatalogBrandRepository> logger)
        {
            _dbContext = dbContextWrapper.DbContext;
            _logger = logger;
        }

        public async Task<int?> Add(string brand)
        {
            var item = new CatalogBrand
            {
                Brand = brand
            };

            var result = await _dbContext.CatalogBrands.AddAsync(item);
            await _dbContext.SaveChangesAsync();

            return result.Entity.Id;
        }

        public async Task<int?> Remove(int id)
        {
            var item = await _dbContext.CatalogBrands.FirstOrDefaultAsync(i => i.Id == id);

            if (item == null)
            {
                return null;
            }

            _dbContext.CatalogBrands.Remove(item);
            await _dbContext.SaveChangesAsync();

            return item.Id;
        }

        public async Task<int?> Update(int id, string brand)
        {
            var item = await _dbContext.CatalogBrands
            .FirstOrDefaultAsync(i => i.Id == id);

            if (item == null)
            {
                return null;
            }

            item.Brand = brand;
            await _dbContext.SaveChangesAsync();

            return item.Id;
        }
    }
}
