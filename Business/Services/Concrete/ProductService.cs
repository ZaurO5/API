using AutoMapper;
using Business.Dtos.Product;
using Business.Services.Abstract;
using Business.Validators.Product;
using Business.Wrappers;
using Common.Entities;
using Common.Exceptions;
using Data.Repositories.Abstract;
using Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Concrete
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork,
            IProductRepository productRepository,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<Response<List<ProductDto>>> GetAllProductsAsync()
        {
            return new Response<List<ProductDto>>()
            {
                Data = _mapper.Map<List<ProductDto>>(await _productRepository.GetAllAsync())
            };
        }

        public async Task<Response<ProductDto>> GetProductAsync(int id)
        {
            var product = await _productRepository.GetAsync(id);
            if (product is null)
                throw new NotFoundException("Product is not found");

            return new Response<ProductDto>
            {
                Data = _mapper.Map<ProductDto>(product)
            };
        }

        public async Task<Response> CreateProductAsync(ProductCreateDto model)
        {
            var result = await new ProductCreateDtoValidator().ValidateAsync(model);
            if (!result.IsValid)
                throw new ValidationException(result.Errors );

            var product = await _productRepository.GetByNameAsync(model.Name);
            if (product is not null)
                throw new ValidationException("The product with this name is already exist");

            product = _mapper.Map<Product>(model);
   
            await _productRepository.CreateAsync(product);
            await _unitOfWork.CommitAsync();

            return new Response
            {
                Message = "Product created successfully"
            };
        }

        public async Task<Response> UpdateProductAsync(int id, ProductUpdateDto model)
        {
            var product = await _productRepository.GetAsync(id);
            if (product is null)
                throw new NotFoundException("Product not found");

            var result = await new ProductUpdateDtoValidator().ValidateAsync(model);
            if (!result.IsValid)
                throw new ValidationException(result.Errors );

            _mapper.Map(model, product);

            _productRepository.Update(product);
            await _unitOfWork.CommitAsync();

            return new Response
            {
                Message = "Product updated successfully"
            };
        }

        public async Task<Response> DeleteProductAsync(int id)
        {
            var product = await _productRepository.GetAsync(id);
            if (product is null)
                throw new NotFoundException("Product is not found");

            _productRepository.Delete(product);
            await _unitOfWork.CommitAsync();

            return new Response()
            {
                Message = "Product deleted successfully"
            };
        }
    }
}
