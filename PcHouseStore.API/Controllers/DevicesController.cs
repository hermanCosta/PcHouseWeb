using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PcHouseStore.API.Models;
using PcHouseStore.Domain.Models;
using PcHouseStore.Infrastructure.Data;
using PcHouseStore.Infrastructure.Repositories;

namespace PcHouseStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DevicesController : ControllerBase
{
    private readonly IRepository<Device> _deviceRepository;
    private readonly PcHouseStoreDbContext _context;

    public DevicesController(IRepository<Device> deviceRepository, PcHouseStoreDbContext context)
    {
        _deviceRepository = deviceRepository;
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DeviceResponse>>> GetDevices([FromQuery] long customerId)
    {
        if (customerId <= 0)
            return BadRequest("Customer ID is required");

        var devices = await _context.Devices
            .Where(d => d.CustomerId == customerId)
            .ToListAsync();
        return Ok(devices.Select(d => DeviceMapper.ToResponse(d)));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DeviceResponse>> GetDevice(long id)
    {
        var device = await _deviceRepository.GetByIdAsync(id);
        if (device == null)
            return NotFound();

        return Ok(DeviceMapper.ToResponse(device));
    }

    [HttpPost]
    public async Task<ActionResult<DeviceResponse>> CreateDevice([FromBody] CreateDeviceRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var device = new Device
            {
                CustomerId = request.CustomerId,
                Brand = request.Brand,
                Model = request.Model,
                SerialNumber = request.SerialNumber,
                Description = request.Description
            };

            var createdDevice = await _deviceRepository.AddAsync(device);
            return CreatedAtAction(nameof(GetDevice), new { id = createdDevice.DeviceId }, 
                DeviceMapper.ToResponse(createdDevice));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while creating the device: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<DeviceResponse>> UpdateDevice(long id, [FromBody] UpdateDeviceRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var device = await _deviceRepository.GetByIdAsync(id);
            if (device == null)
                return NotFound();

            if (!string.IsNullOrEmpty(request.Brand))
                device.Brand = request.Brand;
            if (!string.IsNullOrEmpty(request.Model))
                device.Model = request.Model;
            device.SerialNumber = request.SerialNumber;
            device.Description = request.Description;

            await _deviceRepository.UpdateAsync(device);
            return Ok(DeviceMapper.ToResponse(device));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while updating the device: {ex.Message}");
        }
    }
}

