using System.Linq;
using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

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

    public async Task<int?> Add(string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, string pictureFileName)
    {
        var item = await _dbContext.AddAsync(new CatalogItem
        {
            CatalogBrandId = catalogBrandId,
            CatalogTypeId = catalogTypeId,
            Description = description,
            Name = name,
            PictureFileName = pictureFileName,
            Price = price
        });

        await _dbContext.SaveChangesAsync();

        return item.Entity.Id;
    }

    public async Task<int?> Update(int id, string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, string pictureFileName)
    {
        var item = await _dbContext.CatalogItems.FirstOrDefaultAsync(x => x.Id == id);

        if (item != null)
        {
            item.CatalogBrandId = catalogBrandId;
            item.CatalogTypeId = catalogTypeId;
            item.Description = description;
            item.Name = name;
            item.PictureFileName = pictureFileName;
            item.Price = price;
            item.AvailableStock = availableStock;
            await _dbContext.SaveChangesAsync();

            return item.Id;
        }

        return default(int?);
    }

    public async Task<int?> Remove(int id)
    {
        var item = await _dbContext.CatalogItems.FirstOrDefaultAsync(x => x.Id == id);

        if (item == null)
        {
            return null;
        }

        var result = _dbContext.Remove(item);
        await _dbContext.SaveChangesAsync();

        return result.Entity.Id;
    }

    public async Task<PaginatedItems<CatalogItem>> GetByIdAsync(int id)
    {
        var totalItems = await _dbContext.CatalogItems
            .LongCountAsync();

        var itemsOnPage = await _dbContext.CatalogItems
            .Where(x => x.Id == id)
            .Include(i => i.CatalogBrand)
            .Include(i => i.CatalogType)
            .OrderBy(c => c.Name)
            .ToListAsync();

        return new PaginatedItems<CatalogItem>() { TotalCount = totalItems, Data = itemsOnPage };
    }

    public async Task<PaginatedItems<CatalogItem>> GetByBrandAsync(int pageIndex, int pageSize, string nameBrand)
    {
        var totalItems = await _dbContext.CatalogItems
            .LongCountAsync();

        var itemsOnPage = await _dbContext.CatalogItems
            .Include(i => i.CatalogBrand)
            .Where(w => w.CatalogBrand.Brand.Contains(nameBrand))
            .Include(i => i.CatalogType)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .OrderBy(c => c.Name)
            .ToListAsync();

        return new PaginatedItems<CatalogItem>() { TotalCount = totalItems, Data = itemsOnPage };
    }

    public async Task<PaginatedItems<CatalogItem>> GetByTypeAsync(int pageIndex, int pageSize, string nameType)
    {
        var totalItems = await _dbContext.CatalogItems
            .LongCountAsync();

        var itemsOnPage = await _dbContext.CatalogItems
            .Include(i => i.CatalogBrand)
            .Include(i => i.CatalogType)
            .Where(w => w.CatalogType.Type.Contains(nameType))
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .OrderBy(c => c.Name)
            .ToListAsync();

        return new PaginatedItems<CatalogItem>() { TotalCount = totalItems, Data = itemsOnPage };
    }
}