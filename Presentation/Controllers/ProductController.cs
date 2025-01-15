using Business.Dtos.Product;
using Business.Services.Abstract;
using Business.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        #region Documentation
        /// <summary>
        /// Products List 
        /// </summary>
        [ProducesResponseType(typeof(Response<List<ProductDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        #endregion 
        [HttpGet]
        public async Task<Response<List<ProductDto>>> GetAllProductsAsync()
        => await _productService.GetAllProductsAsync();

        #region Documentation
        /// <summary>
        /// Get product by Id 
        /// </summary>
        /// <param name="id"></param>
        [ProducesResponseType(typeof(Response<ProductDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        #endregion
        [HttpGet("{id}")]
        public async Task<Response<ProductDto>> GetResponseAsync(int id)
        => await _productService.GetProductAsync(id);

        #region Documentation
        /// <summary>
        /// For product creating
        /// </summary>
        /// <remarks>
        /// <ul>
        /// <li><b>Type:</b><p>0 - New, 1 - Sold</p></li>>
        /// </ul>  
        /// </remarks>
        /// <param name="model"></param>
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        #endregion
        [HttpPost]
        public async Task<Response> CreateProductAsync(ProductCreateDto model)
        => await _productService.CreateProductAsync(model);

        #region Documentation
        /// <summary>
        /// Product Updating
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        #endregion
        [HttpPut("{id}")]
        public async Task<Response> UpdateProductAsync(int id, ProductUpdateDto model)
        => await _productService.UpdateProductAsync(id, model);

        #region Documentation
        /// <summary>
        /// Product deleting
        /// </summary>
        /// <param name="id"></param>
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        #endregion
        [HttpDelete]
        public async Task<Response> DeleteProductAsync(int id)
        => await _productService.DeleteProductAsync(id);
    }
}
