using DataAccess.Models;
using DataAccess.Service;
using Microsoft.AspNetCore.Mvc;

namespace RabbitMQProductAPI.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {


        private readonly IProductService _productService;

        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet("productlist")]
        public async Task<IEnumerable<Product>> GetAllProduct()
        {
            _logger.LogInformation("Serilog is working fine");
            var result = await _productService.GetProductList();

            return result;
        }

        [HttpGet("getproductbyid")]
        public Product GetProductById(int id)
        {
            return _productService.GetProductById(id);
        }

        [HttpPost("addproduct")]

        public async Task<Product> AddProduct(Product product)
        {
            //_logger.LogInformation("Serilog is working fine");

            var productdata = await _productService.AddProduct(product);

            //_rabbitMQproducer.SendProductMessage(productdata);

            return productdata;
        }

        [HttpPut("updateproduct")]
        public Product UpdataProduct(Product product)
        {
            _productService.UpdateProduct(product);
            return product;
        }

        [HttpDelete("deleteproduct")]
        public bool DeleteProduct(int id)
        {
            return _productService.DeleteProduct(id);
        }
    }
}
