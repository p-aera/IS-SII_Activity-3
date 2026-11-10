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
    public partial class RegisterForm : Form
    {
        private OleDbConnection con;

        public RegisterForm()
        {
            InitializeComponent();

            Passbox.UseSystemPasswordChar = true;
            PassConbox.UseSystemPasswordChar = true;
        }

        private void RegisBut_Click(object sender, EventArgs e)
        {
            string fullName = FNbox.Text.Trim();
            string username = Userbox.Text.Trim();
            string Password = Passbox.Text;
            string confirmPassword = PassConbox.Text;


            if (fullName == "" || username == "" || Password == "" || confirmPassword == "")
            {
                MessageBox.Show("Please fill in all fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Password.Length < 8)
            {
                MessageBox.Show("Password must be at least 8 characters long", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (OleDbConnection con = database.GetDbConnection())
                {
                    con.Open();

                    string checkQuerry = "SELECT COUNT(*) FROM Users WHERE Username = ?";
                    using (OleDbCommand checkCmd = new OleDbCommand(checkQuerry, con))
                    {
                        checkCmd.Parameters.AddWithValue("?", username);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (count > 0) 
                        {
                            MessageBox.Show("Username already exists. Please choose a different username.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    string query = @"INSERT INTO Users (Username, PasswordHash, FullName, UserRole, isActive) VALUES (?, ?, ?, ?, ?)";
                    using (OleDbCommand cmd = new OleDbCommand(query, con))
                    {
                        string passwordHash = PasswordHelper.HashPassword(Password);
                        cmd.Parameters.AddWithValue("?", username);
                        cmd.Parameters.AddWithValue("?", passwordHash);
                        cmd.Parameters.AddWithValue("?", fullName);
                        cmd.Parameters.AddWithValue("?", "User");
                        cmd.Parameters.AddWithValue("?", true);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("User registered successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            FNbox.Clear();
                            Userbox.Clear();
                            Passbox.Clear();
                            PassConbox.Clear();

                            SignInForm login = new SignInForm();
                            login.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Failed to register user.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An Error occurred while registering: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FNbox_TextChanged(object sender, EventArgs e)
        {

        }

        private void RegisterForm_Load(object sender, EventArgs e)
        {

        }
    }
}