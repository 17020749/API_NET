
using Microsoft.EntityFrameworkCore;
using API_CORE.Models;
using Microsoft.AspNetCore.Mvc;
namespace API_CORE.Services;
public class CustomerService : ICustomerService
{
    private readonly BikeStoresContext _DBContext;
    public CustomerService(BikeStoresContext dbContext)
    {
        _DBContext = dbContext;
    }

    public async Task<List<Customer>> GetAll()
    {
        List<Customer> res = new List<Customer>();
         res = await _DBContext.Customers.ToListAsync();
        return res;
    }
    public async Task<Object> GetById(int id)
    {
        Customer customer = new Customer();
         customer = await _DBContext.Customers.FindAsync(id);
        if (customer != null)
        {
            return customer;
        }
        else
        {
            return  new  {Message = "Not find id in table Customer"};
        }
    }
     public async Task<Customer> Update(int id, Customer _customer)
    {   
                try
                {
                    _DBContext.Update(_customer);
                    await _DBContext.SaveChangesAsync();
                    return _customer;
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(_customer.CustomerId))
                    {
                         throw;
                    }
                    else
                    {
                        throw;
                    }
                }

    }
    public async Task<Customer> Insert([Bind("CustomerId,FirstName,LastName,Phone,Email,Street,City,State,ZipCode")] Customer _customer)
    {
           _DBContext.Customers.Add(_customer);
            await _DBContext.SaveChangesAsync();
        return _customer;
    }

    public async Task<Customer> Delete(int id)
    {   
        
        var customer = await _DBContext.Customers.FindAsync(id);
                 _DBContext.Remove(customer);
                await _DBContext.SaveChangesAsync();
            return customer;
    }
 private bool CustomerExists(int id)
        {
          return (_DBContext.Customers?.Any(e => e.CustomerId == id)).GetValueOrDefault();
        }
}