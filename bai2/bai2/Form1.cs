using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bai2
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

        private void btnTaiAnh_Click(object sender, EventArgs e)
        {

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Chọn ảnh lỗi";
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                picAnhLoi.Image = Image.FromFile(ofd.FileName);
                picAnhLoi.SizeMode = PictureBoxSizeMode.StretchImage;
            }

        }

        private void btnGuiYeuCau_Click(object sender, EventArgs e)
        {

            string maPhieu = txtMaPhieu.Text;
            string nguoiYeuCau = txtNguoiYeuCau.Text;
            string ngay = dtpNgayGhiNhan.Value.ToString("dd/MM/yyyy");

            string uuTien = "";
            if (rdoThap.Checked) uuTien = "Thấp";
            else if (rdoTrungBinh.Checked) uuTien = "Trung bình";
            else if (rdoKhanCap.Checked) uuTien = "Khẩn cấp";

            string loaiSuCo = cboLoaiSuCo.Text;

            string thietBi = "";
            if (chkMayTinhBan.Checked) thietBi += "Máy tính bàn, ";
            if (chkLaptop.Checked) thietBi += "Laptop, ";
            if (chkMayIn.Checked) thietBi += "Máy in, ";
            if (chkDienThoai.Checked) thietBi += "Điện thoại, ";

            if (thietBi == "")
                thietBi = "Chưa chọn";
            else
                thietBi = thietBi.TrimEnd(' ', ',');

            if (maPhieu.Trim() == "" || nguoiYeuCau.Trim() == "")
            {
                MessageBox.Show("Vui lòng nhập mã phiếu và người yêu cầu!");
                return;
            }

            if (uuTien == "" || loaiSuCo == "")
            {
                MessageBox.Show("Vui lòng chọn mức độ ưu tiên và loại sự cố!");
                return;
            }

            string thongTin =
                "Mã phiếu: " + maPhieu +
                "\nNgười yêu cầu: " + nguoiYeuCau +
                "\nNgày ghi nhận: " + ngay +
                "\nMức độ ưu tiên: " + uuTien +
                "\nLoại sự cố: " + loaiSuCo +
                "\nThiết bị ảnh hưởng: " + thietBi;

            MessageBox.Show(thongTin, "Thông tin phiếu IT");

        }

        private void btnNhapLai_Click(object sender, EventArgs e)
        {

            txtMaPhieu.Clear();
            txtNguoiYeuCau.Clear();

            dtpNgayGhiNhan.Value = DateTime.Now;

            rdoThap.Checked = false;
            rdoTrungBinh.Checked = false;
            rdoKhanCap.Checked = false;

            cboLoaiSuCo.SelectedIndex = -1;

            chkMayTinhBan.Checked = false;
            chkLaptop.Checked = false;
            chkMayIn.Checked = false;
            chkDienThoai.Checked = false;

            picAnhLoi.Image = null;

            txtMaPhieu.Focus();

        }
    }
}
