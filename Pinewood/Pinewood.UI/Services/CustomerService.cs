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

        /// <summary>
        /// Retrieves a list of customers.
        /// </summary>
        /// <returns>An enumerable collection of CustomerDto.</returns>
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

        /// <summary>
        /// Retrieves a customer by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the customer.</param>
        /// <returns>A CustomerDto if found. Otherwise, null.</returns>
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

        /// <summary>
        /// Adds a new customer.
        /// </summary>
        /// <param name="customerDto">The details of the customer to add.</param>
        /// <returns>The added CustomerDto if successful. Otherwise, null.</returns>
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

        /// <summary>
        /// Updates an existing customer.
        /// </summary>
        /// <param name="customerDto">The updated details of the customer.</param>
        /// <returns>The updated CustomerDto if successful. Otherwise, null.</returns>
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

        /// <summary>
        /// Deletes a customer by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the customer to delete.</param>
        /// <returns>True if the deletion was successful. Otherwise, false.</returns>
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
