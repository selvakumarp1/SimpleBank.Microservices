using Microsoft.AspNetCore.Mvc;
using SimpleBank.Accounts.Api.Domain;
using SimpleBank.Accounts.Api.Persistence;
using System.Net.Http;
using System.Text.Json;


namespace SimpleBank.Accounts.Api.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    public class AccountsController : ControllerBase
    {
        private readonly AccountRepository _repo;
        private readonly HttpClient _httpClient;

        public AccountsController(AccountRepository repo, IHttpClientFactory httpClientFactory)
        {
            _repo = repo;
            _httpClient = httpClientFactory.CreateClient();
        }

        // ✅ GET ALL ACCOUNTS
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var accounts = await _repo.GetAllAsync();
            return Ok(accounts);
        }

        // ✅ CREATE ACCOUNT (WITH CUSTOMER VALIDATION)
        [HttpPost]
        public async Task<IActionResult> Create(Account account)
        {
            // ✅ STEP 1: Validate Customer exists (call Customer API)
            var customerApiUrl = $"http://localhost:5074/api/customers/{account.CustomerId}";
            var response = await _httpClient.GetAsync(customerApiUrl);

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("Customer does not exist");
            }

            // ✅ STEP 2: Save account
            var created = await _repo.AddAsync(account);

            return Ok(created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, Account account)
        {
            var existing = await _repo.GetByIdAsync(id);

            if (existing == null)
                return NotFound();

            existing.Balance = account.Balance;

            await _repo.UpdateAsync(existing);

            return Ok(existing);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var account = await _repo.GetByIdAsync(id);

            if (account == null)
                return NotFound();

            return Ok(account);
        }
    }
}