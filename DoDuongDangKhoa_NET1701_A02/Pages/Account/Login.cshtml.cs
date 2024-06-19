using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using DataAccessObjects;
using Repositories;

namespace DoDuongDangKhoa_NET1701_A02.Pages.Account
{
    public class LoginModel : PageModel
    {
        //private readonly FuminiHotelManagementContext _context;
        private readonly ICustomerRepository _customerRepository;

        public LoginModel(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        [Required]
        public string Email { get; set; }

        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string ErrorMessage { get; set; } = string.Empty;
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //var user = await _context.Customers
            //    .FirstOrDefaultAsync(u => u.EmailAddress == Email && u.Password == Password);
            var user = await _customerRepository.CheckLogin(Email, Password);


            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.EmailAddress),
                    new Claim("CustomerID", user.CustomerId.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    // Optional settings
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                return RedirectToPage("/Index");
            }
            else
            {
                ErrorMessage = "Not found any user";
                ModelState.AddModelError(string.Empty, ErrorMessage);
                return Page();
            }
        }
    }
}
