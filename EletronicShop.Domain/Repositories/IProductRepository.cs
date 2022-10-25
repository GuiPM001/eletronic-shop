using EletronicShop.Domain.Entities;

namespace EletronicShop.Domain.Repositories
{
    public interface IProductRepository
    {
        Task<int> RegisterProduct(Product product);
        Task AddProductImage(int id, string image);
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product> GetProductByName(string name);
    }
}
