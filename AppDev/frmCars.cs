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
    public partial class frmCars : Form
    {
        public frmCars()
        {
            InitializeComponent();
        }

        private void UpdateRecordCount()
        {
            int totalRecords = tblCarBindingSource.Count;
            int currentRecord = tblCarBindingSource.Position + 1; // Position is 0-based

            txtCount.Text = currentRecord + " / " + totalRecords;
        }

        private void frmCars_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'hireDataSet.tblCar' table. You can move, or remove it, as needed.
            this.tblCarTableAdapter.Fill(this.hireDataSet.tblCar);


        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Hire.mdf;Integrated Security=True"))
                {
                    connection.Open();

                    string updateQuery = "UPDATE tblCar " +
                                        "SET Make = @Make, " +
                                        "EngineSize = @EngineSize, " +
                                        "DateRegistered = @DateReg, " +
                                        "RentalPerDay = @RentalPerDay, " +
                                        "Available = @Available " +
                                        "WHERE VehicleRegNo = @VehicleRegNo";

                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        // Set parameters based on the textboxes and checkbox
                        command.Parameters.AddWithValue("@Make", txtMake.Text);
                        command.Parameters.AddWithValue("@EngineSize", txtEngineSize.Text);
                        command.Parameters.AddWithValue("@DateReg", DateTime.Parse(txtDateReg.Text));
                        command.Parameters.AddWithValue("@RentalPerDay", Convert.ToDecimal(txtRentalPerDay.Text));
                        command.Parameters.AddWithValue("@Available", chckAvail.Checked);
                        command.Parameters.AddWithValue("@VehicleRegNo", txtVehicleRegNo.Text);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Record updated successfully.");
                        }
                        else
                        {
                            MessageBox.Show("No records were updated.");
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Hire.mdf;Integrated Security=True"))
                {
                    connection.Open();

                    // Create the SQL INSERT statement
                    string insertQuery = "INSERT INTO tblCar (VehicleRegNo, Make, EngineSize, DateRegistered, RentalPerDay, Available) " +
                                        "VALUES (@VehicleRegNo, @Make, @EngineSize, @DateRegistered, @RentalPerDay, @Available)";

                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        // Set parameters based on the input fields
                        command.Parameters.AddWithValue("@VehicleRegNo", txtVehicleRegNo.Text);
                        command.Parameters.AddWithValue("@Make", txtMake.Text);
                        command.Parameters.AddWithValue("@EngineSize", txtEngineSize.Text);
                        command.Parameters.AddWithValue("@DateRegistered", DateTime.Parse(txtDateReg.Text));
                        command.Parameters.AddWithValue("@RentalPerDay", Convert.ToDecimal(txtRentalPerDay.Text));
                        command.Parameters.AddWithValue("@Available", chckAvail.Checked);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Record inserted successfully.");
                        }
                        else
                        {
                            MessageBox.Show("Insertion failed.");
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Hire.mdf;Integrated Security=True"))
                {
                    connection.Open();

                    // Create the SQL DELETE statement
                    string deleteQuery = "DELETE FROM tblCar WHERE VehicleRegNo = @VehicleRegNo";

                    using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                    {
                        // Set the parameter for the VehicleRegNo based on the input field
                        command.Parameters.AddWithValue("@VehicleRegNo", txtVehicleRegNo.Text);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Record deleted successfully.");
                        }
                        else
                        {
                            MessageBox.Show("No records were deleted.");
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            frmSearch frmSearch = new frmSearch();
            frmSearch.Show();
            this.Hide();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtVehicleRegNo.Text = string.Empty;
            txtMake.Text = string.Empty;
            txtEngineSize.Text = string.Empty;
            txtDateReg.Text = string.Empty;
            txtRentalPerDay.Text = string.Empty;
            chckAvail.Checked = false;


            MessageBox.Show("Changes canceled");
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            tblCarBindingSource.Position = tblCarBindingSource.Count - 1;
            UpdateRecordCount();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (tblCarBindingSource.Position + 1 < tblCarBindingSource.Count)
                tblCarBindingSource.MoveNext();
            UpdateRecordCount(); // Update the record count after moving
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (tblCarBindingSource.Position - 1 >= 0)
                tblCarBindingSource.MovePrevious();
            UpdateRecordCount();
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            tblCarBindingSource.MoveFirst();
            UpdateRecordCount();
        }

        private void chckAvail_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
