using ApiRest.DTO;
using ApiRest.Model;
using ApiRest.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiRest.Controllers
{
    [Route("products")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProductsController : ControllerBase
    {
        private readonly IRepositoryProducts repository = new RepoInMemoryProducts();
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IRepositoryProducts repository, ILogger<ProductsController> logger)
        {
            this.repository = repository;
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        public async Task<IEnumerable<DtoProduct>> GetProductsAsync()
        {
            var products = (await repository.ReadAsync()).Select(p => p.ToDTO());

            return products;
        }

        [HttpGet("/{productCode}")]
        public async Task<ActionResult<DtoProduct>> GetProduct(string productCode)
        {
            var product = await repository.ReadAsync(productCode);

            if (product is null)
            {
                _logger.LogError(" ### Item not found: " + productCode);
                return NotFound();
            }

            return product.ToDTO();
        }


        [HttpGet("/{page}/{size}")]
        public async Task<IEnumerable<DtoProduct>> GetProduct(int page, int size)
        {
            var product = await repository.ReadAsync(page, size);

            if (product is null)
            {
                _logger.LogError(" ### There are no items for page {0} and page size {1}: ", page, size);
            }

            return product.Select(p => p.ToDTO());
        }


        [HttpPost]
        public ActionResult<DtoProduct> CreateProduct( DtoProduct newProduct ) 
        {
            Product product = new()
            {
                //Id = repository.ReadAsync().Max(p => p.Id) + 1,
                Name = newProduct.Name,
                Description = newProduct.Description,
                Price = newProduct.Price,
                SKU = newProduct.SKU,
                CreationDate = DateTime.Now,
            };

            repository.Create(product);

            return product.ToDTO();
        }

        [HttpPut]
        public async Task<ActionResult<DtoProduct>> ModifyProduct(DtoProduct modifiedProduct)
        {
            if (modifiedProduct is null)
            {
                return NotFound();
            }

            Product? product = await repository.ReadAsync(modifiedProduct.SKU ?? "");

            if (product is null)
            {
                return NotFound();
            }

            product.Name = modifiedProduct.Name;
            product.Description = modifiedProduct.Description;
            product.Price = modifiedProduct.Price;

            repository.Update(product);

            return product.ToDTO();
        }

        [HttpDelete]
        public async Task<ActionResult<DtoProduct>> DeleteProduct( string productCode )
        {
            Product? product = await repository.ReadAsync(productCode);

            if (product is null)
            {
                return NotFound();
            }

            repository.Delete( productCode );

            return product.ToDTO();
        }
    }
}
