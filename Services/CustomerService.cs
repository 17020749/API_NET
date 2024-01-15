
using Microsoft.EntityFrameworkCore;
using API_CORE.Models;
using API_CORE.Services;
using API_CORE.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ProductAPIVS.Container;
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
    public async Task<Customer> GetbyCode(int id)
    {
        var customer = await _DBContext.Customers.FindAsync(id);
        if (customer != null)
        {
            
            return customer;
        }
        else
        {
            return new Customer();
        }
    }
     public async Task<Customer> Update(int id, Customer _customer)
    {   
        
        var account = await _DBContext.Customers.SingleOrDefaultAsync(i => i.CustomerId == id);
                await _DBContext.SaveChangesAsync();
            return account;
    }
    public async Task<Customer> Insert(Customer _customer)
    {
           _DBContext.Customers.Add(_customer);
            await _DBContext.SaveChangesAsync();

    }

    public async Task<Customer> Delete(int id)
    {   
        
        var customer = await _DBContext.Customers.FindAsync(id);
                 _DBContext.Remove(customer);
                await _DBContext.SaveChangesAsync();
            return customer;
    }



}