using SmartParkingAbstract.ViewModels.Customers;
using SmartParkingAbstract.ViewModels.DataImport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingAbstract.Services.Customers
{
    public interface ICustomerService
    {
        public Task<IEnumerable<CustomerViewModel>> ImportData(IEnumerable<CustomerDataImport> dataImport);
    }
}
