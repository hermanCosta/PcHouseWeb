using Microsoft.AspNetCore.Mvc;
using PcHouseStore.API.Models;
using PcHouseStore.Domain.Models;
using PcHouseStore.Infrastructure.Repositories;

namespace PcHouseStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductServicesController : ControllerBase
{
    private readonly IProductServiceRepository _productServiceRepository;

    public ProductServicesController(IProductServiceRepository productServiceRepository)
    {
        _productServiceRepository = productServiceRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductServiceResponse>>> GetProductServices([FromQuery] long companyId)
    {
        if (companyId <= 0)
            return BadRequest("Company ID is required");

        var productServices = await _productServiceRepository.GetProductsByCompanyAsync(companyId);
        return Ok(productServices.Select(ps => ProductServiceMapper.ToResponse(ps)));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductServiceResponse>> GetProductService(long id)
    {
        var productService = await _productServiceRepository.GetByIdAsync(id);
        if (productService == null)
            return NotFound();

        return Ok(ProductServiceMapper.ToResponse(productService));
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<ProductServiceResponse>>> SearchProductServices([FromQuery] string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return BadRequest("Search term is required");

        var productServices = await _productServiceRepository.SearchProductsAsync(searchTerm);
        return Ok(productServices.Select(ps => ProductServiceMapper.ToResponse(ps)));
    }

    [HttpGet("low-stock")]
    public async Task<ActionResult<IEnumerable<ProductServiceResponse>>> GetLowStockProducts([FromQuery] long companyId)
    {
        if (companyId <= 0)
            return BadRequest("Company ID is required");

        var lowStockProducts = await _productServiceRepository.GetLowStockProductsAsync(companyId);
        return Ok(lowStockProducts.Select(ps => ProductServiceMapper.ToResponse(ps)));
    }

    [HttpGet("category/{category}")]
    public async Task<ActionResult<IEnumerable<ProductServiceResponse>>> GetProductsByCategory([FromQuery] long companyId, [FromRoute] string category)
    {
        if (companyId <= 0)
            return BadRequest("Company ID is required");

        if (string.IsNullOrWhiteSpace(category))
            return BadRequest("Category is required");

        var productServices = await _productServiceRepository.GetProductsByCategoryAsync(companyId, category);
        return Ok(productServices.Select(ps => ProductServiceMapper.ToResponse(ps)));
    }

    [HttpGet("check-exists")]
    public async Task<ActionResult<bool>> CheckProductExists([FromQuery] string productName, [FromQuery] long companyId)
    {
        if (string.IsNullOrWhiteSpace(productName))
            return BadRequest("Product name is required");

        if (companyId <= 0)
            return BadRequest("Company ID is required");

        var exists = await _productServiceRepository.CheckProductExistsAsync(productName, companyId);
        return Ok(exists);
    }

    [HttpPost]
    public async Task<ActionResult<ProductServiceResponse>> CreateProductService([FromBody] CreateProductServiceRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Check if product already exists
        var exists = await _productServiceRepository.CheckProductExistsAsync(request.Name, request.CompanyId);
        if (exists)
            return BadRequest("A product with this name already exists for this company");

        try
        {
            var productService = new ProductService
            {
                CompanyId = request.CompanyId,
                Name = request.Name,
                Category = request.Category,
                Price = request.Price,
                Quantity = request.Quantity,
                MinQuantity = request.MinQuantity,
                Note = request.Note,
                CreatedAt = DateTime.UtcNow
            };

            var createdProductService = await _productServiceRepository.AddAsync(productService);
            return CreatedAtAction(nameof(GetProductService), new { id = createdProductService.ProductServiceId }, 
                ProductServiceMapper.ToResponse(createdProductService));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while creating the product/service: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ProductServiceResponse>> UpdateProductService(long id, [FromBody] UpdateProductServiceRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var productService = await _productServiceRepository.GetByIdAsync(id);
            if (productService == null)
                return NotFound();

            if (!string.IsNullOrEmpty(request.Name))
                productService.Name = request.Name;
            productService.Category = request.Category;
            if (request.Price.HasValue)
                productService.Price = request.Price.Value;
            if (request.Quantity.HasValue)
                productService.Quantity = request.Quantity.Value;
            if (request.MinQuantity.HasValue)
                productService.MinQuantity = request.MinQuantity.Value;
            productService.Note = request.Note;
            productService.UpdatedAt = DateTime.UtcNow;

            await _productServiceRepository.UpdateAsync(productService);
            return Ok(ProductServiceMapper.ToResponse(productService));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while updating the product/service: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProductService(long id)
    {
        try
        {
            await _productServiceRepository.DeleteByIdAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while deleting the product/service: {ex.Message}");
        }
    }
}
