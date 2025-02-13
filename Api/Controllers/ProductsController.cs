using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductRepository repository) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand, string? type, string? sort)
        {
            var products = await repository.GetProductsAsync(brand, type, sort);
            return Ok(products);
        }

        [HttpGet("{id:int}")] // api/products/2
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await repository.GetProductByIdAsync(id);

            if (product is null)
                return NotFound();

            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            repository.AddProduct(product);

            if (await repository.SaveChangesAsync())
            {
                return CreatedAtAction("GetProduct", new { id = product.Id }, product);
            }

            return BadRequest("Problem creating product");
        }

        private bool ProductExists(int id)
        {
            return repository.ProductExists(id);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id, Product product)
        {
            if (product.Id != id || !ProductExists(id))
                return BadRequest("Cannot update this product");

            repository.UpdateProduct(product);

            if (await repository.SaveChangesAsync())
                return Ok();

            return BadRequest("Problem updating the product");
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await repository.GetProductByIdAsync(id);

            if (product is null)
                return NotFound();

            repository.DeleteProduct(product);

            if (await repository.SaveChangesAsync())
                return Ok();

            return BadRequest("Problem deleting the product");
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
        {
            var brands = await repository.GetBrandsAsync();
            return Ok(brands);
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
        {
            var types = await repository.GetTypesAsync();
            return Ok(types);
        }
    }
}
