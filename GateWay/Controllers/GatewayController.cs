using Microsoft.AspNetCore.Mvc;
using SimpleBank.Gateway.Services;
using SimpleBank.Gateway.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace SimpleBank.Gateway.Controllers
{
    [ApiController]
    [Route("api/gateway")]
    public class GatewayController : ControllerBase
    {
        private readonly CustomerServiceClient _customerService;
        private readonly AccountServiceClient _accountService;
        private readonly TransactionServiceClient _transactionService;

        public GatewayController(
            CustomerServiceClient customerService,
            AccountServiceClient accountService,
            TransactionServiceClient transactionService)
        {
            _customerService = customerService;
            _accountService = accountService;
            _transactionService = transactionService;
        }

        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok("Gateway is running");
        }

        // ✅ CUSTOMER
        [Authorize]
        [HttpGet("customers")]
        public async Task<IActionResult> GetCustomers()
        {
            var result = await _customerService.GetCustomersAsync();

            /*var customers = JsonSerializer.Deserialize<List<CustomerDto>>(
                result,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return Ok(customers);
            */

            return Content(result, "application/json");
        }

        // ✅ ACCOUNT
        [Authorize]
        [HttpGet("accounts")]
        public async Task<IActionResult> GetAccounts()
        {
            var result = await _accountService.GetAccountsAsync();

            /*var accounts = JsonSerializer.Deserialize<List<object>>(
                result,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return Ok(accounts);
            */

            return Content(result, "application/json");
        }

        
[HttpPost("transactions")]
public async Task<IActionResult> CreateTransaction([FromBody] object request)
{
    var result = await _transactionService.CreateTransactionAsync(request);

    var transaction = JsonSerializer.Deserialize<object>(result,
        new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

    return Ok(transaction);
}


        // ✅ TRANSACTION
        [Authorize]
        [HttpGet("transactions")]
        public async Task<IActionResult> GetTransactions()
        {
            var result = await _transactionService.GetTransactionsAsync();

            /*var transactions = JsonSerializer.Deserialize<List<object>>(
                result,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return Ok(transactions);
            */
            return Content(result, "application/json");

        }
    }
}