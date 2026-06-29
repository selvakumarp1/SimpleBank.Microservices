using Microsoft.AspNetCore.Mvc;
using SimpleBank.Customers.Api.Domain;
using SimpleBank.Customers.Api.Persistence;

namespace SimpleBank.Customers.Api.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public class CustomersController : ControllerBase
    {
        private readonly CustomerRepository _repo;

        public CustomersController(CustomerRepository repo)
        {
            _repo = repo;
        }
        /* //for Mock Test Only
        [HttpGet]
        public IActionResult GetAll()
        {
            // MOCK DATA (temporary for cloud test)
            return Ok(new[]
            {
                new { id = 1, fullName = "Selva Cloud User" },
                new { id = 2, fullName = "AWS Test User" }
            });
        }
        */

         //Later enable it after Mock Data Method tested (above)..
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var customers = await _repo.GetAllAsync();
            return Ok(customers);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Customer customer)
        {
            var created = await _repo.AddAsync(customer);
            return Ok(created);
        }

        /* [HttpGet]
         public IActionResult GetAll() => Ok(_repo.GetAll());

         [HttpPost]
         public IActionResult Create(Customer customer)
         {
             var created = _repo.Add(customer);
             return Ok(created);
         }*/

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var customer = await _repo.GetByIdAsync(id);

            if (customer == null)
                return NotFound();

            return Ok(customer);
        }
    }
}
