using PcHouseStore.Domain.Enums;
using PcHouseStore.Web.Models;

namespace PcHouseStore.Web.Services;

public class OrderService
{
    private readonly ApiService _apiService;

    public OrderService(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<IEnumerable<OrderResponse>?> GetOrdersAsync(long companyId, OrderType? orderType = null)
    {
        var endpoint = orderType.HasValue
            ? $"api/orders?companyId={companyId}&orderType={orderType.Value}"
            : $"api/orders?companyId={companyId}";
        return await _apiService.GetListAsync<OrderResponse>(endpoint);
    }

    public async Task<OrderResponse?> GetOrderAsync(long orderId)
    {
        return await _apiService.GetAsync<OrderResponse>($"api/orders/{orderId}");
    }

    public async Task<IEnumerable<OrderResponse>?> SearchOrdersAsync(long companyId, string searchTerm, OrderType? orderType = null)
    {
        var endpoint = orderType.HasValue
            ? $"api/orders/search?companyId={companyId}&orderType={orderType.Value}&searchTerm={Uri.EscapeDataString(searchTerm)}"
            : $"api/orders/search?companyId={companyId}&searchTerm={Uri.EscapeDataString(searchTerm)}";
        return await _apiService.GetListAsync<OrderResponse>(endpoint);
    }

    public async Task<IEnumerable<OrderResponse>?> GetOrdersByCustomerAsync(long customerId, OrderType? orderType = null)
    {
        var endpoint = orderType.HasValue
            ? $"api/orders/customer/{customerId}?orderType={orderType.Value}"
            : $"api/orders/customer/{customerId}";
        return await _apiService.GetListAsync<OrderResponse>(endpoint);
    }

    public async Task<IEnumerable<OrderResponse>?> GetOrdersByDateRangeAsync(long companyId, DateTime startDate, DateTime endDate, OrderType? orderType = null)
    {
        var baseEndpoint = $"api/orders/date-range?companyId={companyId}&startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}";
        var endpoint = orderType.HasValue ? $"{baseEndpoint}&orderType={orderType.Value}" : baseEndpoint;
        return await _apiService.GetListAsync<OrderResponse>(endpoint);
    }

    public async Task<OrderResponse?> CreateOrderAsync(CreateOrderRequest request)
    {
        return await _apiService.PostAsync<CreateOrderRequest, OrderResponse>("api/orders", request);
    }

    public async Task<bool> UpdateOrderAsync(long orderId, UpdateOrderRequest request)
    {
        return await _apiService.PutAsync("api/orders/" + orderId, request);
    }

    public async Task<bool> DeleteOrderAsync(long orderId)
    {
        return await _apiService.DeleteAsync($"api/orders/{orderId}");
    }
}
