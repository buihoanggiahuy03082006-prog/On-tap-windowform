using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bài1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnTinhTien_Click(object sender, EventArgs e)
        {

            decimal donGia, giamGia;
            int soLuong;

            if (!decimal.TryParse(txtDonGia.Text, out donGia) ||
                !int.TryParse(txtSoLuong.Text, out soLuong) ||
                !decimal.TryParse(txtGiamGia.Text, out giamGia))
            {
                MessageBox.Show("Vui long nhap day du va dung du lieu!");
                return;
            }

            if (donGia < 0 || soLuong < 0 || giamGia < 0 || giamGia > 100)
            {
                MessageBox.Show("Don gia, so luong phai >= 0 va giam gia tu 0 den 100!");
                return;
            }

            decimal tongTien = donGia * soLuong * (100 - giamGia) / 100;

            lblTongTien.Text = "Tong tien: " + tongTien.ToString("N0") + " VND";

        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {

            txtDonGia.Clear();
            txtSoLuong.Clear();
            txtGiamGia.Clear();

            lblTongTien.Text = "Tong tien: 0 VND";

            txtDonGia.Focus();

        }
    }
}
