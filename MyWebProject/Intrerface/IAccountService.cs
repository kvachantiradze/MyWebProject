using Microsoft.AspNetCore.Identity;
using MyWebProject.Models;
using System.Security.Claims;

namespace MyWebProject.Intrerface
{
   
    public interface IAccountService
    {
        // Method to find a user by email address
        Task<IdentityUser> FindUserByEmailAsync(string email);


        // Method to check if a user's password is correct
        Task<bool> CheckPasswordAsync(IdentityUser user, string password);


        // Method to sign in a user
        Task SignInAsync(IdentityUser user, bool isPersistent);


        // Method to create a new user
        Task<IdentityResult> CreateUserAsync(IdentityUser user, string password);


        // Method to add a user to a role
        Task AddToRoleAsync(IdentityUser user, string role);


        // Method to sign out the current user
        Task SignOutAsync();


        // Method to get the current user based on the claims principal
        Task<IdentityUser> GetUserAsync(ClaimsPrincipal principal);


        // Method to find a user by ID
        Task<IdentityUser> FindUserByIdAsync(string userId);


        // Method to update a user's information
        Task<IdentityResult> UpdateUserAsync(IdentityUser user);


        // Method to get a list of all users
        Task<IList<IdentityUser>> GetUsersAsync();


        // Method to get a list of orders for a specific user
        Task<IList<Order>> GetOrdersByUserIdAsync(string userId);


        // Method to get the details of a specific order for a user
        Task<Order> GetOrderDetailsAsync(string userId, int orderId);


        // Method to generate a password reset token for a user
        Task<string> GeneratePasswordResetTokenAsync(IdentityUser user);


        // Method to reset a user's password
        Task<IdentityResult> ResetPasswordAsync(IdentityUser user, string token, string newPassword);
    }



}
