Aplikasi Kasir .NET Visual Studio dengan LINQ, database dan otentikasi Terhubung WhatsApp untuk Layanan Self-Service

![image](https://github.com/itakrisna/Aplikasi-Kasir-.NET-Visual-Studio/assets/152336076/d1b869b6-3bb4-415b-84a1-2614911f24a8)

Form_Login: 
* Menangani otentikasi pengguna terhadap database ( DB_Resto). 
* Menggunakan SqlConnectiondan SqlCommanduntuk menanyakan database untuk kredensial login. 
* Membuka formulir baru ( Form_Kasir) setelah login berhasil. 

![image](https://github.com/itakrisna/Aplikasi-Kasir-.NET-Visual-Studio/assets/152336076/36ea5e5a-e10a-4b85-8467-6d1eae07d124)

Form_Kasir: 
* Memuat Data Menu: Mengambil data menu dari database menggunakan Dapper dan LINQ. Menampilkan data menu dalam ComboBox (comboBox_menu) untuk dipilih oleh kasir. 
* Tambah Item ke Bill: Kasir dapat menambahkan item pesanan ke DataGridView (dataGridView_bill) dengan menentukan menu, harga, dan jumlah. Tombol "button_tambah" memicu penambahan item ke DataGridView dan mengupdate total harga. 
* Hapus Item dari Bill: Kasir dapat menghapus item yang dipilih dari DataGridView menggunakan tombol "button_hapus". 
* Hitung dan Tampilkan Total Harga: Total harga dari seluruh item dalam DataGridView dihitung dan ditampilkan di TextBox (textBox_total_harga). 
* Hitung dan Tampilkan Kembalian: Kasir memasukkan jumlah uang yang diterima oleh pelanggan. Jumlah kembalian dihitung dan ditampilkan di TextBox (textBox_jumlah_kembalian). 
* Cetak Tagihan melalui WhatsApp: Tombol "button_print_bill" mengirim rincian pesanan dan tagihan kepada pelanggan melalui WhatsApp menggunakan Twilio API. Informasi pesanan dikirim sebagai pesan WhatsApp. 

![image](https://github.com/itakrisna/Aplikasi-Kasir-.NET-Visual-Studio/assets/152336076/e050dc94-89a5-4fcd-a2e8-293018a00fb0)

Form_Pesanan: 
* TampilkanDataPesanan: Menampilkan data pesanan dari database ke dalam DataGridView (dataGridView_pesanan). Dapat diakses melalui tombol "button_data_pesanan". 
* UpdateStatusToPesananSelesai: Mengubah status pesanan menjadi "Pesanan Selesai" dalam database untuk nomor WhatsApp tertentu. Dipanggil saat pesan konfirmasi pengiriman diterima oleh pelanggan. 
* Kirim Pesan Konfirmasi: Tombol "button_kirim" mengirim pesan konfirmasi kepada pelanggan bahwa pesanan mereka sudah selesai dan dapat diambil. Menggunakan Twilio API untuk mengirim pesan WhatsApp. 
* Kembali ke Form Kasir: Tombol "button_kembali" memungkinkan kembali ke Form_Kasir untuk melanjutkan operasi kasir lainnya.
* Tampilkan Data Pesanan: Tombol "button_data_pesanan" memanggil TampilkanDataPesanan untuk menampilkan data pesanan terbaru di DataGridView. 

![image](https://github.com/itakrisna/Aplikasi-Kasir-.NET-Visual-Studio/assets/152336076/940f5683-f836-4ec6-9c03-07130a2e5251)

Tampilan form login 

![image](https://github.com/itakrisna/Aplikasi-Kasir-.NET-Visual-Studio/assets/152336076/d5bf64ea-3b97-4059-913d-db015a8f73f0)

Muncul notifikasi mohon isi nama dan password apabila tidak mengisi nim danpassword 

![image](https://github.com/itakrisna/Aplikasi-Kasir-.NET-Visual-Studio/assets/152336076/be549ac6-44ad-46cf-8b14-6fb920d4f00f)

Tampilan form kasir 

![image](https://github.com/itakrisna/Aplikasi-Kasir-.NET-Visual-Studio/assets/152336076/ba3bc834-e0f5-4d13-8cfb-36d6800d25ae)

Muncul notifikasi masukan nama dan no wa apabila belum memasukan nama dan no wa 

![image](https://github.com/itakrisna/Aplikasi-Kasir-.NET-Visual-Studio/assets/152336076/94da8e44-48ae-48ff-a0b3-c013ad6337e6)

Muncul notifikasi no wa apabila belum memasukan no wa 

![image](https://github.com/itakrisna/Aplikasi-Kasir-.NET-Visual-Studio/assets/152336076/a7788d86-cd20-446e-95d8-d85c2a51e680)

Muncul notifikasi jumlah harus lebih dari 0 untuk memesan makanan 

![image](https://github.com/itakrisna/Aplikasi-Kasir-.NET-Visual-Studio/assets/152336076/2030ed51-31ab-4d55-b6fc-d5e84be4331e)

Menampilkan Pesanan di data grid view 

![image](https://github.com/itakrisna/Aplikasi-Kasir-.NET-Visual-Studio/assets/152336076/ff8bd8ad-b3c0-4a76-b05d-f588d163c946)

Bisa memesan lebih dari satu menu 

![image](https://github.com/itakrisna/Aplikasi-Kasir-.NET-Visual-Studio/assets/152336076/07b2400e-cf86-4ffd-9a8b-bf1238761d0a)

Bisa lebih dari satu menu 

![image](https://github.com/itakrisna/Aplikasi-Kasir-.NET-Visual-Studio/assets/152336076/deceec81-763d-48de-af5b-cbeddb521c27)

Muncul notifikasi pilih item yang ingin dihapus sebelum menghapus apabila belum dipilih 

![image](https://github.com/itakrisna/Aplikasi-Kasir-.NET-Visual-Studio/assets/152336076/2cd127fe-60c2-43b2-8cc4-7e2b69513365)

![image](https://github.com/itakrisna/Aplikasi-Kasir-.NET-Visual-Studio/assets/152336076/1ff7f241-9343-44a8-8000-f5d5b462f3bf)

Tampilan saat sudah dihapus 

![image](https://github.com/itakrisna/Aplikasi-Kasir-.NET-Visual-Studio/assets/152336076/eae6bf83-b30b-42c1-a789-3381e99e8cb7)

Muncul notifikasi masukan jumlah uang apabila belum mengisi jumlah uang 

![image](https://github.com/itakrisna/Aplikasi-Kasir-.NET-Visual-Studio/assets/152336076/5126dead-c77e-46d4-9ac6-8c649666480b)

Muncul notifikasi masukan jumlah kembalian sebrelum menekan tombol print bill 

![image](https://github.com/itakrisna/Aplikasi-Kasir-.NET-Visual-Studio/assets/152336076/040d10b5-876d-4639-9968-3f4d2261c5ab)

![image](https://github.com/itakrisna/Aplikasi-Kasir-.NET-Visual-Studio/assets/152336076/d473515d-8be2-44bf-b880-343e4dd7e06f)

Muncul notifikasi pesanan terkirim 

![image](https://github.com/itakrisna/Aplikasi-Kasir-.NET-Visual-Studio/assets/152336076/f54ccc1a-0fc3-452c-ae75-7ffe5a91fd28)

Notifikasi di no wa pelanggan 

![image](https://github.com/itakrisna/Aplikasi-Kasir-.NET-Visual-Studio/assets/152336076/f48007d6-d273-4589-8ebf-fda3617c2a91)

Menuju form pesanan 

![image](https://github.com/itakrisna/Aplikasi-Kasir-.NET-Visual-Studio/assets/152336076/c0c3440f-8483-418a-a7cf-8dfc51babe15)

Tampilan Form Pesanan 

![image](https://github.com/itakrisna/Aplikasi-Kasir-.NET-Visual-Studio/assets/152336076/558a1b8d-00f6-414b-a58a-a8ede7daaf0c)

Data pesanan pelanggan 

![image](https://github.com/itakrisna/Aplikasi-Kasir-.NET-Visual-Studio/assets/152336076/4422ffa1-688e-4b53-bbbf-19f8ddc458ef)

Muncul notifikasi pilih baris yang berisi nomor apabila belum memilih no pesanan 

![image](https://github.com/itakrisna/Aplikasi-Kasir-.NET-Visual-Studio/assets/152336076/6292a882-ae8a-4049-80e9-e48d021cf515)

Memilih no pesanan yang orderanya sudah jadi 

![image](https://github.com/itakrisna/Aplikasi-Kasir-.NET-Visual-Studio/assets/152336076/e49da3f2-5309-4a6c-bbed-660311692d19)

Muncul notifikasi pesan terkirim artinya pesan terkirim ke pelanggan 

![image](https://github.com/itakrisna/Aplikasi-Kasir-.NET-Visual-Studio/assets/152336076/2dbfff8e-dd3b-4a2f-b109-4d1bd72eca0d)

Muncul notifikasi pesanan di pelanggan 

![image](https://github.com/itakrisna/Aplikasi-Kasir-.NET-Visual-Studio/assets/152336076/196ebaa0-8807-41c2-af05-6d217bb4264c)

Status pesanan menjadi pesanan selesai 

![image](https://github.com/itakrisna/Aplikasi-Kasir-.NET-Visual-Studio/assets/152336076/80fae209-5043-4160-8d94-7cc91944b5c4)

Kembali ke form kasir 

![image](https://github.com/itakrisna/Aplikasi-Kasir-.NET-Visual-Studio/assets/152336076/b731f45c-f2cd-449b-8050-baeea5f5da1d)

Kembali ke form kasir 

![image](https://github.com/itakrisna/Aplikasi-Kasir-.NET-Visual-Studio/assets/152336076/0aaa97bf-c8c9-4dde-ba4d-9d8540e78c6b)

Kembali ke form login 

![image](https://github.com/itakrisna/Aplikasi-Kasir-.NET-Visual-Studio/assets/152336076/940f5683-f836-4ec6-9c03-07130a2e5251)

Form login 
