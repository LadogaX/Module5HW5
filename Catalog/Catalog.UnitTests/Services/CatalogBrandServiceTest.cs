using System.Threading;
using Catalog.Host.Data.Entities;

namespace Catalog.UnitTests.Services
{
    public class CatalogBrandServiceTest
    {
        private readonly ICatalogBrandService _catalogBrandService;

        private readonly Mock<ICatalogBrandRepository> _catalogBrandRepository;
        private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
        private readonly Mock<ILogger<CatalogBrandService>> _logger;

        private readonly CatalogBrand _testItem = new CatalogBrand()
        {
            Brand = "NameBrand"
        };
        public CatalogBrandServiceTest()
        {
            _catalogBrandRepository = new Mock<ICatalogBrandRepository>();
            _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
            _logger = new Mock<ILogger<CatalogBrandService>>();

            var dbContextTransaction = new Mock<IDbContextTransaction>();
            _dbContextWrapper.Setup(s => s.BeginTransactionAsync(CancellationToken.None)).ReturnsAsync(dbContextTransaction.Object);

            _catalogBrandService = new CatalogBrandService(_dbContextWrapper.Object, _logger.Object, _catalogBrandRepository.Object);
        }

        [Fact]
        public async Task AddAsync_Success()
        {
            // arrange
            var testResult = 1;

            _catalogBrandRepository.Setup(s => s.Add(
                It.IsAny<string>())).ReturnsAsync(testResult);

            // act
            var result = await _catalogBrandService.AddAsync(_testItem.Brand);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task AddAsync_Failed()
        {
            // arrange
            int? testResult = null;

            _catalogBrandRepository.Setup(s => s.Add(
                It.IsAny<string>())).ReturnsAsync(testResult);

            // act
            var result = await _catalogBrandService.AddAsync(_testItem.Brand);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task Update_Success()
        {
            // arrange
            var testResult = 1;
            var testId = 1;

            _catalogBrandRepository.Setup(s => s.Update(
                It.IsAny<int>(),
                It.IsAny<string>())).ReturnsAsync(testResult);

            // act
            var result = await _catalogBrandService.UpdateAsync(testId, _testItem.Brand);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task Update_Failed()
        {
            // arrange
            int? testResult = null;
            var testId = 1;

            _catalogBrandRepository.Setup(s => s.Update(
                It.IsAny<int>(),
                It.IsAny<string>())).ReturnsAsync(testResult);

            // act
            var result = await _catalogBrandService.UpdateAsync(testId, _testItem.Brand);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task Remove_Success()
        {
            // arrange
            var testResult = 1;
            var testId = 1;

            _catalogBrandRepository.Setup(s => s.Remove(It.IsAny<int>())).ReturnsAsync(testResult);

            // act
            var result = await _catalogBrandService.RemoveAsync(testId);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task Remove_Failed()
        {
            // arrange
            int? testResult = null;
            var testId = 1;

            _catalogBrandRepository.Setup(s => s.Remove(It.IsAny<int>())).ReturnsAsync(testResult);

            // act
            var result = await _catalogBrandService.RemoveAsync(testId);

            // assert
            result.Should().Be(testResult);
        }
    }
}
