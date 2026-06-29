using Microsoft.AspNetCore.Mvc;
using SimpleBank.Transactions.Api.Domain;
using SimpleBank.Transactions.Api.Models;
using SimpleBank.Transactions.Api.Persistence;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace SimpleBank.Transactions.Api.Controllers
{
    [ApiController]
    [Route("api/transactions")]
    public class TransactionsController : ControllerBase
    {
        private readonly TransactionRepository _repo;
        private readonly HttpClient _httpClient;

        public TransactionsController(TransactionRepository repo, IHttpClientFactory httpClientFactory)
        {
            _repo = repo;
            _httpClient = httpClientFactory.CreateClient();
        }

[Authorize]
[HttpGet]
public async Task<IActionResult> GetAll()
{
    var txs = await _repo.GetAllAsync();
    return Ok(txs);
}

        [HttpPost]
        //public async Task<IActionResult> Process(Transaction tx)
        public async Task<IActionResult> Process(CreateTransactionRequest request)
        {

            // because of the DTO introduces below logic added with above new parameter at the method
            // ✅ Map to entity
            var tx = new Transaction
            {
                AccountId = request.AccountId,
                Amount = request.Amount,
                Type = request.Type
            };

            // ✅ rest of your below logic remains SAME
            
            //For the Non-docker below-Normal api build before docker
            //var accountApiUrl = $"http://localhost:5172/api/accounts/{tx.AccountId}";
            //For the docker below
            var accountApiUrl = $"http://account-api/api/accounts/{tx.AccountId}";
            // ✅ STEP 1: Get account
            var accountResponse = await _httpClient.GetAsync(accountApiUrl);

            if (!accountResponse.IsSuccessStatusCode)
                return BadRequest("Account not found");

            var content = await accountResponse.Content.ReadAsStringAsync();
            var account = JsonSerializer.Deserialize<AccountDto>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // ✅ STEP 2: Business logic
            if (tx.Type == "Withdraw" && account.Balance < tx.Amount)
            {
                return BadRequest("Insufficient balance");
            }

            if (tx.Type == "Withdraw")
                account.Balance -= tx.Amount;
            else
                account.Balance += tx.Amount;

            // ✅ STEP 3: Update account via API
            var json = JsonSerializer.Serialize(account);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            //before docker compose below
            //var updateResponse = await _httpClient.PutAsync($"http://localhost:5172/api/accounts/{account.Id}", httpContent);
            //After docker compose below
            var updateResponse = await _httpClient.PutAsync($"http://account-api/api/accounts/{account.Id}", httpContent);
            
            if (!updateResponse.IsSuccessStatusCode)
                return BadRequest("Failed to update account");

            // ✅ STEP 4: Save transaction
            var created = await _repo.AddAsync(tx);

            return Ok(created);
        }
    }

    // ✅ DTO for Account
    public class AccountDto
    {
        public Guid Id { get; set; }
        public decimal Balance { get; set; }
    }
}