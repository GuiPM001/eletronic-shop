using EletronicShop.Domain.Entities;
using EletronicShop.Domain.Repositories;
using EletronicShop.Domain.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EletronicShop.Tests.Services
{
    public class ProductServiceTest
    {
        private readonly ProductService productService;
        private readonly Mock<IProductRepository> productRepository;

        public ProductServiceTest()
        {
            productRepository = new Mock<IProductRepository>();
            productService = new ProductService(productRepository.Object);
        }

        [Fact]
        public async Task MustRegisterProduct_Success()
        {
            // Arrange
            var product = new Product("Test", 10, 150.0, "tBrand");

            productRepository.Setup(p => p.RegisterProduct(product));

            // Act
            await productService.RegisterProduct(product);

            // Assert
            productRepository.Verify(p => p.RegisterProduct(product), Times.Once);
        }

        [Fact]
        public async Task MustNotRegisterProduct_ProductAlreadyExist_Error()
        {
            // Arrange
            var product = new Product("Test", 10, 150.0, "tBrand");

            productRepository.Setup(p => p.RegisterProduct(product));
            productRepository.Setup(p => p.GetProductByName(product.Name)).ReturnsAsync(product);

            // Act and Assert
            await Assert.ThrowsAsync<Exception>(async () => await productService.RegisterProduct(product));
            productRepository.Verify(p => p.RegisterProduct(product), Times.Never);
        }
    }
}
