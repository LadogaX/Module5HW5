namespace Catalog.Host.Services.Interfaces
{
    public interface ICatalogBrandService
    {
        Task<int?> AddAsync(string brand);
        Task<int?> RemoveAsync(int id);
        Task<int?> UpdateAsync(int id, string brand);
    }
}