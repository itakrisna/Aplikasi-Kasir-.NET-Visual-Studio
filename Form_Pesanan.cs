using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Resto
{
    public partial class Form_Pesanan : Form
    {
        const string AccountSid = "AC174c0edc2b0f2155537c411b8388e3b4";
        const string AuthToken = "fb6c285de8adf85483d6d2cdb426a837";
        private string connectionString = "Data Source=DESKTOP-G96JCI6;Initial Catalog=DB_Resto;Integrated Security=True";

        public Form_Pesanan()
        {
            InitializeComponent();
        }

        private void button_kirim_Click(object sender, EventArgs e)
        {
            try
            {
                TwilioClient.Init(AccountSid, AuthToken);

                // Check if there is a selected row
                if (dataGridView_pesanan.SelectedRows.Count > 0)
                {
                    // Get the phone number from the second column of the selected row
                    string phoneNumber = dataGridView_pesanan.SelectedRows[0].Cells["No_Wa"].Value.ToString();

                    string messageText = $"Pesanan Anda sudah jadi dan siap untuk diambil. Terima kasih telah memesan di restoran kami! 🍔🍟";

                    if (!phoneNumber.StartsWith("+") || !phoneNumber.Substring(1).All(char.IsDigit))
                    {
                        MessageBox.Show("Nomor telepon tidak valid. Harap dimulai dengan '+' dan hanya menggunakan digit.");
                        return;
                    }

                    var to = new PhoneNumber($"whatsapp:{phoneNumber}");
                    var from = new PhoneNumber("whatsapp:+14155238886");

                    var message = MessageResource.Create(
                        to: to,
                        from: from,
                        body: messageText
                    );

                    // Update the status to "Pesanan Selesai" in the database
                    UpdateStatusToPesananSelesai(phoneNumber);

                    MessageBox.Show($"Pesan terkirim dengan ID: {message.Sid}");

                    // Refresh the DataGridView to show the updated data
                    TampilkanDataPesanan();
                }
                else
                {
                    MessageBox.Show("Pilih baris yang berisi nomor telepon yang ingin dikirimkan pesan.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending message: {ex.Message}");
            }
        }

        private void UpdateStatusToPesananSelesai(string phoneNumber)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Open the connection
                    connection.Open();

                    // Update the status in the database
                    string updateQuery = $"UPDATE dbo.Pesanan SET Status = 'Pesanan Selesai' WHERE No_Wa = '{phoneNumber}'";

                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // Close the connection
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating status: {ex.Message}");
            }
        }
        private void button_kembali_Click(object sender, EventArgs e)
        {
            Form_Kasir formKasir = new Form_Kasir();
            formKasir.Show();
            this.Close();
        }

        private void button_data_pesanan_Click(object sender, EventArgs e)
        {
            // Panggil fungsi untuk menampilkan data ke dataGridView_pesanan
            TampilkanDataPesanan();
        }

        private void TampilkanDataPesanan()
        {
            // Gantilah "ConnectionString" dengan koneksi database Anda.
            string connectionString = "Data Source=DESKTOP-G96JCI6;Initial Catalog=DB_Resto;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Query SQL untuk mendapatkan data dari tabel dbo.Pesanan
                string query = "SELECT * FROM dbo.Pesanan";

                // Buat objek SqlDataAdapter untuk mengambil data dari database
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);

                // Buat objek DataTable untuk menyimpan hasil query
                DataTable dataTable = new DataTable();

                // Buka koneksi database
                connection.Open();

                // Isi DataTable dengan hasil query
                adapter.Fill(dataTable);

                // Tutup koneksi database
                connection.Close();

                // Tampilkan data ke dataGridView_pesanan
                dataGridView_pesanan.DataSource = dataTable;
            }
        }
    }
}
