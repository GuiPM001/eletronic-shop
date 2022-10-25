using EletronicShop.Domain.Entities;
using EletronicShop.Domain.Repositories;
using EletronicShop.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace EletronicShop.Domain.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;

        public ProductService(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public async Task RegisterProduct(Product product)
        {
            await CheckProductAlreadyExists(product.Name);

            await productRepository.RegisterProduct(product);
        }

        public async Task RegisterProductsFromFile(IFormFile file)
        {
            var productList = await CreateProductList(file);
            
            foreach(var product in productList)
            {
                await CheckProductAlreadyExists(product.Name);

                await productRepository.RegisterProduct(product);
            }
        }

        public async Task AddImage(int id, IFormFile image)
        {
            var imageData = await ConvertImage(image);

            await productRepository.AddProductImage(id, imageData);
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await productRepository.GetAllProducts();
        }

        #region Private methods
        private async Task CheckProductAlreadyExists(string name)
        {
            var existProduct = await productRepository.GetProductByName(name);
            if (existProduct != null)
            {
                throw new Exception($"Product {name} already exists.");
            }
        }

        private async Task<string> ConvertImage(IFormFile image)
        {
            try
            {
                string filePath = Path.GetTempFileName();
                using (var stream = File.Create(filePath))
                {
                    await image.CopyToAsync(stream);
                }

                var imageData = await File.ReadAllBytesAsync(filePath);

                return Convert.ToBase64String(imageData);
            }
            catch (Exception ex)
            {
                throw new Exception("Error trying convert image", ex);
            }
        }

        private async Task<List<Product>> CreateProductList(IFormFile file)
        {
            List<Product> productList = new List<Product>();

            StreamReader streamReader = new StreamReader(file.OpenReadStream(), Encoding.UTF8, true);
            var streamContent = (await streamReader.ReadToEndAsync()).Split("\r\n").Where(x => x != "").ToList();

            var fileHeader = streamContent[0];
            ValidateFileHeader(fileHeader);

            streamContent = streamContent.Skip(1).ToList();

            foreach (var line in streamContent)
            {
                var lineContent = line.Trim().Split(";");

                string name = lineContent[0];
                int quantity = Convert.ToInt32(lineContent[1]);
                double price = Convert.ToDouble(lineContent[2]);
                string brand = lineContent[3];
                
                productList.Add(new Product(name, quantity, price, brand));
            }

            return productList;
        }

        private void ValidateFileHeader(string fileHeader)
        {
            var expectedHeader = "Name;Quantity;Price;Brand";

            if (fileHeader != expectedHeader)
                throw new Exception("File header is wrong.");
        }
        #endregion
    }
}
