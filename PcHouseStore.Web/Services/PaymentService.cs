using PcHouseStore.Domain.Models;

namespace PcHouseStore.Web.Services;

public class PaymentService
{
    private readonly ApiService _apiService;

    public PaymentService(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<IEnumerable<Payment>?> GetPaymentsByOrderAsync(long orderId)
    {
        return await _apiService.GetListAsync<Payment>($"api/payments?orderId={orderId}");
    }

    public async Task<IEnumerable<Payment>?> GetDepositsByOrderAsync(long orderId)
    {
        return await _apiService.GetListAsync<Payment>($"api/payments/deposits?orderId={orderId}");
    }

    public async Task<Payment?> CreatePaymentAsync(Payment payment)
    {
        return await _apiService.PostAsync<Payment>("api/payments", payment);
    }

    public async Task<Payment?> GetPaymentAsync(long id)
    {
        return await _apiService.GetAsync<Payment>($"api/payments/{id}");
    }
}

