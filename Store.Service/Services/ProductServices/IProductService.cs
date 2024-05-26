using Store.Repository.Specification.ProductSpec;
using Store.Service.PaginatedDto;
using Store.Service.Services.ProductServices.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.ProductServices
{
    public interface IProductService
    {
        Task<ProductDetailsDto> GetProductByIdAsync(int? id);
        Task<PaginatedResultDto<ProductDetailsDto>> GetAllProductsAsync(ProductSpecification input);
        Task<IReadOnlyList<BrandTypeDetalisDto>> GetAllBrandsAsync();
        Task<IReadOnlyList<BrandTypeDetalisDto>> GetAllTypesAsync();

    }
}
