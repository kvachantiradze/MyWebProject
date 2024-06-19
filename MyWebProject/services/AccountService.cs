using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyWebProject.Date;
using MyWebProject.Intrerface;
using MyWebProject.Models;
using System.Security.Claims;

namespace MyWebProject.services
{
    public class AccountService : IAccountService
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppDbContext _context;

        public AccountService(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, AppDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }

        // Finds a user by their email asynchronously
        public async Task<IdentityUser> FindUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        // Checks if the provided password matches the user's password asynchronously
        public async Task<bool> CheckPasswordAsync(IdentityUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        // Signs in a user asynchronously
        public async Task SignInAsync(IdentityUser user, bool isPersistent)
        {
            await _signInManager.SignInAsync(user, isPersistent);
        }

        // Creates a new user with the provided password asynchronously
        public async Task<IdentityResult> CreateUserAsync(IdentityUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        // Adds a user to a specified role asynchronously
        public async Task AddToRoleAsync(IdentityUser user, string role)
        {
            await _userManager.AddToRoleAsync(user, role);
        }

        // Signs out the current user asynchronously
        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        // Retrieves the user associated with the specified claims principal asynchronously
        public async Task<IdentityUser> GetUserAsync(ClaimsPrincipal principal)
        {
            return await _userManager.GetUserAsync(principal);
        }

        // Finds a user by their ID asynchronously
        public async Task<IdentityUser> FindUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        // Updates the specified user asynchronously
        public async Task<IdentityResult> UpdateUserAsync(IdentityUser user)
        {
            return await _userManager.UpdateAsync(user);
        }

        // Retrieves all users asynchronously
        public async Task<IList<IdentityUser>> GetUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        // Retrieves orders associated with a user by their ID asynchronously
        public async Task<IList<Order>> GetOrdersByUserIdAsync(string userId)
        {
            return await _context.Orders.Where(o => o.UserId == userId).ToListAsync();
        }

        // Retrieves details of a specific order associated with a user asynchronously
        public async Task<Order> GetOrderDetailsAsync(string userId, int orderId)
        {
            return await _context.Orders
                .Include(o => o.Items)
                .ThenInclude(item => item.Product)
                .FirstOrDefaultAsync(o => o.UserId == userId && o.OrderId == orderId);
        }

        // Generates a password reset token for the specified user asynchronously
        public async Task<string> GeneratePasswordResetTokenAsync(IdentityUser user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        // Resets the password for the specified user asynchronously using a token
        public async Task<IdentityResult> ResetPasswordAsync(IdentityUser user, string token, string newPassword)
        {
            return await _userManager.ResetPasswordAsync(user, token, newPassword);
        }
    }

}
