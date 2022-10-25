using EletronicShop.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace EletronicShop.Domain.Services.Interfaces
{
    public interface IProductService
    {
        Task RegisterProduct(Product product);
        Task RegisterProductsFromFile(IFormFile file);
        Task AddImage(int id, IFormFile image);
        Task<IEnumerable<Product>> GetAllProducts();
    }
}
