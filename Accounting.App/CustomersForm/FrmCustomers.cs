using Accounting.App.CustomersForm;
using Accounting.DataLayer.UnitOfWork;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Accounting.App
{
    public partial class FrmCustomers : Form
    {
        public FrmCustomers()
        {
            InitializeComponent();
        }

        private void FrmCustomers_Load(object sender, EventArgs e)
        {
            BindGrid();
        }

        void BindGrid()
        {
            using(UnitOfWork db = new UnitOfWork())
            {
                //خط زیر برای این هست که فقط ستون هایی که ما میخواهیم نمایش داده شود.
                dgCustomers.AutoGenerateColumns = false;
                dgCustomers.DataSource = db.CustomerRepository.GetAllCustomers();
            }
        }

        private void btnRefreshCustomer_Click(object sender, EventArgs e)
        {
            txtFilter.Text = null;
            BindGrid();
        }

        private void txtFilter_Click(object sender, EventArgs e)
        {

        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            using(UnitOfWork db = new UnitOfWork())
            {
                dgCustomers.DataSource = db.CustomerRepository.GetCustomersByFilter(txtFilter.Text); 
            }
        }

        private void btnRemoveCustomer_Click(object sender, EventArgs e)
        {
            if(dgCustomers.CurrentRow != null)
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    string message = dgCustomers.CurrentRow.Cells[1].Value.ToString();
                    if (MessageBox.Show($"آیا از حذف {message} مطمئن هستید؟","هشدار",MessageBoxButtons.YesNo,MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        int customerId = int.Parse(dgCustomers.CurrentRow.Cells[0].Value.ToString());
                        db.CustomerRepository.DeleteCustomer(customerId);
                        db.SaveCustomer();
                        BindGrid();
                    }
                }
            }
            else
            {
                MessageBox.Show("لطفا یک شخص را انتخاب کنید.");
            }
        }

        private void btnAddNewCustomer_Click(object sender, EventArgs e)
        {
            FrmAddNewCustomer frmAddNewCustomers = new FrmAddNewCustomer();
            if(frmAddNewCustomers.ShowDialog() == DialogResult.OK)
            {
                BindGrid();
            }
        }

        private void btnEditCustomer_Click(object sender, EventArgs e)
        {
            if(dgCustomers.CurrentRow != null)
            {
                int customerId = int.Parse(dgCustomers.CurrentRow.Cells[0].Value.ToString());
                FrmAddNewCustomer frmAddNew = new FrmAddNewCustomer();
                frmAddNew.customerId = customerId;
                if(frmAddNew.ShowDialog() == DialogResult.OK)
                {
                    BindGrid();
                }
            }
        }
    }
}
