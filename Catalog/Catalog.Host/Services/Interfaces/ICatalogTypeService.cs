namespace Catalog.Host.Services.Interfaces
{
    public interface ICatalogTypeService
    {
        Task<int?> AddAsync(string type);
        Task<int?> RemoveAsync(int id);
        Task<int?> UpdateAsync(int id, string type);
    }
}