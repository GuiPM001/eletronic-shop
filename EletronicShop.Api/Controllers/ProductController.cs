using EletronicShop.Domain.Entities;
using EletronicShop.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EletronicShop.Api.Controllers
{
    [ApiController]
    [Route("product")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> PostProduct(Product product)
        {
            try
            {
                await productService.RegisterProduct(product);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch]
        [Route("image/{id}")]
        public async Task<IActionResult> AddProductImage(int id, IFormFile image)
        {
            try
            {
                await productService.AddImage(id, image);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("/products")]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await productService.GetAllProducts();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("/products")]
        public async Task<IActionResult> PostProducts(IFormFile file)
        {
            try
            {
                await productService.RegisterProductsFromFile(file);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
