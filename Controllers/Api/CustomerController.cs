using Microsoft.AspNetCore.Mvc;
using API_CORE.Models;
using API_CORE.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
namespace API.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly  ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
    [HttpGet("GetInfor")]
    [Authorize] // Yêu cầu xác thực
    public IActionResult SomeSecuredEndpoint()
    {
        // Đây là một endpoint yêu cầu xác thực. 
        // Bạn có thể truy cập thông tin đăng nhập qua HttpContext.User

        var userName = HttpContext.User.Identity.Name;
        var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // Các logic xử lý khác

        return Ok($"Hello, authenticated user {userName} (ID: {userId})!");
    }
        // GET: api/ApiCustomer
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> GetCustomers()
        {
            if(User.HasClaim("Permission", "Get")) 
            {
                var customer = await _customerService.GetAll();
                return Ok(customer);
            }
         return Forbid();
        }

        // GET: api/ApiCustomer/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomerById(int id)
        {
          
            return Ok(await _customerService.GetById(id));
        }

        // PUT: api/ApiCustomer/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {
            

            return Ok(await _customerService.Update(id,customer));
        }

        // POST: api/ApiCustomer
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer([Bind("CustomerId,FirstName,LastName,Phone,Email,Street,City,State,ZipCode")] Customer customer)
        {
          

            return Ok(await _customerService.Insert(customer));
        }

        // DELETE: api/ApiCustomer/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
           

            return Ok(await _customerService.Delete(id));
        }
    }
}
