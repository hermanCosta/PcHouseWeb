using PcHouseStore.Domain.Models;

namespace PcHouseStore.Web.Services;

public class FaultService
{
    private readonly ApiService _apiService;

    public FaultService(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<IEnumerable<Fault>?> GetFaultsAsync()
    {
        return await _apiService.GetListAsync<Fault>("api/faults");
    }

    public async Task<Fault?> GetFaultAsync(long id)
    {
        return await _apiService.GetAsync<Fault>($"api/faults/{id}");
    }

    public async Task<IEnumerable<Fault>?> SearchFaultsAsync(string searchTerm)
    {
        return await _apiService.GetListAsync<Fault>($"api/faults/search?searchTerm={Uri.EscapeDataString(searchTerm)}");
    }

    public async Task<Fault?> CreateFaultAsync(Fault fault)
    {
        return await _apiService.PostAsync<Fault>("api/faults", fault);
    }
}

