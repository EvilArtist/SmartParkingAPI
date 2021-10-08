using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SmartParkingAbstract.Services.Customers;
using SmartParkingAbstract.ViewModels.Customers;
using SmartParkingAbstract.ViewModels.DataImport;
using SmartParkingCoreModels.Customers;
using SmartParkingCoreModels.Data;
using SmartParkingCoreServices.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingCoreServices.Customers
{
    public class CustomerService : MultitanentService, ICustomerService
    {
        private readonly IMapper mapper;
        private readonly ApplicationDbContext dbContext;
        private readonly IHttpContextAccessor httpContextAccessor;

        public CustomerService(IMapper mapper,
            ApplicationDbContext dbContext,
            IHttpContextAccessor httpContextAccessor): base(httpContextAccessor)
        {
            this.mapper = mapper;
            this.dbContext = dbContext;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task<IEnumerable<CustomerViewModel>> ImportData(IEnumerable<CustomerDataImport> dataImport)
        {
            try
            {
                var customersCode = dataImport.Select(x => x.CustomerCode).ToList();
                var existingCustomers = await dbContext.Customers
                    .Where(x => customersCode.Contains(x.CustomerCode) && x.ClientId == ClientId)
                    .ToListAsync();

                foreach (var customer in existingCustomers)
                {
                    var data = dataImport.First(x => x.CustomerCode == customer.CustomerCode);
                    mapper.Map(data, customer);
                }
                dbContext.UpdateRange(existingCustomers);
                List<Customer> newCustomers = dataImport
                    .Where(x => !existingCustomers.Any(y => y.CustomerCode == x.CustomerCode))
                    .Select(x => mapper.Map<Customer>(x)).ToList();
                await dbContext.AddRangeAsync(newCustomers);
                await dbContext.SaveChangesAsync();
                return mapper.Map<IEnumerable<CustomerViewModel>>(existingCustomers.Union(newCustomers));
            }
            catch (Exception)
            {
                return new List<CustomerViewModel>();
            }
        }
    }
}
