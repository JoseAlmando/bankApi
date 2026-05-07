namespace BankApi.Application.Interfaces;

public interface IAuthService
{
    Task<(string UserId, string Email)> RegisterAsync(
        string firstName, string lastName, string email, string password);

    Task<(string UserId, string Email)?> LoginAsync(string email, string password);
}
