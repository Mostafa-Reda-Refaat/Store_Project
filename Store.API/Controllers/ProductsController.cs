using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.API.Helper;
using Store.Repository.Specification.ProductSpec;
using Store.Service.PaginatedDto;
using Store.Service.Services.ProductServices;
using Store.Service.Services.ProductServices.Dtos;

namespace Store.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductsController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet]
        [Cache(90)]
        public async Task<ActionResult<IReadOnlyList<BrandTypeDetalisDto>>> GetAllBrands()
            => Ok(await productService.GetAllBrandsAsync());

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<BrandTypeDetalisDto>>> GetAllTypes()
            => Ok(await productService.GetAllTypesAsync());

        [HttpGet]
        public async Task<ActionResult<PaginatedResultDto<ProductDetailsDto>>> GetAllProducts([FromQuery] ProductSpecification input)
            => Ok(await productService.GetAllProductsAsync(input));

        [HttpGet]
        public async Task<ActionResult<ProductDetailsDto>> GetProduct(int? id)
            => Ok(await productService.GetProductByIdAsync(id));
    }
}
