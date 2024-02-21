using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Resto
{
    public partial class Form_Login : Form
    {
        public Form_Login()
        {
            InitializeComponent();
        }
        private void button_login_Click(object sender, EventArgs e)
        {
            String constr = "Data Source=DESKTOP-G96JCI6;Initial Catalog=DB_Resto;Integrated Security=True";
            SqlConnection con = new SqlConnection(constr);

            try

            {

                if (con.State == ConnectionState.Open) { con.Close(); }
                con.Open();
                String sql = "select * from dbo.Login where Nama = '" + textBox_nama.Text + "' and Password = '" + textBox_password.Text + "'";
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows == false)

                {
                    MessageBox.Show("mohon isi nama dan password anda", "Perhatian", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox_nama.Focus();
                    return;

                }
                else
                {
                    Form_Kasir myMenu = new Form_Kasir();
                    myMenu.Show();
                    this.Hide();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
