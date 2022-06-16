﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace GetFastAdmin
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
            textBox_password.PasswordChar = '*';

        }

        SqlConnection connection = new SqlConnection(@"Data Source=91.236.239.56;Initial Catalog=getFastSQL;Persist Security Info=True;User ID=sa;Password=@aCLSkT5D@KQk6DQ");
        SqlCommand com = new SqlCommand();
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string Password = "";
            string role = "";
            bool IsExist = false;

            connection.Open();

            com.Connection = connection;
            com.CommandText = "SELECT * FROM db_users WHERE email = '" + textBoxEmail.Text + "'";

            SqlDataReader dr = com.ExecuteReader();

            if (dr.Read())
            {
                Password = dr.GetString(3);  //get the user password from db if the user name is exist in that.  
                IsExist = true;
            }

            if (IsExist)  //if record exis in db , it will return true, otherwise it will return false  
            {
                role = dr.GetString(8);
                connection.Close();
                if (role == "role_technique" || role == "role_commercial")
                {
                    if (Cryptography.Decrypt(Password).Equals(textBox_password.Text))
                    {
                        this.Hide();
                        FormDataGrid formDatGrid = new FormDataGrid();
                        formDatGrid.ShowDialog();

                    }
                    else
                    {
                        connection.Close();
                        MessageBox.Show("Password is wrong!...", "error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    connection.Close();
                    MessageBox.Show("Vous n'etes pas du staff", "error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                
            }
            else  //showing the error message if user credential is wrong  
            {
                connection.Close();
                MessageBox.Show("Please enter the valid credentials", "error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void textBox_username_TextChanged(object sender, EventArgs e)
        {

        }

        private void quitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
