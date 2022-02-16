using System.Threading;
using Catalog.Host.Data.Entities;

namespace Catalog.UnitTests.Services;

public class CatalogBrandServiceTest
{
    private readonly ICatalogBrandService _catalogService;

    private readonly Mock<ICatalogBrandRepository> _catalogBrandRepository;
    private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
    private readonly Mock<ILogger<CatalogService>> _logger;

    private readonly CatalogBrand _testItem = new CatalogBrand()
    {
        Brand = "Name"
    };

    public CatalogBrandServiceTest()
    {
        _catalogBrandRepository = new Mock<ICatalogBrandRepository>();
        _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
        _logger = new Mock<ILogger<CatalogService>>();

        var dbContextTransaction = new Mock<IDbContextTransaction>();
        _dbContextWrapper.Setup(s => s.BeginTransactionAsync(CancellationToken.None)).ReturnsAsync(dbContextTransaction.Object);

        _catalogService = new CatalogBrandService(_dbContextWrapper.Object, _logger.Object, _catalogBrandRepository.Object);
    }
}