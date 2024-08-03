using Microsoft.AspNetCore.Mvc;
using Pinewood.UI.Interfaces;
using Pinewood.UI.Models;
using Pinewood.UI.Services;

namespace Pinewood.UI.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ICustomerService customerService, ILogger<CustomerController> logger)
        {
            _customerService = customerService;
            _logger = logger;
        }

        /// <summary>
        /// Displays the list of customers.
        /// </summary>
        /// <returns>The customer list view.</returns>
        public async Task<IActionResult> Index()
        {
            var customers = await _customerService.GetCustomersAsync();
            return View(customers);
        }

        /// <summary>
        /// Displays the details of a customer.
        /// </summary>
        /// <param name="id">The unique identifier of the customer.</param>
        /// <returns>The customer details view.</returns>
        public async Task<IActionResult> Details(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    return RedirectToAction("Index", "Customer");
                }

                var customer = await _customerService.GetCustomerByIdAsync(id);
                if (customer == null)
                {
                    return RedirectToAction("Index", "Customer");
                }
                return View(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return RedirectToAction("Index", "Customer");
            }
        }

        /// <summary>
        /// Displays the form to create a new customer.
        /// </summary>
        /// <returns>The customer creation view.</returns>
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Processes the customer creation form submission.
        /// </summary>
        /// <param name="customer">The customer data transfer object.</param>
        /// <returns>Redirects to the customer list view if successful.</returns>
        [HttpPost]
        public async Task<IActionResult> Create(AddCustomerDto customer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _customerService.AddCustomerAsync(customer);
                    return RedirectToAction("Index", "Customer");
                }
                return View(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return RedirectToAction("Index", "Customer");
            }
        }

        /// <summary>
        /// Displays the form to edit an existing customer.
        /// </summary>
        /// <param name="id">The unique identifier of the customer to edit.</param>
        /// <returns>The customer edit view.</returns>
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    return RedirectToAction("Index", "Customer");
                }

                var customer = await _customerService.GetCustomerByIdAsync(id);
                if (customer == null)
                {
                    return RedirectToAction("Index", "Customer");
                }

                var updateCustomerDto = new UpdateCustomerDto
                {
                    Id = customer.Id,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Email = customer.Email
                };

                return View(updateCustomerDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return RedirectToAction("Index", "Customer");
            }
        }

        /// <summary>
        /// Processes the customer edit form submission.
        /// </summary>
        /// <param name="customer">The customer data transfer object.</param>
        /// <returns>Redirects to the customer list view if successful</returns>
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateCustomerDto customer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _customerService.UpdateCustomerAsync(customer);
                    return RedirectToAction("Index", "Customer");
                }
                return View(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return RedirectToAction("Index", "Customer");
            }
        }

        /// <summary>
        /// Displays the confirmation view to delete a customer.
        /// </summary>
        /// <param name="id">The unique identifier of the customer to delete.</param>
        /// <returns>The customer deletion confirmation view.</returns>
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    return RedirectToAction("Index", "Customer");
                }

                var customer = await _customerService.GetCustomerByIdAsync(id);
                if (customer == null)
                {
                    return RedirectToAction("Index", "Customer");
                }

                return View(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return RedirectToAction("Index", "Customer");
            }
        }

        /// <summary>
        /// Processes the customer deletion confirmation.
        /// </summary>
        /// <param name="id">The unique identifier of the customer to delete.</param>
        /// <returns>Redirects to the customer list view.</returns>
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    return RedirectToAction("Index", "Customer");
                }

                await _customerService.DeleteCustomerAsync(id);
                return RedirectToAction("Index", "Customer");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return RedirectToAction("Index", "Customer");
            }
        }
    }
}
