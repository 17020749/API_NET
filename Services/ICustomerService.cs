using API_CORE.Models;
using Microsoft.AspNetCore.Mvc;
namespace API_CORE.Services
{
    public interface ICustomerService
    {
        Task<List<Customer>> GetAll();
        Task<object> GetById(int id);
        Task<Customer> Update(int id, Customer _customer);
        Task<Customer> Insert([Bind("CustomerId,FirstName,LastName,Phone,Email,Street,City,State,ZipCode")] Customer _customer);
        Task<Customer> Delete(int id);
    }
}