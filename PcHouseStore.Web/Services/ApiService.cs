using System.Text;
using System.Text.Json;

namespace PcHouseStore.Web.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<T?> GetAsync<T>(string endpoint)
    {
        try
        {
            var response = await _httpClient.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(json, _jsonOptions);
            }
            return default;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetAsync: {ex.Message}");
            return default;
        }
    }

    public async Task<IEnumerable<T>?> GetListAsync<T>(string endpoint)
    {
        try
        {
            // Debug the full URL
            var fullUrl = $"{_httpClient.BaseAddress}{endpoint}";
            Console.WriteLine($"Making API call to: {fullUrl}");

            // Debug request headers
            foreach (var header in _httpClient.DefaultRequestHeaders)
            {
                Console.WriteLine($"Request Header: {header.Key}: {string.Join(", ", header.Value)}");
            }

            var response = await _httpClient.GetAsync(endpoint);
            Console.WriteLine($"Response Status: {response.StatusCode}");
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"API Response: {json.Substring(0, Math.Min(json.Length, 500))}..."); // Truncate long responses
                
                var result = JsonSerializer.Deserialize<IEnumerable<T>>(json, _jsonOptions);
                Console.WriteLine($"Deserialized {result?.Count() ?? 0} items");
                return result;
            }
            else
            {
                Console.WriteLine($"API Error: {response.StatusCode} - {response.ReasonPhrase}");
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error Content: {errorContent}");
            }
            return new List<T>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetListAsync: {ex.Message}");
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            return new List<T>();
        }
    }

    public async Task<T?> PostAsync<T>(string endpoint, T data)
    {
        try
        {
            var json = JsonSerializer.Serialize(data, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync(endpoint, content);
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(responseJson, _jsonOptions);
            }
            return default;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in PostAsync: {ex.Message}");
            return default;
        }
    }

    public async Task<bool> PutAsync<T>(string endpoint, T data)
    {
        try
        {
            var json = JsonSerializer.Serialize(data, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PutAsync(endpoint, content);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in PutAsync: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteAsync(string endpoint)
    {
        try
        {
            var response = await _httpClient.DeleteAsync(endpoint);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in DeleteAsync: {ex.Message}");
            return false;
        }
    }
}
