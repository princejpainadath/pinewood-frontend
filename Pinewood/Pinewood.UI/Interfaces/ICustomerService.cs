using Pinewood.UI.Models;

namespace Pinewood.UI.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerDto>> GetCustomersAsync();
        Task<CustomerDto?> GetCustomerByIdAsync(Guid id);
        Task<CustomerDto?> AddCustomerAsync(AddCustomerDto customerDto);
        Task<CustomerDto?> UpdateCustomerAsync(UpdateCustomerDto customerDto);
        Task<bool> DeleteCustomerAsync(Guid id);
    }
}
