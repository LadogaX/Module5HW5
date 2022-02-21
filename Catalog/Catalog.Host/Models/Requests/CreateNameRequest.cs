namespace Catalog.Host.Models.Requests
{
    public class CreateNameRequest
    {
        public string Name { get; set; } = null!;

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
