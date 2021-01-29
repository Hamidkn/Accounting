using Accounting.DataLayer.Repositories;
using Accounting.DataLayer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.DataLayer.UnitOfWork
{
    public class UnitOfWork : IDisposable
    {
        //در این کلاس تعریف پایگاه داده و تمام
        //repository
        //ها اینجا ایجاد میشود تا دسترسی به بانک اطلاعاتی محدودتر شود.
        Accounting_DBEntities DataSet = new Accounting_DBEntities();

        // در اینجا هرچند تعداد repository داشته باشیم 
        // مثل همین خط به آن اضافه میکنیم
        private ICustomerRepository _customerRepository;
        public ICustomerRepository CustomerRepository
        {
            get
            {
                if(_customerRepository == null)
                {
                    _customerRepository = new CustomerRepository(DataSet);
                }
                return _customerRepository;
            }
        }

        public void SaveCustomer()
        {
            DataSet.SaveChanges();
        }

        //برای ازبین بردن منابعی که توسط این پایگاه داده استفاده میشود
        public void Dispose()
        {
            DataSet.Dispose();
        }
    }
}
