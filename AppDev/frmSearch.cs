using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppDevAss_Mekalen
{
    public partial class frmSearch : Form
    {
        public frmSearch()
        {
            InitializeComponent();
        }

        private void fmSearch_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'hireDataSet.tblCar' table. You can move, or remove it, as needed.
            this.tblCarTableAdapter.Fill(this.hireDataSet.tblCar);

            string[] fieldNames = { "Make", "EngineSize", "RentalPerDay", "Available" };
            cboField.DataSource = fieldNames;


            string[] operatorSymbols = { "=", "<", ">", "<=", ">=" };
            cboOperator.DataSource = operatorSymbols;

        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            if (cboField.SelectedItem != null && cboOperator.SelectedItem != null && !string.IsNullOrEmpty(txtSearchValue.Text))
            {
                string selectedField = cboField.SelectedItem.ToString();
                string selectedOperator = cboOperator.SelectedItem.ToString();
                string searchValue = txtSearchValue.Text;

                // Handle the "Available" field separately
                if (selectedField == "Available" && (searchValue == "Yes" || searchValue == "No"))
                {
                    searchValue = (searchValue == "Yes") ? "1" : "0";
                }

                string query = $"SELECT * FROM tblCar WHERE {selectedField} {selectedOperator} @SearchValue";

                try
                {
                    using (SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Hire.mdf;Integrated Security=True "))
                    {
                        connection.Open();

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@SearchValue", searchValue);

                            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                            {
                                DataTable dataTable = new DataTable();
                                adapter.Fill(dataTable);

                                dataGridView1.DataSource = dataTable;

                                if (dataTable.Rows.Count == 0)
                                {
                                    MessageBox.Show("No matching records found.");
                                }
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please enter criteria in all three controls.");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();

            // Show the frmCars form (assuming it's an instance of FrmCars)
            frmCars frmCars = new frmCars();
            frmCars.Show();
        }
    }
}
