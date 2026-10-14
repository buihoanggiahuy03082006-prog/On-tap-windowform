using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bai4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            cboKhungGio.Items.Clear();
            cboKhungGio.Items.Add("Sáng - 100.000đ");
            cboKhungGio.Items.Add("Tối - 150.000đ");
            cboKhungGio.SelectedIndex = 0;

            tlpViTri.Controls.Clear();
            tlpViTri.ColumnCount = 5;
            tlpViTri.RowCount = 4;
            tlpViTri.ColumnStyles.Clear();
            tlpViTri.RowStyles.Clear();

            for (int i = 0; i < 5; i++)
            {
                tlpViTri.ColumnStyles.Add(
                    new ColumnStyle(SizeType.Percent, 20F));
            }

            for (int i = 0; i < 4; i++)
            {
                tlpViTri.RowStyles.Add(
                    new RowStyle(SizeType.Percent, 25F));
            }

            for (int i = 1; i <= 20; i++)
            {
                Button btn = new Button();

                btn.Name = "btnViTri" + i;
                btn.Text = "Vị trí " + i;
                btn.Dock = DockStyle.Fill;
                btn.Margin = new Padding(5);
                btn.BackColor = System.Drawing.Color.WhiteSmoke;
                btn.Tag = "TRONG";

                btn.Click += ViTri_Click;

                int cot = (i - 1) % 5;
                int hang = (i - 1) / 5;

                tlpViTri.Controls.Add(btn, cot, hang);
            }

            lblSoViTri.Text = "Số vị trí đang chọn: 0";
            lblTamTinh.Text = "Tạm tính tiền: 0 đ";

        }


        private void ViTri_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            if (btn == null)
                return;

            if (btn.Tag.ToString() == "KHOA")
            {
                MessageBox.Show("Vị trí này đã có người đặt!");
                return;
            }

            if (btn.Tag.ToString() == "TRONG")
            {
                btn.Tag = "CHON";
                btn.BackColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                btn.Tag = "TRONG";
                btn.BackColor = System.Drawing.Color.WhiteSmoke;
            }

            CapNhatThongKe();
        }

        private void CapNhatThongKe()
        {
            int soViTri = 0;

            foreach (Control control in tlpViTri.Controls)
            {
                Button btn = control as Button;

                if (btn != null && btn.Tag.ToString() == "CHON")
                {
                    soViTri++;
                }
            }

            decimal donGia = cboKhungGio.SelectedIndex == 1
                ? 150000m
                : 100000m;

            decimal tongTien = soViTri * donGia;

            lblSoViTri.Text = "Số vị trí đang chọn: " + soViTri;
            lblTamTinh.Text = "Tạm tính tiền: " + tongTien.ToString("N0") + " đ";
        }
    }
}