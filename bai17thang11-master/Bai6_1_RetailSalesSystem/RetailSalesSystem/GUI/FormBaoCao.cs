using System;
using System.Drawing;
using System.Windows.Forms;
using RetailSalesSystem.BUS;

namespace RetailSalesSystem.GUI
{
    public class FormBaoCao : Form
    {
        private readonly ThongKeBUS _thongKeBUS = new();

        private Label lblDoanhThuHomNay = null!;
        private Label lblDoanhThuThang = null!;
        private Label lblTongSoHoaDon = null!;
        private DateTimePicker dtpChonNgay = null!;
        private NumericUpDown numThang = null!;
        private NumericUpDown numNam = null!;
        private Button btnXemThang = null!;
        private DataGridView dgvTopSanPham = null!;

        public FormBaoCao()
        {
            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            this.Text = "Báo Cáo - Thống Kê Doanh Thu (ExecuteScalar & SqlDataReader)";
            this.Size = new Size(1000, 680);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            this.BackColor = Color.FromArgb(248, 250, 252);

            // Title Header
            var pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 65,
                BackColor = Color.FromArgb(15, 23, 42)
            };
            var lblTitle = new Label
            {
                Text = "BÁO CÁO THỐNG KÊ DOANH THU & TOP SẢN PHẨM BÁN CHẠY",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 13.5f, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0)
            };
            pnlHeader.Controls.Add(lblTitle);

            // Row 1: KPI Cards (ExecuteScalar)
            var pnlCards = new Panel
            {
                Location = new Point(20, 80),
                Size = new Size(940, 130)
            };

            // Card 1: Doanh thu hôm nay
            var card1 = CreateCard("DOANH THU HÔM NAY (ExecuteScalar)", Color.FromArgb(59, 130, 246), 0, 0, 300);
            lblDoanhThuHomNay = CreateCardValue("0 VNĐ", card1);
            pnlCards.Controls.Add(card1);

            // Card 2: Doanh thu theo tháng
            var card2 = CreateCard("DOANH THU THÁNG NÀY (ExecuteScalar)", Color.FromArgb(16, 185, 129), 320, 0, 300);
            lblDoanhThuThang = CreateCardValue("0 VNĐ", card2);
            pnlCards.Controls.Add(card2);

            // Card 3: Tổng số hóa đơn
            var card3 = CreateCard("TỔNG HÓA ĐƠN ĐÃ LẬP (ExecuteScalar)", Color.FromArgb(245, 158, 11), 640, 0, 300);
            lblTongSoHoaDon = CreateCardValue("0", card3);
            pnlCards.Controls.Add(card3);

            // Filter Section: Tra cứu doanh thu theo ngày & tháng tùy chọn
            var grpFilter = new GroupBox
            {
                Text = "Tùy chọn tra cứu doanh thu thời gian cụ thể (ExecuteScalar)",
                Location = new Point(20, 225),
                Size = new Size(940, 85),
                BackColor = Color.White,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold)
            };

            var lblNgay = new Label { Text = "Chọn ngày:", Location = new Point(20, 35), AutoSize = true, Font = new Font("Segoe UI", 9f) };
            dtpChonNgay = new DateTimePicker { Location = new Point(95, 30), Size = new Size(150, 28), Format = DateTimePickerFormat.Short, Value = DateTime.Now };
            dtpChonNgay.ValueChanged += (s, e) =>
            {
                decimal dt = _thongKeBUS.GetDoanhThuNgay(dtpChonNgay.Value);
                MessageBox.Show($"Doanh thu ngày {dtpChonNgay.Value:dd/MM/yyyy} là: {dt:N0} VNĐ (Tính qua ExecuteScalar)", "Kết quả truy vấn", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            var lblThang = new Label { Text = "Tháng:", Location = new Point(320, 35), AutoSize = true, Font = new Font("Segoe UI", 9f) };
            numThang = new NumericUpDown { Location = new Point(370, 30), Size = new Size(60, 28), Minimum = 1, Maximum = 12, Value = DateTime.Now.Month };

            var lblNam = new Label { Text = "Năm:", Location = new Point(450, 35), AutoSize = true, Font = new Font("Segoe UI", 9f) };
            numNam = new NumericUpDown { Location = new Point(490, 30), Size = new Size(80, 28), Minimum = 2000, Maximum = 2099, Value = DateTime.Now.Year };

            btnXemThang = new Button
            {
                Text = "Tra Cứu Doanh Thu Tháng",
                Location = new Point(590, 28),
                Size = new Size(200, 32),
                BackColor = Color.FromArgb(79, 70, 229),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnXemThang.FlatAppearance.BorderSize = 0;
            btnXemThang.Click += (s, e) =>
            {
                decimal dt = _thongKeBUS.GetDoanhThuThang((int)numThang.Value, (int)numNam.Value);
                lblDoanhThuThang.Text = $"{dt:N0} VNĐ";
                MessageBox.Show($"Tổng doanh thu tháng {numThang.Value}/{numNam.Value} là: {dt:N0} VNĐ (Tính qua ExecuteScalar)", "Kết quả", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            grpFilter.Controls.AddRange(new Control[] { lblNgay, dtpChonNgay, lblThang, numThang, lblNam, numNam, btnXemThang });

            // Top 5 Products (SqlDataReader)
            var grpTop5 = new GroupBox
            {
                Text = "Top 5 Sản phẩm bán chạy nhất mọi thời đại (Truy vấn tối ưu qua SqlDataReader)",
                Location = new Point(20, 325),
                Size = new Size(940, 300),
                BackColor = Color.White,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold)
            };

            dgvTopSanPham = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                AllowUserToAddRows = false
            };
            grpTop5.Controls.Add(dgvTopSanPham);

            this.Controls.Add(grpTop5);
            this.Controls.Add(grpFilter);
            this.Controls.Add(pnlCards);
            this.Controls.Add(pnlHeader);
        }

        private Panel CreateCard(string title, Color barColor, int x, int y, int w)
        {
            var pnl = new Panel
            {
                Location = new Point(x, y),
                Size = new Size(w, 115),
                BackColor = Color.White
            };

            var bar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 6,
                BackColor = barColor
            };
            pnl.Controls.Add(bar);

            var lbl = new Label
            {
                Text = title,
                Location = new Point(15, 18),
                Size = new Size(w - 30, 22),
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 116, 139)
            };
            pnl.Controls.Add(lbl);

            return pnl;
        }

        private Label CreateCardValue(string initVal, Panel parent)
        {
            var lbl = new Label
            {
                Text = initVal,
                Location = new Point(15, 45),
                Size = new Size(parent.Width - 30, 45),
                Font = new Font("Segoe UI", 16f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42)
            };
            parent.Controls.Add(lbl);
            return lbl;
        }

        private void LoadData()
        {
            // 1. ExecuteScalar: Doanh thu hôm nay
            decimal dtHomNay = _thongKeBUS.GetDoanhThuNgay(DateTime.Today);
            lblDoanhThuHomNay.Text = $"{dtHomNay:N0} VNĐ";

            // 2. ExecuteScalar: Doanh thu tháng hiện tại
            decimal dtThang = _thongKeBUS.GetDoanhThuThang(DateTime.Today.Month, DateTime.Today.Year);
            lblDoanhThuThang.Text = $"{dtThang:N0} VNĐ";

            // 3. ExecuteScalar: Tổng số hóa đơn
            int soHD = _thongKeBUS.GetTongSoHoaDon();
            lblTongSoHoaDon.Text = $"{soHD} Hóa đơn";

            // 4. SqlDataReader: Top 5 sản phẩm
            var top5 = _thongKeBUS.GetTop5SanPhamBanChay();
            dgvTopSanPham.DataSource = top5;

            if (dgvTopSanPham.Columns["MaSP"] != null) dgvTopSanPham.Columns["MaSP"].HeaderText = "Mã Sản Phẩm";
            if (dgvTopSanPham.Columns["TenSP"] != null) dgvTopSanPham.Columns["TenSP"].HeaderText = "Tên Sản Phẩm";
            if (dgvTopSanPham.Columns["TongSoLuongBan"] != null) dgvTopSanPham.Columns["TongSoLuongBan"].HeaderText = "Tổng SL Đã Bán";
            if (dgvTopSanPham.Columns["TongDoanhThu"] != null)
            {
                dgvTopSanPham.Columns["TongDoanhThu"].HeaderText = "Tổng Doanh Thu (VNĐ)";
                dgvTopSanPham.Columns["TongDoanhThu"].DefaultCellStyle.Format = "N0";
            }
        }
    }
}
