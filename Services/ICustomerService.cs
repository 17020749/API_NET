using API_CORE.Models;
using Microsoft.AspNetCore.Mvc;
namespace API_CORE.Services
{
    public interface ICustomerService
    {
        Task<List<Customer>> GetAll();
        Task<Customer> GetById(int id);
        Task<Customer> Update(int id, Customer _customer);
        Task<Customer> Insert(Customer _customer);
        Task<Customer> Delete(int id);
    }
}