using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Accounting.DataLayer.Repositories;

namespace Accounting.DataLayer.Services
{
    public class CustomerRepository : ICustomerRepository
    {
        // اولین کار ایجاد یک نمونه از پایگاه داده است.
        //در واقع کانکشن اینجا ایجاد میشود یعنی باز میشود
        //نوشتن خط بعدی مشکل ایجاد میکند چون خیلی راحت به لایه های دیگر اجازه دسترسی به 
        //بانک میدهد. برای اینکار کلاس unitof work ایجاد میشود
        //خط بعدی را کامنت میکنیم و مراحل بعدی را در ادامه اضافه میکنیم
        //Accounting_DBEntities DataSet = new Accounting_DBEntities();
        private Accounting_DBEntities DataSet;
        public CustomerRepository(Accounting_DBEntities DataSetEntity)
        {
            DataSet = DataSetEntity;
        }

        public bool DeleteCustomer(int customerId)
        {
            try
            {
                var customer = GetCustomerById(customerId);
                DeleteCustomer(customer);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteCustomer(Customers customer)
        {
            try
            {
                DataSet.Entry(customer).State = EntityState.Deleted;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<Customers> GetAllCustomers()
        {
            return DataSet.Customers.ToList();
        }

        public Customers GetCustomerById(int customerId)
        {
            return DataSet.Customers.Find(customerId);
        }

        public IEnumerable<Customers> GetCustomersByFilter(string parameter)
        {
            return DataSet.Customers.Where(ct => ct.FullName.Contains(parameter) ||
            ct.Email.Contains(parameter) || ct.Mobile.Contains(parameter)).ToList();
        }

        public bool InsertCustomer(Customers customer)
        {
            try
            {
                DataSet.Customers.Add(customer);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateCustomer(Customers customer)
        {
            try
            {
                DataSet.Entry(customer).State = EntityState.Modified;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
