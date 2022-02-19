using System.Threading;
using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Response;

namespace Catalog.UnitTests.Services;

public class CatalogServiceTest
{
    private readonly ICatalogService _catalogService;

    private readonly Mock<ICatalogItemRepository> _catalogItemRepository;
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
    private readonly Mock<ILogger<CatalogService>> _logger;

    public CatalogServiceTest()
    {
        _catalogItemRepository = new Mock<ICatalogItemRepository>();
        _mapper = new Mock<IMapper>();
        _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
        _logger = new Mock<ILogger<CatalogService>>();

        var dbContextTransaction = new Mock<IDbContextTransaction>();
        _dbContextWrapper.Setup(s => s.BeginTransactionAsync(CancellationToken.None)).ReturnsAsync(dbContextTransaction.Object);

        _catalogService = new CatalogService(_dbContextWrapper.Object, _logger.Object, _catalogItemRepository.Object, _mapper.Object);
    }

    [Fact]
    public async Task GetCatalogItemsAsync_Success()
    {
        // arrange
        var testPageIndex = 0;
        var testPageSize = 4;
        var testTotalCount = 12;

        var pagingPaginatedItemsSuccess = new PaginatedItems<CatalogItem>()
        {
            Data = new List<CatalogItem>()
            {
                new CatalogItem()
                {
                    Name = "TestName",
                },
            },
            TotalCount = testTotalCount,
        };

        var catalogItemSuccess = new CatalogItem()
        {
            Name = "TestName"
        };

        var catalogItemDtoSuccess = new CatalogItemDto()
        {
            Name = "TestName"
        };

        _catalogItemRepository.Setup(s => s.GetByPageAsync(
            It.Is<int>(i => i == testPageIndex),
            It.Is<int>(i => i == testPageSize))).ReturnsAsync(pagingPaginatedItemsSuccess);

        _mapper.Setup(s => s.Map<CatalogItemDto>(
            It.Is<CatalogItem>(i => i.Equals(catalogItemSuccess)))).Returns(catalogItemDtoSuccess);

        // act
        var result = await _catalogService.GetCatalogItemsAsync(testPageSize, testPageIndex);

        // assert
        result.Should().NotBeNull();
        result?.Data.Should().NotBeNull();
        result?.Count.Should().Be(testTotalCount);
        result?.PageIndex.Should().Be(testPageIndex);
        result?.PageSize.Should().Be(testPageSize);
    }

    [Fact]
    public async Task GetCatalogItemsAsync_Failed()
    {
        // arrange
        var testPageIndex = 1000;
        var testPageSize = 10000;

        _catalogItemRepository.Setup(s => s.GetByPageAsync(
            It.Is<int>(i => i == testPageIndex),
            It.Is<int>(i => i == testPageSize))).Returns((Func<PaginatedItemsResponse<CatalogItemDto>>)null!);

        // act
        var result = await _catalogService.GetCatalogItemsAsync(testPageSize, testPageIndex);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetItemByIdAsync_Success()
    {
        // arrage
        var testId = 5;

        var catalogItemSuccess = new CatalogItem()
        {
            Id = testId
        };

        var catalogItemDtoSuccess = new CatalogItemDto()
        {
            Id = testId
        };

        _catalogItemRepository.Setup(s => s.GetItemByIdAsync(
            It.Is<int>(i => i == testId))).ReturnsAsync(catalogItemSuccess);

        _mapper.Setup(s => s.Map<CatalogItemDto>(
            It.Is<CatalogItem>(i => i.Equals(catalogItemSuccess)))).Returns(catalogItemDtoSuccess);

        // act
        var result = await _catalogService.GetItemByIdAsync(testId);

        // assert
        result.Should().NotBeNull();
        result?.Id.Should().Be(testId);
    }

    [Fact]
    public async Task GetItemByIdAsync_Failed()
    {
        // arrange
        var testId = 5000;

        _catalogItemRepository.Setup(s => s.GetItemByIdAsync(
            It.Is<int>(i => i == testId))).Returns((Func<CatalogItemDto>)null!);

        // act
        var result = await _catalogService.GetItemByIdAsync(testId);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetItemsByBrandAsync_Success()
    {
        // arrange
        var testBrand = "TestBrand";

        var catalogBrandSuccess = new CatalogBrand()
        {
            Brand = testBrand
        };

        var catalogBrandDtoSuccess = new CatalogBrandDto()
        {
            Brand = testBrand
        };

        var catalogItemSuccess = new CatalogItem()
        {
            CatalogBrand = catalogBrandSuccess
        };

        var catalogItemDtoSuccess = new CatalogItemDto()
        {
            CatalogBrand = catalogBrandDtoSuccess
        };

        var catalogItemsSuccess = new List<CatalogItem>
        {
            catalogItemSuccess,
            catalogItemSuccess
        };

        _catalogItemRepository.Setup(s => s.GetItemsByBrandAsync(
            It.Is<string>(i => i == testBrand))).ReturnsAsync(catalogItemsSuccess.AsReadOnly());

        _mapper.Setup(s => s.Map<CatalogItemDto>(
            It.Is<CatalogItem>(i => i.Equals(catalogItemSuccess)))).Returns(catalogItemDtoSuccess);

        _mapper.Setup(s => s.Map<CatalogBrandDto>(
            It.Is<CatalogBrand>(i => i.Equals(catalogBrandSuccess)))).Returns(catalogBrandDtoSuccess);

        // act
        var result = await _catalogService.GetItemsByBrandAsync(testBrand);

        // assert
        result.Should().NotBeNull();
        result?.Should().NotContainNulls();
        result?.Should().OnlyContain(x => x.CatalogBrand.Brand == testBrand);
    }

    [Fact]
    public async Task GetItemsByBrandAsync_Failed()
    {
        // arrange
        var testBrand = "None";

        _catalogItemRepository.Setup(s => s.GetItemsByBrandAsync(
            It.Is<string>(i => i == testBrand))).Returns((Func<List<CatalogItemDto>>)null!);

        // act
        var result = await _catalogService.GetItemsByBrandAsync(testBrand);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetItemsByTypeAsync_Success()
    {
        // arrange
        var testType = "TestType";

        var catalogTypeSuccess = new CatalogType()
        {
            Type = testType
        };

        var catalogTypeDtoSuccess = new CatalogTypeDto()
        {
            Type = testType
        };

        var catalogItemSuccess = new CatalogItem()
        {
            CatalogType = catalogTypeSuccess
        };

        var catalogItemDtoSuccess = new CatalogItemDto()
        {
            CatalogType = catalogTypeDtoSuccess
        };

        var catalogItemsSuccess = new List<CatalogItem>
        {
            catalogItemSuccess,
            catalogItemSuccess
        };

        _catalogItemRepository.Setup(s => s.GetItemsByTypeAsync(
            It.Is<string>(i => i == testType))).ReturnsAsync(catalogItemsSuccess.AsReadOnly());

        _mapper.Setup(s => s.Map<CatalogItemDto>(
            It.Is<CatalogItem>(i => i.Equals(catalogItemSuccess)))).Returns(catalogItemDtoSuccess);

        _mapper.Setup(s => s.Map<CatalogTypeDto>(
            It.Is<CatalogType>(i => i.Equals(catalogTypeSuccess)))).Returns(catalogTypeDtoSuccess);

        // act
        var result = await _catalogService.GetItemsByTypeAsync(testType);

        // assert
        result.Should().NotBeNull();
        result?.Should().NotContainNulls();
        result?.Should().OnlyContain(x => x.CatalogType.Type == testType);
    }

    [Fact]
    public async Task GetItemsByTypeAsync_Failed()
    {
        // arrange
        var testType = "None";

        _catalogItemRepository.Setup(s => s.GetItemsByTypeAsync(
            It.Is<string>(i => i == testType))).Returns((Func<List<CatalogItemDto>>)null!);

        // act
        var result = await _catalogService.GetItemsByTypeAsync(testType);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetBrandsAsync_Success()
    {
        // arrange
        var catalogBrandSuccess = new CatalogBrand()
        {
            Brand = "TestBrand"
        };

        var catalogBrandDtoSuccess = new CatalogBrandDto()
        {
            Brand = "TestBrand"
        };

        var brandsSuccess = new List<CatalogBrand> { catalogBrandSuccess };

        _catalogItemRepository.Setup(s => s.GetBrandsAsync()).ReturnsAsync(brandsSuccess);

        _mapper.Setup(s => s.Map<CatalogBrandDto>(
            It.Is<CatalogBrand>(i => i.Equals(catalogBrandSuccess)))).Returns(catalogBrandDtoSuccess);

        // act
        var result = await _catalogService.GetBrandsAsync();

        // assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetBrandsAsync_Failed()
    {
        // arrange
        _catalogItemRepository.Setup(s => s.GetBrandsAsync()).Returns((Func<List<CatalogBrandDto>>)null!);

        // act
        var result = await _catalogService.GetBrandsAsync();

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetTypesAsync_Success()
    {
        // arrange
        var catalogTypeSuccess = new CatalogType()
        {
            Type = "TestType"
        };

        var catalogTypeDtoSuccess = new CatalogTypeDto()
        {
            Type = "TestType"
        };

        var typesSuccess = new List<CatalogType> { catalogTypeSuccess };

        _catalogItemRepository.Setup(s => s.GetTypesAsync()).ReturnsAsync(typesSuccess);

        _mapper.Setup(s => s.Map<CatalogTypeDto>(
            It.Is<CatalogType>(i => i.Equals(catalogTypeSuccess)))).Returns(catalogTypeDtoSuccess);

        // act
        var result = await _catalogService.GetTypesAsync();

        // assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetTypesAsync_Failed()
    {
        // arrange
        _catalogItemRepository.Setup(s => s.GetTypesAsync()).Returns((Func<List<CatalogTypeDto>>)null!);

        // act
        var result = await _catalogService.GetTypesAsync();

        // assert
        result.Should().BeNull();
    }
}