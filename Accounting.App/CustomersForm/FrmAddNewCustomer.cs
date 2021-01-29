using Accounting.DataLayer;
using Accounting.DataLayer.UnitOfWork;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ValidationComponents;

namespace Accounting.App.CustomersForm
{
    public partial class FrmAddNewCustomer : Form
    {
        public int customerId = 0;
        //UnitOfWork db = new UnitOfWork();

        public FrmAddNewCustomer()
        {
            InitializeComponent();
        }

        private void btnSelectPhoto_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if(openFile.ShowDialog() == DialogResult.OK)
            {
                pcCustomer.ImageLocation = openFile.FileName;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (BaseValidator.IsFormValid(this.components))
            {
                using(UnitOfWork db = new UnitOfWork())
                {
                    //در خط بعدی قمست اول یک نام منحصر بخ فرد تولید میکند و
                    //قسمت بعدی فرمت تصویر را از روی لوکیشن تصویر به دست میاورد
                    string imageName = Guid.NewGuid().ToString() + Path.GetExtension(pcCustomer.ImageLocation);
                    string path = Application.StartupPath + "/Images/";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    pcCustomer.Image.Save(path + imageName);
                    Customers customers = new Customers()
                    {
                        FullName = txtName.Text,
                        Mobile = txtMobile.Text,
                        Email = txtEmail.Text,
                        Address = txtAddress.Text,
                        CustomerImage = imageName
                    };
                    if (customerId == 0)
                    {
                        db.CustomerRepository.InsertCustomer(customers);
                    }
                    else
                    {
                        customers.CustomerID = customerId;
                        db.CustomerRepository.UpdateCustomer(customers);
                    }
                    db.SaveCustomer();
                    DialogResult = DialogResult.OK;
                }
            }
        }

        private void FrmAddNewCustomer_Load(object sender, EventArgs e)
        {
            using( UnitOfWork db = new UnitOfWork())
            {
                if (customerId != 0)
                {
                    this.Text = "ویرایش شخص";
                    btnSave.Text = "ویرایش";
                    var customer = db.CustomerRepository.GetCustomerById(customerId);
                    txtName.Text = customer.FullName;
                    txtMobile.Text = customer.Mobile;
                    txtEmail.Text = customer.Email;
                    txtAddress.Text = customer.Address;
                    pcCustomer.ImageLocation = Application.StartupPath + "/Images/" + customer.CustomerImage;
                }
            }
        }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.Cancel;
    }
  }
}
