﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pizza_Project.database.controllers.data_controllers.person_controllers;
using Pizza_Project.database.Models.customer_info;
using Pizza_Project.database.Models.person_info;
using Pizza_Project.helper_classes;

namespace Pizza_Project.Forms
{
    public partial class CustomerListPageForm : Form
    {
        static CustomerController c1 = new CustomerController();
        List<Customer> listOfCustomers;
        string customerKey;
        MainSelectionPage referencedPage;

        public CustomerListPageForm()
        {

        }
        public CustomerListPageForm(MainSelectionPage referencedPage)
        {
            this.referencedPage = referencedPage;

            InitializeComponent();

            FixWindowSize.FixLayout(this);

            listOfCustomers = c1.Read();
            CustomerListDataGrid.ColumnCount = 2;
            CustomerListDataGrid.Columns[0].HeaderText = "Name";
            CustomerListDataGrid.Columns[1].HeaderText = "Number";
            CustomerListDataGrid.Columns[0].DefaultCellStyle.Padding = new Padding(5);
            CustomerListDataGrid.Columns[1].DefaultCellStyle.Padding = new Padding(5);

            int currentRow = 0;
            foreach (var cust in listOfCustomers)
            {
                addToCustomerList(cust);
                CustomerListDataGrid.Rows[currentRow].HeaderCell.Value = cust.Id;
                currentRow++;  
            }
            addButtonColumn();
        }

        private void addButtonColumn()
        {
            DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
            btn.HeaderText = "";
            btn.Text = "More Info";
            btn.Name = "MoreInfoButton";
            btn.UseColumnTextForButtonValue = true;
            btn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            CustomerListDataGrid.Columns.Add(btn);
        }
        private void addToCustomerList(Customer customer)
        {

            var item = new ListViewItem( new String [] {customer.Name, customer.PhoneNumber} );
            CustomerListDataGrid.Rows.Add(new String[] { customer.Name, customer.PhoneNumber });
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CustomerListPageForm_Load(object sender, EventArgs e)
        {

        }
        // If button inside a cell is clicked
        private void CustomerListDataGrid_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
         
                if (!GlobalVariables.CurrentUserRole.Equals("manager"))
                {
                    this.errorLabel.Visible = true;
                    return;
                }

                customerKey = (string)CustomerListDataGrid.Rows[e.RowIndex].HeaderCell.Value;
                var customerInfoPage = new CustomerInfoPageForm(this.customerKey, this);
                this.Hide();
                customerInfoPage.ShowDialog();
                this.Show();
            }
        }
    }
}
