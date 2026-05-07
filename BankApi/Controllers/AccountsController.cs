using BankApi.Application.DTOs.Accounts;
using BankApi.Application.DTOs.Transactions;
using BankApi.Application.UseCases.Accounts;
using BankApi.Application.UseCases.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankApi.Api.Controllers;

[ApiController]
[Route("api/accounts")]
[Authorize]
public class AccountsController(
    CreateAccount createAccount,
    GetAccountById getAccountById,
    GetMyAccounts getMyAccounts,
    Deposit deposit,
    Withdraw withdraw,
    GetTransactionsByAccount getTransactionsByAccount) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAccountRequest request, CancellationToken ct)
    {
        var result = await createAccount.ExecuteAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await getMyAccounts.ExecuteAsync(ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await getAccountById.ExecuteAsync(id, ct);
        return Ok(result);
    }

    [HttpPost("{id:guid}/deposit")]
    public async Task<IActionResult> Deposit(Guid id, [FromBody] AmountRequest request, CancellationToken ct)
    {
        var result = await deposit.ExecuteAsync(id, request.Amount, ct);
        return Ok(result);
    }

    [HttpPost("{id:guid}/withdraw")]
    public async Task<IActionResult> Withdraw(Guid id, [FromBody] AmountRequest request, CancellationToken ct)
    {
        var result = await withdraw.ExecuteAsync(id, request.Amount, ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}/transactions")]
    public async Task<IActionResult> GetTransactions(Guid id, CancellationToken ct)
    {
        var result = await getTransactionsByAccount.ExecuteAsync(id, ct);
        return Ok(result);
    }
}
