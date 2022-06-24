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
            com.CommandText = "SELECT email FROM db_users";

            SqlDataReader dr = com.ExecuteReader();

            if (dr.Read())
            {
                if (textBoxMail.Text.Equals(dr["email"].ToString()))
                {
                    MessageBox.Show("Email is same", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBoxMail.Clear();


                    connection.Close();

                    return;
                }
                else
                {
                    dr.Close();
                    String sqlQuery = "INSERT INTO db_users ([firstname],[lastname],[password],[email],[address],[tel],[status],[role]) VALUES (@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8)";

                    string password_crypted = AESEncryption.Encrypt(textBoxPassword.Text.ToString(), "YFpoGQ@$VrUMf64tZ9eg^RiaQSZ^Pw%*"); //Cryptography.Encrypt(textBoxPassword.Text.ToString());

                    cmd = new SqlCommand(sqlQuery, connection);
                    cmd.Parameters.AddWithValue("@p1", textBoxFirstname.Text);
                    cmd.Parameters.AddWithValue("@p2", textBoxLastname.Text);
                    cmd.Parameters.AddWithValue("@p3", password_crypted);
                    cmd.Parameters.AddWithValue("@p4", textBoxMail.Text);
                    cmd.Parameters.AddWithValue("@p5", textBoxAddress.Text);
                    cmd.Parameters.AddWithValue("@p6", textBoxTel.Text);
                    cmd.Parameters.AddWithValue("@p7", comboBoxStatus.Text);
                    cmd.Parameters.AddWithValue("@p8", comboBoxUserRole.Text);

                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();

                    connection.Close();

                    display();
                    MessageBox.Show("Utilisateur ajouté");
                    clearFields();

                }

            }
        }

        public void clearFields()
        {
            textBoxFirstname.Clear();
            textBoxLastname.Clear();
            textBoxPassword.Clear();
            textBoxMail.Clear();
            textBoxAddress.Clear();
            textBoxTel.Clear();
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

                    MessageBox.Show("Utilisateur supprimé");
                    connection.Close();
                    clearFields();
                }
            }

            connection.Close();
        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if(dataGridView1.Rows.Count > 0)
            {
                tempID = int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                textBoxFirstname.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                textBoxLastname.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                textBoxPassword.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                textBoxMail.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
                textBoxAddress.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
                textBoxTel.Text = dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
                comboBoxStatus.Text = dataGridView1.SelectedRows[0].Cells[7].Value.ToString();
                comboBoxUserRole.Text = dataGridView1.SelectedRows[0].Cells[8].Value.ToString();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SqlCommand cmd;
            connection.Open();

            com.Connection = connection;

            String sqlQuery = "UPDATE db_users SET firstname=@p1, lastname=@p2, password=@p3, email=@p4, address=@p5, tel=@p6, status=@p7, role=@p8 WHERE ID = '" + tempID + "' ";

            string password_crypted = AESEncryption.Encrypt(textBoxPassword.Text, "YFpoGQ@$VrUMf64tZ9eg^RiaQSZ^Pw%*"); //Cryptography.Encrypt(password_decrypt);

            cmd = new SqlCommand(sqlQuery, connection);

                cmd.Parameters.AddWithValue("@p1", textBoxFirstname.Text);
                cmd.Parameters.AddWithValue("@p2", textBoxLastname.Text);
                cmd.Parameters.AddWithValue("@p3", password_crypted);
                cmd.Parameters.AddWithValue("@p4", textBoxMail.Text);
                cmd.Parameters.AddWithValue("@p5", textBoxAddress.Text);
                cmd.Parameters.AddWithValue("@p6", textBoxTel.Text);
                cmd.Parameters.AddWithValue("@p7", comboBoxStatus.Text);
                cmd.Parameters.AddWithValue("@p8", comboBoxUserRole.Text);

            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();

            connection.Close();

            display();
            MessageBox.Show("✅ Utlisateur mis à jour ✅");

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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void textBoxFirstname_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
