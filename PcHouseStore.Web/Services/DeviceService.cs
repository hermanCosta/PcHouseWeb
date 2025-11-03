using PcHouseStore.Domain.Models;

namespace PcHouseStore.Web.Services;

public class DeviceService
{
    private readonly ApiService _apiService;

    public DeviceService(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<IEnumerable<Device>?> GetDevicesByCustomerAsync(long customerId)
    {
        return await _apiService.GetListAsync<Device>($"api/devices?customerId={customerId}");
    }

    public async Task<Device?> GetDeviceAsync(long id)
    {
        return await _apiService.GetAsync<Device>($"api/devices/{id}");
    }

    public async Task<Device?> CreateDeviceAsync(Device device)
    {
        return await _apiService.PostAsync<Device>("api/devices", device);
    }

    public async Task<bool> UpdateDeviceAsync(Device device)
    {
        return await _apiService.PutAsync<Device>($"api/devices/{device.DeviceId}", device);
    }
}

