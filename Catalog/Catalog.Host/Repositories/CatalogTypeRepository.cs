using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Host.Repositories
{
    public class CatalogTypeRepository : ICatalogTypeRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<CatalogTypeRepository> _logger;

        public CatalogTypeRepository(
            IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
            ILogger<CatalogTypeRepository> logger)
        {
            _dbContext = dbContextWrapper.DbContext;
            _logger = logger;
        }

        public async Task<int?> Add(string type)
        {
            var item = new CatalogType
            {
                Type = type
            };

            var result = await _dbContext.CatalogTypes.AddAsync(item);
            await _dbContext.SaveChangesAsync();

            return result.Entity.Id;
        }

        public async Task<int?> Remove(int id)
        {
            var item = await _dbContext.CatalogTypes.FirstOrDefaultAsync(i => i.Id == id);

            if (item == null)
            {
                return null;
            }

            _dbContext.CatalogTypes.Remove(item);
            await _dbContext.SaveChangesAsync();

            return item.Id;
        }

        public async Task<int?> Update(int id, string type)
        {
            var item = await _dbContext.CatalogTypes
            .FirstOrDefaultAsync(i => i.Id == id);

            if (item == null)
            {
                return null;
            }

            item.Type = type;
            await _dbContext.SaveChangesAsync();

            return item.Id;
        }
    }
}
