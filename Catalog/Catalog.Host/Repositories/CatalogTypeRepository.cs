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

        public async Task Add(string type)
        {
            var item = await _dbContext.AddAsync(new CatalogType
            {
                Type = type
            });

            await _dbContext.SaveChangesAsync();
        }

        public async Task Remove(int id)
        {
            var item = new CatalogType { Id = id };

            _dbContext.CatalogTypes.Remove(item);

            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(int id, string type)
        {
            var item = await _dbContext.CatalogTypes
            .FirstOrDefaultAsync(i => i.Id == id);

            if (item != null)
            {
                item.Type = type;

                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
