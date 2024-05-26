using AutoMapper;
using Store.Repository.Interfaces;
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
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<IReadOnlyList<BrandTypeDetalisDto>> GetAllBrandsAsync()
        {
            var brands = await unitOfWork.ProductBrandRepository.GetAllAsync();

            var mappedBrands = mapper.Map<IReadOnlyList<BrandTypeDetalisDto>>(brands);

            return mappedBrands;
        }

        public async Task<PaginatedResultDto<ProductDetailsDto>> GetAllProductsAsync(ProductSpecification input)
        {
            var specs = new ProductWithSpecifications(input);

            var products = await unitOfWork.ProductRepository.GetAllWithSpecificationAsync(specs);

            var countSpecs = new ProductWithFilterAndCountSpecification(input);

            var count = await unitOfWork.ProductRepository.CountSpecificationAsync(countSpecs);

            var mappedProducts = mapper.Map<IReadOnlyList<ProductDetailsDto>>(products);

            return new PaginatedResultDto<ProductDetailsDto>(input.PageIndex, input.PageSize, count, mappedProducts);
        }

        public async Task<IReadOnlyList<BrandTypeDetalisDto>> GetAllTypesAsync()
        {
            var Types = await unitOfWork.ProductTypeRepository.GetAllAsync();

            var mappedTypes = mapper.Map<IReadOnlyList<BrandTypeDetalisDto>>(Types);

            return mappedTypes;
        }

        public async Task<ProductDetailsDto> GetProductByIdAsync(int? id)
        {
            if (id is null)
                throw new Exception("Id is Null");

            var specs = new ProductWithSpecifications(id);

            var product = await unitOfWork.ProductRepository.GetWithSpecificationByIdAsync(specs);

            var mappedProduct = mapper.Map<ProductDetailsDto>(product);

            return mappedProduct;
        }
    }
}
