using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Catalog.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(IProductRepository productRepository, ILogger<CatalogController> logger)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        // GET: api/<CatalogController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _productRepository.GetProducts();
            return Ok(products);
        }

        // GET api/<CatalogController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> GetProduct(string id)
        {
            var product = await _productRepository.GetProduct(id);
            if (product is not null)
            {
                return Ok(product);
            }
            _logger.LogError($"Product with Id: {id} could not be found on method {nameof(GetProduct)}");
            return NotFound();
        }

        // GET api/<CatalogController>/?name=IphoneX
        [Route("[action]/{name}", Name = nameof(GetProductByName))]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByName(string name)
        {
            var products = await _productRepository.GetProductByName(name);
            return Ok(products);
        }

        // GET api/<CatalogController>/?categoryName=Accessories
        [Route("[action]/{category}", Name = nameof(GetProductByCategory))]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string category)
        {
            var products = await _productRepository.GetProductByCategory(category);
            return Ok(products);
        }

        // POST api/<CatalogController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            await _productRepository.CreateProduct(product);
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id });
        }

        // PUT api/<CatalogController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> UpdateProduct([FromBody] Product product)
        {
            var updated = await _productRepository.UpdateProduct(product);
            if(updated)
            {
                return NoContent();
            }
            _logger.LogError($"Product with Id: {product.Id} could not be updated on method {nameof(UpdateProduct)}");
            return BadRequest();
        }

        // DELETE api/<CatalogController>/5
        [HttpDelete("{id:length(24)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> DeleteProduct(string id)
        {
            var deleted = await _productRepository.DeleteProduct(id);
            if (deleted)
            {
                return NoContent();
            }
            _logger.LogError($"Product with Id: {id} could not be deteleted on method {nameof(DeleteProduct)}");
            return BadRequest();
        }
    }
}
