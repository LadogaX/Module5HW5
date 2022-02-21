using System.Threading;
using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Response;

namespace Catalog.UnitTests.Services;

public class CatalogServiceTest
{
    private readonly ICatalogService _catalogService;

    private readonly Mock<ICatalogItemRepository> _catalogItemRepository;
    private readonly Mock<ICatalogBrandRepository> _catalogBrandRepository;
    private readonly Mock<ICatalogTypeRepository> _catalogTypeRepository;

    private readonly Mock<IMapper> _mapper;
    private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
    private readonly Mock<ILogger<CatalogService>> _logger;

    public CatalogServiceTest()
    {
        _catalogItemRepository = new Mock<ICatalogItemRepository>();
        _catalogBrandRepository = new Mock<ICatalogBrandRepository>();
        _catalogTypeRepository = new Mock<ICatalogTypeRepository>();

        _mapper = new Mock<IMapper>();
        _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
        _logger = new Mock<ILogger<CatalogService>>();

        var dbContextTransaction = new Mock<IDbContextTransaction>();
        _dbContextWrapper.Setup(s => s.BeginTransactionAsync(CancellationToken.None)).ReturnsAsync(dbContextTransaction.Object);

        _catalogService = new CatalogService(
            _dbContextWrapper.Object,
            _logger.Object,
            _catalogItemRepository.Object,
            _catalogBrandRepository.Object,
            _catalogTypeRepository.Object,
            _mapper.Object);
    }

    [Fact]
    public async Task GetCatalogItemsAsync_Success()
    {
        // arrange
        var testPageIndex = 0;
        var testPageSize = 10;
        var testTotalCount = 12;
        var testName = "TestName";

        var pagingPaginatedItemsSuccess = new PaginatedItems<CatalogItem>()
        {
            Data = new List<CatalogItem>()
            {
                new CatalogItem()
                {
                    Name = testName,
                },
            },
            TotalCount = testTotalCount,
        };

        var catalogItemSuccess = new CatalogItem()
        {
            Name = testName
        };

        var catalogItemDtoSuccess = new CatalogItemDto()
        {
            Name = testName
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
    public async Task GetCatalogByIdAsync_Success()
    {
        // arrange
        var testPageIndex = 1;
        var testPageSize = 1;
        var testTotalCount = 12;
        var testName = "TestName";
        var testId = 1;

        var pagingPaginatedItemsSuccess = new PaginatedItems<CatalogItem>()
        {
            Data = new List<CatalogItem>()
            {
                new CatalogItem()
                {
                    Name = testName,
                },
            },
            TotalCount = testTotalCount,
        };

        var catalogItemSuccess = new CatalogItem()
        {
            Name = testName
        };

        var catalogItemDtoSuccess = new CatalogItemDto()
        {
            Name = testName
        };

        _catalogItemRepository.Setup(s => s.GetByIdAsync(It.Is<int>(i => i == testId))).ReturnsAsync(pagingPaginatedItemsSuccess);

        _mapper.Setup(s => s.Map<CatalogItemDto>(It.Is<CatalogItem>(i => i.Equals(catalogItemSuccess)))).Returns(catalogItemDtoSuccess);

        // act
        var result = await _catalogService.GetCatalogByIdAsync(testId);

        // assert
        result.Should().NotBeNull();
        result?.Data.Should().NotBeNull();
        result?.Count.Should().Be(testTotalCount);
        result?.PageIndex.Should().Be(testPageIndex);
        result?.PageSize.Should().Be(testPageSize);
    }

    [Fact]
    public async Task GetCatalogByIdAsync_Failed()
    {
        // arrange
        var testId = -1;

        _catalogItemRepository.Setup(s => s.GetByIdAsync(
            It.Is<int>(i => i == testId))).Returns((Func<PaginatedItemsResponse<CatalogItemDto>>)null!);

        // act
        var result = await _catalogService.GetCatalogByIdAsync(testId);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetCatalogByBrandAsync_Success()
    {
        // arrange
        var testPageIndex = 0;
        var testPageSize = 10;
        var testTotalCount = 12;
        var testName = "TestName";
        var testNaneBrand = "Azure";

        var pagingPaginatedItemsSuccess = new PaginatedItems<CatalogItem>()
        {
            Data = new List<CatalogItem>()
            {
                new CatalogItem()
                {
                    Name = testName,
                },
            },
            TotalCount = testTotalCount,
        };

        var catalogItemSuccess = new CatalogItem()
        {
            Name = testName
        };

        var catalogItemDtoSuccess = new CatalogItemDto()
        {
            Name = testName
        };

        _catalogItemRepository.Setup(s => s.GetByBrandAsync(
            It.Is<int>(i => i == testPageIndex),
            It.Is<int>(i => i == testPageSize),
            It.Is<string>(s => s == testNaneBrand))).ReturnsAsync(pagingPaginatedItemsSuccess);

        _mapper.Setup(s => s.Map<CatalogItemDto>(
            It.Is<CatalogItem>(i => i.Equals(catalogItemSuccess)))).Returns(catalogItemDtoSuccess);

        // act
        var result = await _catalogService.GetCatalogByBrandAsync(testPageSize, testPageIndex, testNaneBrand);

        // assert
        result.Should().NotBeNull();
        result?.Data.Should().NotBeNull();
        result?.Count.Should().Be(testTotalCount);
        result?.PageIndex.Should().Be(testPageIndex);
        result?.PageSize.Should().Be(testPageSize);
    }

    [Fact]
    public async Task GetCatalogByBrandAsync_Failed()
    {
        // arrange
        var testPageIndex = 1000;
        var testPageSize = 10000;
        var testNaneBrand = "Az_ure";

        _catalogItemRepository.Setup(s => s.GetByBrandAsync(
            It.Is<int>(i => i == testPageIndex),
            It.Is<int>(i => i == testPageSize),
            It.Is<string>(s => s == testNaneBrand))).Returns((Func<PaginatedItemsResponse<CatalogItemDto>>)null!);

        // act
        var result = await _catalogService.GetCatalogByBrandAsync(testPageSize, testPageIndex, testNaneBrand);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetCatalogByTypeAsync_Success()
    {
        // arrange
        var testPageIndex = 1;
        var testPageSize = 10;
        var testTotalCount = 12;
        var testName = "TestName";
        var testNameType = "Mug";

        var pagingPaginatedItemsSuccess = new PaginatedItems<CatalogItem>()
        {
            Data = new List<CatalogItem>()
            {
                new CatalogItem()
                {
                    Name = testName,
                },
            },
            TotalCount = testTotalCount,
        };

        var catalogItemSuccess = new CatalogItem()
        {
            Name = testName
        };

        var catalogItemDtoSuccess = new CatalogItemDto()
        {
            Name = testName
        };

        _catalogItemRepository.Setup(s => s.GetByTypeAsync(
            It.Is<int>(i => i == testPageIndex),
            It.Is<int>(i => i == testPageSize),
            It.Is<string>(s => s == testNameType))).ReturnsAsync(pagingPaginatedItemsSuccess);

        _mapper.Setup(s => s.Map<CatalogItemDto>(
            It.Is<CatalogItem>(i => i.Equals(catalogItemSuccess)))).Returns(catalogItemDtoSuccess);

        // act
        var result = await _catalogService.GetCatalogByTypeAsync(testPageSize, testPageIndex, testNameType);

        // assert
        result.Should().NotBeNull();
        result?.Data.Should().NotBeNull();
        result?.Count.Should().Be(testTotalCount);
        result?.PageIndex.Should().Be(testPageIndex);
        result?.PageSize.Should().Be(testPageSize);
    }

    [Fact]
    public async Task GetCatalogByTypeAsync_Failed()
    {
        // arrange
        var testPageIndex = 1000;
        var testPageSize = 10000;
        var testNameType = "M_g";

        _catalogItemRepository.Setup(s => s.GetByTypeAsync(
            It.Is<int>(i => i == testPageIndex),
            It.Is<int>(i => i == testPageSize),
            It.Is<string>(s => s == testNameType))).Returns((Func<PaginatedItemsResponse<CatalogItemDto>>)null!);

        // act
        var result = await _catalogService.GetCatalogByTypeAsync(testPageSize, testPageIndex, testNameType);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetBrandsAsync_Success()
    {
        // arrange
        var testPageIndex = 1;
        var testPageSize = 10;
        var testTotalCount = 12;
        var testName = "TestName";

        var pagingPaginatedBrandsSuccess = new PaginatedItems<CatalogBrand>()
        {
            Data = new List<CatalogBrand>()
            {
                new CatalogBrand()
                {
                    Brand = testName,
                },
            },
            TotalCount = testTotalCount,
        };

        var catalogBrandSuccess = new CatalogBrand()
        {
            Brand = testName
        };

        var catalogBrandDtoSuccess = new CatalogBrandDto()
        {
            Brand = testName
        };

        _catalogBrandRepository.Setup(s => s.GetBrandsAsync(
            It.Is<int>(i => i == testPageIndex),
            It.Is<int>(i => i == testPageSize))).ReturnsAsync(pagingPaginatedBrandsSuccess);

        _mapper.Setup(s => s.Map<CatalogBrandDto>(
            It.Is<CatalogBrand>(i => i.Equals(catalogBrandSuccess)))).Returns(catalogBrandDtoSuccess);

        // act
        var result = await _catalogService.GetBrandsAsync(testPageSize, testPageIndex);

        // assert
        result.Should().NotBeNull();
        result?.Data.Should().NotBeNull();
        result?.Count.Should().Be(testTotalCount);
        result?.PageIndex.Should().Be(testPageIndex);
        result?.PageSize.Should().Be(testPageSize);
    }

    [Fact]
    public async Task GetBrandsAsync_Failed()
    {
        // arrange
        var testPageIndex = 1000;
        var testPageSize = 10000;

        _catalogBrandRepository.Setup(s => s.GetBrandsAsync(
            It.Is<int>(i => i == testPageIndex),
            It.Is<int>(i => i == testPageSize))).Returns((Func<PaginatedItemsResponse<CatalogBrandDto>>)null!);

        // act
        var result = await _catalogService.GetBrandsAsync(testPageSize, testPageIndex);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetTypesAsync_Success()
    {
        // arrange
        var testPageIndex = 1;
        var testPageSize = 10;
        var testTotalCount = 12;
        var testName = "TestName";

        var pagingPaginatedTypesSuccess = new PaginatedItems<CatalogType>()
        {
            Data = new List<CatalogType>()
            {
                new CatalogType()
                {
                    Type = testName,
                },
            },
            TotalCount = testTotalCount,
        };

        var catalogTypeSuccess = new CatalogType()
        {
            Type = testName
        };

        var catalogTypeDtoSuccess = new CatalogTypeDto()
        {
            Type = testName
        };

        _catalogTypeRepository.Setup(s => s.GetTypesAsync(
            It.Is<int>(i => i == testPageIndex),
            It.Is<int>(i => i == testPageSize))).ReturnsAsync(pagingPaginatedTypesSuccess);

        _mapper.Setup(s => s.Map<CatalogTypeDto>(
            It.Is<CatalogBrand>(i => i.Equals(catalogTypeSuccess)))).Returns(catalogTypeDtoSuccess);

        // act
        var result = await _catalogService.GetTypesAsync(testPageSize, testPageIndex);

        // assert
        result.Should().NotBeNull();
        result?.Data.Should().NotBeNull();
        result?.Count.Should().Be(testTotalCount);
        result?.PageIndex.Should().Be(testPageIndex);
        result?.PageSize.Should().Be(testPageSize);
    }

    [Fact]
    public async Task GetTypesAsync_Failed()
    {
        // arrange
        var testPageIndex = 1000;
        var testPageSize = 10000;

        _catalogBrandRepository.Setup(s => s.GetBrandsAsync(
            It.Is<int>(i => i == testPageIndex),
            It.Is<int>(i => i == testPageSize))).Returns((Func<PaginatedItemsResponse<CatalogBrandDto>>)null!);

        // act
        var result = await _catalogService.GetTypesAsync(testPageSize, testPageIndex);

        // assert
        result.Should().BeNull();
    }
}