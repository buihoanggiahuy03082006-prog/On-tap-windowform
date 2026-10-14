using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bai3
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

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void btnThem_Click(object sender, EventArgs e)
        {

            string maVT = txtMaVT.Text.Trim();
            string tenVT = txtTenVT.Text.Trim();
            string donVi = cboDonVi.Text;
            decimal donGia;

            if (maVT == "" || tenVT == "" || donVi == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            if (!decimal.TryParse(txtDonGia.Text, out donGia) || donGia < 0)
            {
                MessageBox.Show("Đơn giá phải là số không âm!");
                return;
            }

            foreach (ListViewItem item in lvVatTu.Items)
            {
                if (item.SubItems[0].Text.Equals(
                    maVT, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Mã vật tư đã tồn tại!");
                    return;
                }
            }

            ListViewItem dong = new ListViewItem(maVT);
            dong.SubItems.Add(tenVT);
            dong.SubItems.Add(donVi);
            dong.SubItems.Add(donGia.ToString("N0"));

            lvVatTu.Items.Add(dong);

            txtMaVT.Clear();
            txtTenVT.Clear();
            cboDonVi.SelectedIndex = -1;
            txtDonGia.Clear();
            txtMaVT.Focus();

        }

        private void lvVatTu_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (lvVatTu.SelectedItems.Count > 0)
            {
                ListViewItem dong = lvVatTu.SelectedItems[0];

                txtMaVT.Text = dong.SubItems[0].Text;
                txtTenVT.Text = dong.SubItems[1].Text;
                cboDonVi.Text = dong.SubItems[2].Text;
                txtDonGia.Text = dong.SubItems[3].Text;
            }

        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {

            if (lvVatTu.SelectedItems.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn vật tư cần cập nhật!");
                return;
            }

            string maVT = txtMaVT.Text.Trim();
            string tenVT = txtTenVT.Text.Trim();
            string donVi = cboDonVi.Text;
            decimal donGia;

            if (maVT == "" || tenVT == "" || donVi == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            if (!decimal.TryParse(txtDonGia.Text, out donGia) || donGia < 0)
            {
                MessageBox.Show("Đơn giá phải là số không âm!");
                return;
            }

            ListViewItem dong = lvVatTu.SelectedItems[0];

            foreach (ListViewItem item in lvVatTu.Items)
            {
                if (item != dong &&
                    item.SubItems[0].Text.Equals(
                        maVT, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Mã vật tư đã tồn tại!");
                    return;
                }
            }

            dong.SubItems[0].Text = maVT;
            dong.SubItems[1].Text = tenVT;
            dong.SubItems[2].Text = donVi;
            dong.SubItems[3].Text = donGia.ToString("N0");

            MessageBox.Show("Cập nhật thành công!");

        }

        private void btnXoaDong_Click(object sender, EventArgs e)
        {

            if (lvVatTu.SelectedItems.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn dòng cần xóa!");
                return;
            }

            DialogResult ketQua = MessageBox.Show(
                "Bạn có chắc muốn xóa dòng đã chọn?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (ketQua == DialogResult.Yes)
            {
                lvVatTu.Items.Remove(lvVatTu.SelectedItems[0]);

                txtMaVT.Clear();
                txtTenVT.Clear();
                cboDonVi.SelectedIndex = -1;
                txtDonGia.Clear();

                MessageBox.Show("Đã xóa dòng!");
            }

        }

        private void btnXoaTatCa_Click(object sender, EventArgs e)
        {

            if (lvVatTu.Items.Count == 0)
            {
                MessageBox.Show("Danh sách đang trống!");
                return;
            }

            DialogResult ketQua = MessageBox.Show(
                "Bạn có chắc muốn xóa toàn bộ vật tư?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (ketQua == DialogResult.Yes)
            {
                lvVatTu.Items.Clear();

                txtMaVT.Clear();
                txtTenVT.Clear();
                cboDonVi.SelectedIndex = -1;
                txtDonGia.Clear();

                MessageBox.Show("Đã xóa toàn bộ danh sách!");
            }

        }
    }
}
