using Business.Dtos.Product;
using Business.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Abstract
{
    public interface IProductService
    {
        Task<Response<List<ProductDto>>> GetAllProductsAsync();
        Task<Response<ProductDto>> GetProductAsync(int id);
        Task<Response> CreateProductAsync(ProductCreateDto model);
        Task<Response> UpdateProductAsync(int id, ProductUpdateDto model);
        Task<Response> DeleteProductAsync(int id);
    }
}
