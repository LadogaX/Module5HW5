using System.Threading;
using Catalog.Host.Data.Entities;

namespace Catalog.UnitTests.Services
{
    public class CatalogTypeServiceTest
    {
        private readonly ICatalogTypeService _catalogTypeService;

        private readonly Mock<ICatalogTypeRepository> _catalogTypeRepository;
        private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
        private readonly Mock<ILogger<CatalogTypeService>> _logger;

        private readonly CatalogType _testItem = new CatalogType()
        {
            Type = "NameType"
        };
        public CatalogTypeServiceTest()
        {
            _catalogTypeRepository = new Mock<ICatalogTypeRepository>();
            _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
            _logger = new Mock<ILogger<CatalogTypeService>>();

            var dbContextTransaction = new Mock<IDbContextTransaction>();
            _dbContextWrapper.Setup(s => s.BeginTransactionAsync(CancellationToken.None)).ReturnsAsync(dbContextTransaction.Object);

            _catalogTypeService = new CatalogTypeService(_dbContextWrapper.Object, _logger.Object, _catalogTypeRepository.Object);
        }

        [Fact]
        public async Task AddAsync_Success()
        {
            // arrange
            var testResult = 1;

            _catalogTypeRepository.Setup(s => s.Add(
                It.IsAny<string>())).ReturnsAsync(testResult);

            // act
            var result = await _catalogTypeService.AddAsync(_testItem.Type);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task AddAsync_Failed()
        {
            // arrange
            int? testResult = null;

            _catalogTypeRepository.Setup(s => s.Add(
                It.IsAny<string>())).ReturnsAsync(testResult);

            // act
            var result = await _catalogTypeService.AddAsync(_testItem.Type);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task Update_Success()
        {
            // arrange
            var testResult = 1;
            var testId = 1;

            _catalogTypeRepository.Setup(s => s.Update(
                It.IsAny<int>(),
                It.IsAny<string>())).ReturnsAsync(testResult);

            // act
            var result = await _catalogTypeService.UpdateAsync(testId, _testItem.Type);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task Update_Failed()
        {
            // arrange
            int? testResult = null;
            var testId = 1;

            _catalogTypeRepository.Setup(s => s.Update(
                It.IsAny<int>(),
                It.IsAny<string>())).ReturnsAsync(testResult);

            // act
            var result = await _catalogTypeService.UpdateAsync(testId, _testItem.Type);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task Remove_Success()
        {
            // arrange
            var testResult = 1;
            var testId = 1;

            _catalogTypeRepository.Setup(s => s.Remove(It.IsAny<int>())).ReturnsAsync(testResult);

            // act
            var result = await _catalogTypeService.RemoveAsync(testId);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task Remove_Failed()
        {
            // arrange
            int? testResult = null;
            var testId = 1;

            _catalogTypeRepository.Setup(s => s.Remove(It.IsAny<int>())).ReturnsAsync(testResult);

            // act
            var result = await _catalogTypeService.RemoveAsync(testId);

            // assert
            result.Should().Be(testResult);
        }
    }
}
