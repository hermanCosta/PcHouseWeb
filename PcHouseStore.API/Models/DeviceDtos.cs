using PcHouseStore.Domain.Models;

namespace PcHouseStore.API.Models;

public record DeviceResponse(
    long DeviceId,
    long CustomerId,
    string Brand,
    string Model,
    string? SerialNumber,
    string? Description,
    string DisplayName);

public record CreateDeviceRequest(
    long CustomerId,
    string Brand,
    string Model,
    string? SerialNumber,
    string? Description);

public record UpdateDeviceRequest(
    string Brand,
    string Model,
    string? SerialNumber,
    string? Description);

public static class DeviceMapper
{
    public static DeviceResponse ToResponse(Device device)
    {
        return new DeviceResponse(
            device.DeviceId,
            device.CustomerId,
            device.Brand,
            device.Model,
            device.SerialNumber,
            device.Description,
            device.DisplayName);
    }
}
