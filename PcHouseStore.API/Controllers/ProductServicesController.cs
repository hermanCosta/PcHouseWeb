using Microsoft.AspNetCore.Mvc;
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
    public async Task<ActionResult<IEnumerable<ProductService>>> GetProductServices([FromQuery] long companyId)
    {
        if (companyId <= 0)
            return BadRequest("Company ID is required");

        var productServices = await _productServiceRepository.GetProductsByCompanyAsync(companyId);
        return Ok(productServices);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductService>> GetProductService(long id)
    {
        var productService = await _productServiceRepository.GetByIdAsync(id);
        if (productService == null)
            return NotFound();

        return Ok(productService);
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<ProductService>>> SearchProductServices([FromQuery] string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return BadRequest("Search term is required");

        var productServices = await _productServiceRepository.SearchProductsAsync(searchTerm);
        return Ok(productServices);
    }

    [HttpGet("low-stock")]
    public async Task<ActionResult<IEnumerable<ProductService>>> GetLowStockProducts([FromQuery] long companyId)
    {
        if (companyId <= 0)
            return BadRequest("Company ID is required");

        var lowStockProducts = await _productServiceRepository.GetLowStockProductsAsync(companyId);
        return Ok(lowStockProducts);
    }

    [HttpGet("category/{category}")]
    public async Task<ActionResult<IEnumerable<ProductService>>> GetProductsByCategory([FromQuery] long companyId, [FromRoute] string category)
    {
        if (companyId <= 0)
            return BadRequest("Company ID is required");

        if (string.IsNullOrWhiteSpace(category))
            return BadRequest("Category is required");

        var productServices = await _productServiceRepository.GetProductsByCategoryAsync(companyId, category);
        return Ok(productServices);
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
    public async Task<ActionResult<ProductService>> CreateProductService([FromBody] ProductService productService)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Check if product already exists
        var exists = await _productServiceRepository.CheckProductExistsAsync(productService.Name, productService.CompanyId);
        if (exists)
            return BadRequest("A product with this name already exists for this company");

        try
        {
            var createdProductService = await _productServiceRepository.AddAsync(productService);
            return CreatedAtAction(nameof(GetProductService), new { id = createdProductService.ProductServiceId }, createdProductService);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while creating the product/service: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProductService(long id, [FromBody] ProductService productService)
    {
        if (id != productService.ProductServiceId)
            return BadRequest("ID mismatch");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await _productServiceRepository.UpdateAsync(productService);
            return NoContent();
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
