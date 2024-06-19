using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebProject.Date;
using MyWebProject.Intrerface;
using MyWebProject.Models;
using MyWebProject.Role;
using MyWebProject.ViewModel;
using System.Threading.Tasks;

namespace MyWebProject.Controllers
{
    public class AccountController : Controller
    {
        // Dependency Injection of the account service
        private readonly IAccountService _accountService;

        // Constructor to inject the IAccountService
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // GET: /Account/Login
        // Displays the login page
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        // Handles the login form submission
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM login)
        {
            if (ModelState.IsValid)
            {
                // Find user by email
                var user = await _accountService.FindUserByEmailAsync(login.Email);
                // Check if user exists and password is correct
                if (user != null && await _accountService.CheckPasswordAsync(user, login.Password))
                {
                    // Sign in the user
                    await _accountService.SignInAsync(user, login.RememberMe);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Invalid login attempt.");
            }
            return View(login);
        }

        // GET: /Account/Register
        // Displays the registration page
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        // Handles the registration form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM register)
        {
            if (ModelState.IsValid)
            {
                // Create a new user
                var user = new IdentityUser
                {
                    NormalizedUserName = register.Name,
                    UserName = register.Name,
                    Email = register.Email,
                    PhoneNumber = register.PhoneNumber,
                };

                // Create the user in the system
                var result = await _accountService.CreateUserAsync(user, register.Password);
                if (result.Succeeded)
                {
                    // Add the user to the default role and sign them in
                    await _accountService.AddToRoleAsync(user, ApplicationRoles.User);
                    await _accountService.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                // Add errors to the model state if registration failed
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                // Check if the email is already used by another user
                var existingUser = await _accountService.FindUserByEmailAsync(register.Email);
                if (existingUser != null && existingUser.Id != user.Id)
                {
                    ModelState.AddModelError(string.Empty, "Email already used.");
                }
            }
            return View(register);
        }

        // GET: /Account/Forgot
        // Displays the forgot password page
        public ActionResult Forgot()
        {
            return View();
        }

        // POST: /Account/Forgot
        // Handles the forgot password form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Forgot(ForgotPasswordVm model)
        {
            if (ModelState.IsValid)
            {
                // Find user by email
                var user = await _accountService.FindUserByEmailAsync(model.Email);
                if (user != null)
                {
                    // Generate a password reset token
                    var token = await _accountService.GeneratePasswordResetTokenAsync(user);
                    // Reset the password
                    var result = await _accountService.ResetPasswordAsync(user, token, model.Password);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    // Add errors to the model state if password reset failed
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User not found.");
                }
            }
            return View(model);
        }

        // POST: /Account/Logout
        // Handles the logout action
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _accountService.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/Users
        // Displays a list of users with optional filters
        public IActionResult Users(string Name, string Email, string PhoneNumber)
        {
            // Get the list of users
            List<IdentityUser> users = _accountService.GetUsersAsync().Result.ToList();

            // Filter users by name
            if (!string.IsNullOrEmpty(Name))
            {
                string normalizedName = Name.ToUpper();
                users = users.Where(p => p.NormalizedUserName.ToUpper().Contains(normalizedName)).ToList();
            }

            // Filter users by email
            if (!string.IsNullOrEmpty(Email))
            {
                string userEmail = Email.ToUpper();
                users = users.Where(p => p.NormalizedEmail.ToUpper().Contains(userEmail)).ToList();
            }

            // Filter users by phone number
            if (!string.IsNullOrEmpty(PhoneNumber))
            {
                users = users.Where(p => p.PhoneNumber == PhoneNumber).ToList();
            }

            return View(users);
        }

        // GET: /Account/MyOrders
        // Displays the current user's orders
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> MyOrders()
        {
            var currentUser = await _accountService.GetUserAsync(User);
            var orders = await _accountService.GetOrdersByUserIdAsync(currentUser.UserName);
            return View(orders);
        }

        // GET: /Account/OrderDetails
        // Displays the details of a specific order
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> OrderDetails(int id)
        {
            var currentUser = await _accountService.GetUserAsync(User);
            var order = await _accountService.GetOrderDetailsAsync(currentUser.UserName, id);
            return View(order);
        }

        // GET: /Account/Profile
        // Displays the current user's profile
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var currentUser = await _accountService.GetUserAsync(User);
            if (currentUser == null)
            {
                return NotFound();
            }

            var profile = new ProfileVM
            {
                Id = currentUser.Id,
                Name = currentUser.UserName,
                Email = currentUser.Email,
                PhoneNumber = currentUser.PhoneNumber
            };
            return View(profile);
        }

        // POST: /Account/Profile
        // Handles the profile update form submission
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _accountService.FindUserByIdAsync(model.Id);
            if (user == null)
            {
                return NotFound();
            }

            // Update the user's information
            user.UserName = model.Name;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;

            // Save the updated user information
            var result = await _accountService.UpdateUserAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Profile"); // Redirect to GET Profile action
            }

            // Add errors to the model state if update failed
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }
    }



}

