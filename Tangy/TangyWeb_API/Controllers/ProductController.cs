using Microsoft.AspNetCore.Mvc;
using Tangy_Business.Repository.IRepository;
using Tangy_Models;

namespace TangyWeb_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // GET: api/Product
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var objList = await _productRepository.GetAllAsync();
            return Ok(objList);
        }

        // GET: api/Product/{id}
        [HttpGet("{productId}")]
        public async Task<IActionResult> Get(int? productId)
        {
            if (productId is null || productId == 0) 
            {
                return BadRequest(new ErrorModelDto() 
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = "Invalid Product Id."
                });
            }

            var product = await _productRepository.GetAsync(productId.Value);
            if (product.Id == 0)
            {
                return NotFound(new ErrorModelDto()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    ErrorMessage = $"Could not find a product with an ID of {productId.Value}"
                });
            }

            return Ok(product);
        }
    }
}
