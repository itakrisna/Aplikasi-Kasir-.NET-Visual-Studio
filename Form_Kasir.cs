using Dapper;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using System.Linq;

namespace Resto
{
    public partial class Form_Kasir : Form
    {
        private string connectionString = "Data Source=DESKTOP-G96JCI6;Initial Catalog=DB_Resto;Integrated Security=True";
        const string AccountSid = "AC174c0edc2b0f2155537c411b8388e3b4";
        const string AuthToken = "fb6c285de8adf85483d6d2cdb426a837";
        public Form_Kasir()
        {
            InitializeComponent();

            // Panggil method untuk mengisi data menu ke dalam comboBox
            LoadMenuData();

            // Tambahkan event handler untuk menghandle perubahan pilihan di comboBox_menu
            comboBox_menu.SelectedIndexChanged += comboBox_menu_SelectedIndexChanged;
        }

        private void LoadMenuData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Menggunakan LINQ untuk mendapatkan data menu
                    var menuQuery = from row in connection.Query("SELECT Menu, Harga FROM DB_Resto.dbo.Menu")
                                    select new
                                    {
                                        Menu = row.Menu,
                                        Harga = row.Harga
                                    };

                    // Mengonversi hasil LINQ ke dalam DataTable
                    DataTable menuTable = new DataTable();
                    menuTable.Columns.Add("Menu", typeof(string));
                    menuTable.Columns.Add("Harga", typeof(decimal));

                    foreach (var item in menuQuery)
                    {
                        menuTable.Rows.Add(item.Menu, item.Harga);
                    }

                    comboBox_menu.DisplayMember = "Menu";
                    comboBox_menu.ValueMember = "Harga";
                    comboBox_menu.DataSource = menuTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading menu data: " + ex.Message);
            }
        }

        private void comboBox_menu_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_menu.SelectedIndex != -1)
            {
                textBox_harga.Text = comboBox_menu.SelectedValue.ToString();
            }
        }


        private void button_tambah_Click(object sender, EventArgs e)
        {
            string menu = comboBox_menu.Text;
            string harga = textBox_harga.Text;
            int jumlah = (int)numericUpDown_jumlah.Value;

            if (!string.IsNullOrEmpty(menu) && !string.IsNullOrEmpty(harga) && jumlah > 0)
            {
                decimal totalHarga = Convert.ToDecimal(harga) * jumlah;

                dataGridView_bill.Rows.Add(menu, jumlah, harga, totalHarga);

                UpdateTotalHarga(); // Panggil fungsi untuk mengupdate total harga

                comboBox_menu.SelectedIndex = -1;
                textBox_harga.Text = string.Empty;
                numericUpDown_jumlah.Value = 1;
            }
            else
            {
                if (jumlah == 0)
                {
                    MessageBox.Show("Jumlah harus lebih dari 0.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Mohon isi semua kolom data.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }


        private void button_hapus_Click(object sender, EventArgs e)
        {
            // Check if there is a selected row
            if (dataGridView_bill.SelectedRows.Count > 0)
            {
                // Remove the selected row
                dataGridView_bill.Rows.Remove(dataGridView_bill.SelectedRows[0]);

                // Update the total harga
                UpdateTotalHarga();
            }
            else
            {
                MessageBox.Show("Pilih item yang ingin dihapus.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void UpdateTotalHarga()
        {
            decimal totalHarga = 0;

            foreach (DataGridViewRow row in dataGridView_bill.Rows)
            {
                // Modify the column names to match your actual column names in dataGridView_bill
                decimal hargaItem = Convert.ToDecimal(row.Cells["Column_harga"].Value);
                int jumlahItem = Convert.ToInt32(row.Cells["Column_jumlah"].Value);
                totalHarga += hargaItem * jumlahItem;
            }

            textBox_total_harga.Text = totalHarga.ToString();
        }

        private void button_jumlah_kembalian_Click(object sender, EventArgs e)
        {
            UpdateKembalian();
        }

        private void UpdateKembalian()
        {
            // Memastikan bahwa isi dari textBox_jumlah_uang dapat diubah menjadi angka
            if (decimal.TryParse(textBox_jumlah_uang.Text, out decimal jumlahUang))
            {
                // Memastikan bahwa isi dari textBox_total_harga dapat diubah menjadi angka
                if (decimal.TryParse(textBox_total_harga.Text, out decimal totalHarga))
                {
                    // Menghitung kembalian
                    decimal kembalian = jumlahUang - totalHarga;

                    // Menampilkan kembalian di textBox_jumlah_kembalian
                    textBox_jumlah_kembalian.Text = kembalian.ToString("N2"); // Format menjadi dua angka desimal
                }
                else
                {
                    // Tidak dapat mengubah isi dari textBox_total_harga menjadi angka
                    MessageBox.Show("Isi TextBox Total Harga harus berupa angka.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                // Tidak dapat mengubah isi dari textBox_jumlah_uang menjadi angka
                MessageBox.Show("Isi TextBox Jumlah Uang harus berupa angka.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button_kembali_Click(object sender, EventArgs e)
        {
            // Instantiate the login form
            Form_Login formLogin = new Form_Login();

            // Show the login form
            formLogin.Show();

            // Close the current form
            this.Close();
        }

        private void button_next_from_Click(object sender, EventArgs e)
        {
            Form_Pesanan formPesanan = new Form_Pesanan();

            // Show the target form
            formPesanan.Show();

            // Optionally, hide the current form if you don't want to keep it open
            this.Hide();
        }

        private void button_print_bill_Click(object sender, EventArgs e)
        {

            try
            {
                // Memeriksa apakah nama pembeli dan nomor WhatsApp pembeli kosong
                if (string.IsNullOrWhiteSpace(textBox_nama_pembeli.Text) && string.IsNullOrWhiteSpace(textBox_no_wa_pembeli.Text))
                {
                    MessageBox.Show("Masukkan nama dan nomor WhatsApp pembeli.");
                    return;
                }
                // Memeriksa apakah nama pembeli kosong
                else if (string.IsNullOrWhiteSpace(textBox_nama_pembeli.Text))
                {
                    MessageBox.Show("Masukkan nama pembeli.");
                    return;
                }
                // Memeriksa apakah nomor WhatsApp pembeli kosong
                else if (string.IsNullOrWhiteSpace(textBox_no_wa_pembeli.Text))
                {
                    MessageBox.Show("Masukkan nomor WhatsApp pembeli.");
                    return;
                }

                // Menambahkan notifikasi jika kolom jumlah uang kosong
                if (string.IsNullOrWhiteSpace(textBox_jumlah_uang.Text))
                {
                    MessageBox.Show("Masukkan jumlah uang.");
                    return;
                }

                // Mengonversi Jumlah Kembalian ke tipe data decimal
                decimal additionalChangeAmount;

                // Memeriksa apakah kolom jumlah kembalian kosong sebelum mengonversi
                if (string.IsNullOrWhiteSpace(textBox_jumlah_kembalian.Text))
                {
                    // Menampilkan notifikasi jika kolom jumlah kembalian kosong saat tombol ditekan
                    MessageBox.Show("Masukkan jumlah kembalian sebelum menekan tombol print bill.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    // Mengonversi Jumlah Kembalian ke tipe data decimal jika tidak kosong
                    additionalChangeAmount = Convert.ToDecimal(textBox_jumlah_kembalian.Text);
                }

                // Menambahkan notifikasi jika kolom jumlah kembalian kosong setelah mengonversi
                if (string.IsNullOrWhiteSpace(textBox_jumlah_kembalian.Text))
                {
                    MessageBox.Show("Masukkan jumlah kembalian.");
                    return;
                }

                TwilioClient.Init(AccountSid, AuthToken);


                // Ganti dengan nomor penerima dan pesan Anda
                string phoneNumber = textBox_no_wa_pembeli.Text;
                string buyerName = textBox_nama_pembeli.Text;

                // Pastikan nomor penerima terisi dan dimulai dengan "+" 
                if (!phoneNumber.StartsWith("+") || !phoneNumber.Substring(1).All(char.IsDigit))
                {
                    MessageBox.Show("Nomor telepon tidak valid. Harap dimulai dengan '+' dan hanya menggunakan digit.");
                    return;
                }



                // Create a StringBuilder to build the message text
                StringBuilder messageBuilder = new StringBuilder();
                messageBuilder.AppendLine($"👤 Nama Pembeli: {buyerName}");
                messageBuilder.AppendLine("📋 Detail Pesanan:");

                foreach (DataGridViewRow row in dataGridView_bill.Rows)
                {
                    string menu = row.Cells["Column_menu"].Value?.ToString() ?? "";
                    int quantity = Convert.ToInt32(row.Cells["Column_jumlah"].Value);
                    decimal price = Convert.ToDecimal(row.Cells["Column_harga"].Value);
                    decimal totalAmount = Convert.ToDecimal(row.Cells["Column_total_harga"].Value);

                    messageBuilder.AppendLine($"🍔 {menu} x{quantity} - Rp {totalAmount:N2}");

                    // Insert each line item into the database
                    InsertOrderItemIntoDatabase(buyerName, phoneNumber, menu, quantity, price, totalAmount);
                }

                decimal totalAmountValue = Convert.ToDecimal(textBox_total_harga.Text);
                decimal paidAmountValue = Convert.ToDecimal(textBox_jumlah_uang.Text);
                decimal changeAmountValue = Convert.ToDecimal(textBox_jumlah_kembalian.Text);

                messageBuilder.AppendLine($"💸 Total Harga: Rp {totalAmountValue:N2}");
                messageBuilder.AppendLine($"💰 Jumlah Uang: Rp {paidAmountValue:N2}");
                messageBuilder.AppendLine($"🌟 Jumlah Kembalian: Rp {changeAmountValue:N2}");

                string messageText = messageBuilder.ToString();

                var to = new PhoneNumber($"whatsapp:{phoneNumber}");
                var from = new PhoneNumber("whatsapp:+14155238886"); // Twilio sandbox number

                var message = MessageResource.Create(
                    to: to,
                    from: from,
                    body: messageText
                );

                // Check if the message object is not null before accessing its properties
                if (message != null && !string.IsNullOrEmpty(message.Sid))
                {
                    // Cetak ID pesan untuk referensi tagihan
                    MessageBox.Show($"Pesan terkirim dengan ID: {message.Sid}");

                    // Comment out or remove the line below if you don't have the NotifyOrderReceived method
                    // NotifyOrderReceived();
                }
                else
                {
                    MessageBox.Show("Error sending message. Message object is null or the SID is empty.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void InsertOrderItemIntoDatabase(string buyerName, string phoneNumber, string menu, int quantity, decimal price, decimal totalAmount)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Insert each line item into the database
                    string orderItemQuery = "INSERT INTO dbo.Pesanan (Nama_Pembeli, No_Wa, Menu, Jumlah, Harga, Total_Harga) " +
                                            "VALUES (@Nama_Pembeli, @No_Wa, @Menu, @Jumlah, @Harga, @Total_Harga)";

                    using (SqlCommand orderItemCommand = new SqlCommand(orderItemQuery, connection))
                    {
                        orderItemCommand.Parameters.AddWithValue("@Nama_Pembeli", buyerName);
                        orderItemCommand.Parameters.AddWithValue("@No_Wa", phoneNumber);
                        orderItemCommand.Parameters.AddWithValue("@Menu", menu);
                        orderItemCommand.Parameters.AddWithValue("@Jumlah", quantity);
                        orderItemCommand.Parameters.AddWithValue("@Harga", price);
                        orderItemCommand.Parameters.AddWithValue("@Total_Harga", totalAmount);

                        orderItemCommand.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inserting into database: {ex.Message}");
            }
        }
    }
}
