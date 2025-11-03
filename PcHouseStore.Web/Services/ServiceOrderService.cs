using PcHouseStore.Domain.Enums;
using PcHouseStore.Web.Models;

namespace PcHouseStore.Web.Services;

public class ServiceOrderService
{
    private readonly ApiService _apiService;

    public ServiceOrderService(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<IEnumerable<OrderResponse>?> GetServiceOrdersAsync(long companyId, OrderStatus? status = null)
    {
        var endpoint = status.HasValue
            ? $"api/serviceorders?companyId={companyId}&status={status.Value}"
            : $"api/serviceorders?companyId={companyId}";
        return await _apiService.GetListAsync<OrderResponse>(endpoint);
    }

    public async Task<OrderResponse?> GetServiceOrderAsync(long id)
    {
        return await _apiService.GetAsync<OrderResponse>($"api/serviceorders/{id}");
    }

    public async Task<IEnumerable<OrderResponse>?> SearchServiceOrdersAsync(long companyId, string searchTerm)
    {
        return await _apiService.GetListAsync<OrderResponse>($"api/serviceorders/search?companyId={companyId}&searchTerm={Uri.EscapeDataString(searchTerm)}");
    }

    public async Task<IEnumerable<OrderResponse>?> GetServiceOrdersByCustomerAsync(long customerId)
    {
        return await _apiService.GetListAsync<OrderResponse>($"api/serviceorders/customer/{customerId}");
    }

    public async Task<IEnumerable<OrderResponse>?> GetServiceOrdersByTechnicianAsync(long technicianId)
    {
        return await _apiService.GetListAsync<OrderResponse>($"api/serviceorders/technician/{technicianId}");
    }

    public async Task<OrderResponse?> CreateServiceOrderAsync(CreateOrderRequest request)
    {
        request.OrderType = OrderType.Service.ToString();
        return await _apiService.PostAsync<CreateOrderRequest, OrderResponse>("api/serviceorders", request);
    }

    public async Task<bool> UpdateServiceOrderAsync(long orderId, UpdateOrderRequest request)
    {
        return await _apiService.PutAsync($"api/serviceorders/{orderId}", request);
    }

    public async Task<bool> DeleteServiceOrderAsync(long id)
    {
        return await _apiService.DeleteAsync($"api/serviceorders/{id}");
    }

    public async Task<bool> AddNoteAsync(long orderId, string note)
    {
        var request = new { Note = note };
        var result = await _apiService.PostAsync<object, object>($"api/serviceorders/{orderId}/notes", request);
        return result != null;
    }

    public async Task<bool> UpdateStatusAsync(long orderId, OrderStatus status, string? comment = null)
    {
        var request = new UpdateOrderRequest 
        { 
            Status = status,
            Notes = comment // The API uses Notes field for status change comments
        };
        return await _apiService.PutAsync($"api/serviceorders/{orderId}", request);
    }
}
