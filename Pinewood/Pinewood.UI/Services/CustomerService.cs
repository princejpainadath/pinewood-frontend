using Newtonsoft.Json;
using Pinewood.UI.Interfaces;
using Pinewood.UI.Models;
using System.Text;

namespace Pinewood.UI.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(HttpClient httpClient, IConfiguration configuration, ILogger<CustomerService> logger)
        {
            _logger = logger;
            _httpClient = httpClient;

            var baseUrl = configuration["ApiSettings:BaseUrl"];
            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new ArgumentNullException(nameof(baseUrl), "Base URL for the API is not configured.");
            }
            _httpClient.BaseAddress = new Uri(baseUrl);
        }

        public async Task<IEnumerable<CustomerDto>> GetCustomersAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("customer");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(content))
                {
                    return Enumerable.Empty<CustomerDto>();
                }

                var customers = JsonConvert.DeserializeObject<IEnumerable<CustomerDto>>(content);
                return customers ?? Enumerable.Empty<CustomerDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Enumerable.Empty<CustomerDto>();
            }
        }

        public async Task<CustomerDto?> GetCustomerByIdAsync(Guid id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"customer/{id}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(content))
                {
                    return null;
                }

                var customer = JsonConvert.DeserializeObject<CustomerDto>(content);
                return customer ?? null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<CustomerDto?> AddCustomerAsync(AddCustomerDto customerDto)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(customerDto), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("customer", content);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<CustomerDto>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<CustomerDto?> UpdateCustomerAsync(UpdateCustomerDto customerDto)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(customerDto), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync("customer", content);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<CustomerDto>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<bool> DeleteCustomerAsync(Guid id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"customer/{id}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(content))
                {
                    return false;
                }
                return JsonConvert.DeserializeObject<bool>(content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }
    }
}
