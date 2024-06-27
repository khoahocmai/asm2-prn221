using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories;
using System.ComponentModel.DataAnnotations;

namespace DoDuongDangKhoa_NET1701_A02.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly ICustomerRepository _customerRepository;

        public RegisterModel(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [BindProperty]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = null!;

        [BindProperty]
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; } = null!;

        [BindProperty]
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [BindProperty]
        [Required(ErrorMessage = "Telephone is required")]
        [Phone(ErrorMessage = "Invalid telephone format")]
        public string Telephone { get; set; } = null!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var customer = new Customer
            {
                EmailAddress = Email,
                CustomerFullName = Username,
                Password = Password,
                Telephone = Telephone,
                CustomerStatus = 1 // Set default status
            };

            await _customerRepository.Register(customer);

            return RedirectToPage("./RegisterSuccess");
        }
    }
}
