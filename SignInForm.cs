using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Activity3_DeDios
{
    public partial class SignInForm : Form
    {
        public SignInForm()
        {
            InitializeComponent();
            Passbox.UseSystemPasswordChar = true;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void SignBut_Click(object sender, EventArgs e)
        {
            string username = Userbox.Text.Trim();
            string password = Passbox.Text;

            if (username == "" || password == "")
            {
                MessageBox.Show("Please enter both username and password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (OleDbConnection con = database.GetDbConnection())
                {
                    con.Open();

                    string query = "SELECT * FROM Users WHERE Username = ?";

                    using (OleDbCommand cmd = new OleDbCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("?", username);

                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                MessageBox.Show("Invalid username or password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            string storedHashed = reader["PasswordHash"].ToString();
                            string fullName = reader["FullName"].ToString();
                            string role = reader["UserRole"].ToString();
                            bool isActive = Convert.ToBoolean(reader["isActive"]);
                            int userId = Convert.ToInt32(reader["UserID"]);

                            reader.Close();

                            if (!PasswordHelper.VerifyPassword(password, storedHashed))
                            {
                                MessageBox.Show("Invalid username or password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            if (!isActive)
                            {
                                MessageBox.Show("Your account is inactive. Please contact the administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            MessageBox.Show($"Welcome, {fullName}!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            Dashboard dashboard = new Dashboard();
                            dashboard.Show();
                            this.Hide();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An Error occurred while signing in: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RegButton_Click(object sender, EventArgs e)
        {

            RegisterForm regForm = new RegisterForm();

            regForm.Show();

            this.Hide();
        }

        private void SignInForm_Load(object sender, EventArgs e)
        {

        }
    }
}