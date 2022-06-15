using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GetFastAdmin
{
    public partial class FormDataGrid : Form
    {
        SqlConnection connection = new SqlConnection(@"Data Source=91.236.239.56;Initial Catalog=getFastSQL;Persist Security Info=True;User ID=sa;Password=@aCLSkT5D@KQk6DQ");
        SqlCommand com = new SqlCommand();

        private int tempID;
        public FormDataGrid()
        {
            InitializeComponent();
            display();
            textBoxPassword.PasswordChar = '*';

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            display();
        }

        public void display()
        {
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("SELECT * FROM db_users", connection);
            DataTable dt = new DataTable();

            sqlDataAdapter.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlCommand cmd;
            connection.Open();

            com.Connection = connection;
            com.CommandText = "SELECT user_name, email FROM db_users";

            SqlDataReader dr = com.ExecuteReader();

            if (dr.Read())
            {
                if (textBoxUsername.Text.Equals(dr["user_name"].ToString()) || textBoxMail.Text.Equals(dr["email"].ToString()))
                {
                    MessageBox.Show("Username and/or email is same", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBoxMail.Clear();
                    textBoxUsername.Clear();

                    connection.Close();

                    return;
                }
                else
                {
                    dr.Close();
                    String sqlQuery = "INSERT INTO db_users ([user_name],[name],[surname],[password],[email],[address],[zip],[city],[country],[status],[userRole]) VALUES (@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11)";

                    string password_crypted = Cryptography.Encrypt(textBoxPassword.Text.ToString());

                    cmd = new SqlCommand(sqlQuery, connection);
                    cmd.Parameters.AddWithValue("@p1", textBoxUsername.Text);
                    cmd.Parameters.AddWithValue("@p2", textBoxName.Text);
                    cmd.Parameters.AddWithValue("@p3", textBoxSurname.Text);
                    cmd.Parameters.AddWithValue("@p4", password_crypted);
                    cmd.Parameters.AddWithValue("@p5", textBoxMail.Text);
                    cmd.Parameters.AddWithValue("@p6", textBoxAddress.Text);
                    cmd.Parameters.AddWithValue("@p7", textBoxZip.Text);
                    cmd.Parameters.AddWithValue("@p8", textBoxCity.Text);
                    cmd.Parameters.AddWithValue("@p9", textBoxCountry.Text);
                    cmd.Parameters.AddWithValue("@p10", comboBoxStatus.Text);
                    cmd.Parameters.AddWithValue("@p11", comboBoxUserRole.Text);

                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();

                    connection.Close();

                    display();
                    MessageBox.Show("Data inserted");
                    clearFields();

                }

            }
        }

        public void clearFields()
        {
            textBoxUsername.Clear();
            textBoxName.Clear();
            textBoxSurname.Clear();
            textBoxPassword.Clear();
            textBoxMail.Clear();
            textBoxAddress.Clear();
            textBoxZip.Clear();
            textBoxCity.Clear();
            textBoxCountry.Clear();
            comboBoxStatus.Text = "";
            comboBoxUserRole.Text = "";

        }

        private void button4_Click(object sender, EventArgs e)
        {
            connection.Open();

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewRow delRow = dataGridView1.Rows[i];

                if (delRow.Selected == true)
                {
                    String sqlQuery = "DELETE FROM db_users WHERE ID = '" + dataGridView1.Rows[i].Cells[0].Value + "'";

                    SqlCommand sqlCommand = new SqlCommand(sqlQuery, connection);
                    sqlCommand.ExecuteNonQuery();
                    dataGridView1.Rows.RemoveAt(i);

                    MessageBox.Show("Data deleted");
                }
            }

            connection.Close();
        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if(dataGridView1.Rows.Count > 0)
            {
                tempID = int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                textBoxUsername.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                textBoxName.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                textBoxSurname.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                textBoxPassword.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
                textBoxMail.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
                textBoxAddress.Text = dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
                textBoxZip.Text = dataGridView1.SelectedRows[0].Cells[7].Value.ToString();
                textBoxCity.Text = dataGridView1.SelectedRows[0].Cells[8].Value.ToString();
                textBoxCountry.Text = dataGridView1.SelectedRows[0].Cells[9].Value.ToString();
                comboBoxStatus.Text = dataGridView1.SelectedRows[0].Cells[10].Value.ToString();
                comboBoxUserRole.Text = dataGridView1.SelectedRows[0].Cells[11].Value.ToString();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SqlCommand cmd;
            connection.Open();

            com.Connection = connection;

            String sqlQuery = "UPDATE db_users SET user_name=@p1, name=@p2, surname=@p3, password=@p4, email=@p5, address=@p6, zip=@p7, city=@p8, country=@p9, status=@p10, userRole=@p11 WHERE ID = '" + tempID + "' ";

            string password_crypted = Cryptography.Encrypt(textBoxPassword.Text.ToString());

            cmd = new SqlCommand(sqlQuery, connection);
            cmd.Parameters.AddWithValue("@p1", textBoxUsername.Text);
            cmd.Parameters.AddWithValue("@p2", textBoxName.Text);
            cmd.Parameters.AddWithValue("@p3", textBoxSurname.Text);
            cmd.Parameters.AddWithValue("@p4", password_crypted);
            cmd.Parameters.AddWithValue("@p5", textBoxMail.Text);
            cmd.Parameters.AddWithValue("@p6", textBoxAddress.Text);
            cmd.Parameters.AddWithValue("@p7", int.Parse(textBoxZip.Text));
            cmd.Parameters.AddWithValue("@p8", textBoxCity.Text);
            cmd.Parameters.AddWithValue("@p9", textBoxCountry.Text);
            cmd.Parameters.AddWithValue("@p10", comboBoxStatus.Text);
            cmd.Parameters.AddWithValue("@p11", comboBoxUserRole.Text);

            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();

            connection.Close();

            display();
            MessageBox.Show("Data updated");

            clearFields();

        }

        private void textBoxPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void textBoxUsername_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
