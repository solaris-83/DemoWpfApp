using DemoWpfApp.Models;
using System.Collections.Generic;

namespace DemoWpfApp.Services.Interfaces
{
    public interface ICustomerRepository
    {
        bool Add(Customer customer);
        bool Remove(Customer customer);
        bool Commit();
        IEnumerable<Customer> Customers { get; }
    }
}
