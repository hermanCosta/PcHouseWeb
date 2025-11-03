using PcHouseStore.Domain.Models;

namespace PcHouseStore.Web.Services;

public class CashMovementService
{
    private readonly ApiService _apiService;

    public CashMovementService(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<CashMovement?> CreateCashMovementAsync(CashMovement cashMovement)
    {
        return await _apiService.PostAsync<CashMovement>("api/cashmovements", cashMovement);
    }

    public async Task<IEnumerable<CashMovement>?> GetCashMovementsByPaymentAsync(long paymentId)
    {
        return await _apiService.GetListAsync<CashMovement>($"api/cashmovements?paymentId={paymentId}");
    }
}

