using System.Threading;
using Catalog.Host.Data.Entities;

namespace Catalog.UnitTests.Services;

public class CatalogTypeServiceTest
{
    private readonly ICatalogTypeService _catalogService;

    private readonly Mock<ICatalogTypeRepository> _catalogTypeRepository;
    private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
    private readonly Mock<ILogger<CatalogService>> _logger;

    private readonly CatalogType _testItem = new CatalogType()
    {
        Type = "Name"
    };

    public CatalogTypeServiceTest()
    {
        _catalogTypeRepository = new Mock<ICatalogTypeRepository>();
        _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
        _logger = new Mock<ILogger<CatalogService>>();

        var dbContextTransaction = new Mock<IDbContextTransaction>();
        _dbContextWrapper.Setup(s => s.BeginTransactionAsync(CancellationToken.None)).ReturnsAsync(dbContextTransaction.Object);

        _catalogService = new CatalogTypeService(_dbContextWrapper.Object, _logger.Object, _catalogTypeRepository.Object);
    }

    [Fact]
    public async Task Add_Success()
    {
        // arrange
        var testResult = 1;

        _catalogTypeRepository.Setup(s => s.Add(
            It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogService.Add(_testItem.Type);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task Add_Failed()
    {
        // arrange
        int? testResult = null;

        _catalogTypeRepository.Setup(s => s.Add(
            It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogService.Add(_testItem.Type);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task Remove_Success()
    {
        // arrange
        var testId = 1;
        int? testResult = 1;

        _catalogTypeRepository.Setup(s => s.Remove(
            It.Is<int>(i => i == testId))).ReturnsAsync(testResult);

        // act
        var result = await _catalogService.Remove(testId);

        // assert
        result.Should().Be(testId);
    }

    [Fact]
    public async Task Remove_Failed()
    {
        // arrange
        int testId = 100;

        _catalogTypeRepository.Setup(s => s.Remove(
            It.Is<int>(i => i == testId))).Returns((Func<int>)null!);

        // act
        var result = await _catalogService.Remove(testId);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_Success()
    {
        // arrange
        var testResult = 1;

        _catalogTypeRepository.Setup(s => s.Update(
            It.IsAny<int>(),
            It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogService.Update(testResult, _testItem.Type);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task UpdateAsync_Failed()
    {
        // arrange
        int? testResult = null;
        int testId = 100;

        _catalogTypeRepository.Setup(s => s.Update(
            It.IsAny<int>(),
            It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogService.Update(testId, _testItem.Type);

        // assert
        result.Should().Be(testResult);
    }
}