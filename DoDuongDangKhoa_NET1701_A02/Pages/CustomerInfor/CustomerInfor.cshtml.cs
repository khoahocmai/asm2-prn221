using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories;

namespace DoDuongDangKhoa_NET1701_A02.Pages.CustomerInfor
{
    public class CustomerInforModel : PageModel
    {
        private readonly ICustomerRepository _customerRepo;

        public CustomerInforModel(ICustomerRepository customerRepository)
        {
            _customerRepo = customerRepository;
        }

        [BindProperty]
        public int CustomerId { get; set; }

        [BindProperty]
        public string? CustomerFullName { get; set; }

        [BindProperty]
        public string? Telephone { get; set; }

        [BindProperty]
        public string EmailAddress { get; set; } = null!;

        [BindProperty]
        public DateOnly? CustomerBirthday { get; set; }

        [BindProperty]
        public string? Password { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userIdClaim = User.FindFirst("CustomerID");
            int id = userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;

            var customer = await _customerRepo.GetCustomerById(id);

            if (customer == null)
            {
                return NotFound();
            }

            CustomerId = customer.CustomerId;
            CustomerFullName = customer.CustomerFullName;
            Telephone = customer.Telephone;
            EmailAddress = customer.EmailAddress;
            CustomerBirthday = customer.CustomerBirthday;
            Password = customer.Password;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userIdClaim = User.FindFirst("CustomerID");
            int id = userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;

            var customer = await _customerRepo.GetCustomerById(id);

            if (customer == null)
            {
                return NotFound();
            }

            customer.CustomerFullName = CustomerFullName;
            customer.Telephone = Telephone;
            customer.EmailAddress = EmailAddress;
            customer.CustomerBirthday = CustomerBirthday;
            customer.Password = Password;

            await _customerRepo.UpdateCustomer(customer);

            return RedirectToPage("/CustomerInfor/CustomerInfor");
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            var userIdClaim = User.FindFirst("CustomerID");
            int id = userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;

            var customer = await _customerRepo.GetCustomerById(id);

            if (customer == null)
            {
                return NotFound();
            }

            await _customerRepo.RemoveCustomer(customer);

            await HttpContext.SignOutAsync();

            return RedirectToPage("/Account/Login");
        }
    }
}
