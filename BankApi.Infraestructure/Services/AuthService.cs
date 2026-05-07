using BankApi.Application.Interfaces;
using BankApi.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace BankApi.Infrastructure.Services;

public class AuthService(
    UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager) : IAuthService
{
    public async Task<(string UserId, string Email)> RegisterAsync(
        string firstName, string lastName, string email, string password)
    {
        var user = new AppUser
        {
            UserName = email,
            Email = email,
            FirstName = firstName,
            LastName = lastName
        };

        var result = await userManager.CreateAsync(user, password);
        if (!result.Succeeded)
            throw new ArgumentException(string.Join(", ", result.Errors.Select(e => e.Description)));

        return (user.Id, user.Email!);
    }

    public async Task<(string UserId, string Email)?> LoginAsync(string email, string password)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null) return null;

        var result = await signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: false);
        if (!result.Succeeded) return null;

        return (user.Id, user.Email!);
    }
}
