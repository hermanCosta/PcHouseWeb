using Microsoft.AspNetCore.Mvc;
using PcHouseStore.Domain.Models;
using PcHouseStore.Infrastructure.Repositories;

namespace PcHouseStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{
    private readonly ISaleRepository _saleRepository;

    public SalesController(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Sale>>> GetSales([FromQuery] long companyId)
    {
        if (companyId <= 0)
            return BadRequest("Company ID is required");

        var sales = await _saleRepository.GetSalesByCompanyAsync(companyId);
        return Ok(sales);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Sale>> GetSale(long id)
    {
        var sale = await _saleRepository.GetSaleWithDetailsAsync(id);
        if (sale == null)
            return NotFound();

        return Ok(sale);
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Sale>>> SearchSales([FromQuery] long companyId, [FromQuery] string searchTerm)
    {
        if (companyId <= 0)
            return BadRequest("Company ID is required");

        if (string.IsNullOrWhiteSpace(searchTerm))
            return BadRequest("Search term is required");

        var sales = await _saleRepository.SearchSalesAsync(companyId, searchTerm);
        return Ok(sales);
    }

    [HttpGet("customer/{customerId}")]
    public async Task<ActionResult<IEnumerable<Sale>>> GetSalesByCustomer(long customerId)
    {
        if (customerId <= 0)
            return BadRequest("Customer ID is required");

        var sales = await _saleRepository.GetSalesByCustomerAsync(customerId);
        return Ok(sales);
    }

    [HttpGet("employee/{employeeId}")]
    public async Task<ActionResult<IEnumerable<Sale>>> GetSalesByEmployee(long employeeId)
    {
        if (employeeId <= 0)
            return BadRequest("Employee ID is required");

        var sales = await _saleRepository.GetSalesByEmployeeAsync(employeeId);
        return Ok(sales);
    }

    [HttpGet("date-range")]
    public async Task<ActionResult<IEnumerable<Sale>>> GetSalesByDateRange(
        [FromQuery] long companyId,
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        if (companyId <= 0)
            return BadRequest("Company ID is required");

        if (startDate > endDate)
            return BadRequest("Start date cannot be greater than end date");

        var sales = await _saleRepository.GetSalesByDateRangeAsync(companyId, startDate, endDate);
        return Ok(sales);
    }

    [HttpPost]
    public async Task<ActionResult<Sale>> CreateSale([FromBody] Sale sale)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var createdSale = await _saleRepository.AddAsync(sale);
            return CreatedAtAction(nameof(GetSale), new { id = createdSale.SaleId }, createdSale);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while creating the sale: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSale(long id, [FromBody] Sale sale)
    {
        if (id != sale.SaleId)
            return BadRequest("ID mismatch");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await _saleRepository.UpdateAsync(sale);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while updating the sale: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSale(long id)
    {
        try
        {
            await _saleRepository.DeleteByIdAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while deleting the sale: {ex.Message}");
        }
    }
}
