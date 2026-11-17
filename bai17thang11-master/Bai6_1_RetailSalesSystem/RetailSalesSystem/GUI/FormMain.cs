using System;
using System.Drawing;
using System.Windows.Forms;
using RetailSalesSystem.BUS;

namespace RetailSalesSystem.GUI
{
    public class FormMain : Form
    {
        private readonly ThongKeBUS _thongKeBUS = new();
        private readonly HoaDonBUS _hoaDonBUS = new();

        private Label lblDoanhThuHomNay = null!;
        private Label lblSoHoaDon = null!;
        private DataGridView dgvLichSuHoaDon = null!;

        public FormMain()
        {
            InitializeComponent();
            LoadDashboardData();
        }

        private void InitializeComponent()
        {
            this.Text = "Hệ Thống Quản Lý Bán Hàng & Siêu Thị Mini (Retail Sales System) - 3-Tier Architecture";
            this.Size = new Size(1200, 750);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            this.BackColor = Color.FromArgb(241, 245, 249);

            // Left Sidebar Menu
            var pnlSidebar = new Panel
            {
                Dock = DockStyle.Left,
                Width = 260,
                BackColor = Color.FromArgb(15, 23, 42)
            };

            var pnlBrand = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(30, 41, 59)
            };
            var lblBrand = new Label
            {
                Text = "🛒 MINI MART PRO\nRetail Sales System",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlBrand.Controls.Add(lblBrand);
            pnlSidebar.Controls.Add(pnlBrand);

            int btnY = 95;
            pnlSidebar.Controls.Add(CreateMenuButton("⚡ BÁN HÀNG & HÓA ĐƠN", Color.FromArgb(79, 70, 229), btnY, (s, e) => OpenForm(new FormBanHang())));
            btnY += 55;
            pnlSidebar.Controls.Add(CreateMenuButton("📦 Quản Lý Sản Phẩm", Color.FromArgb(30, 41, 59), btnY, (s, e) => OpenForm(new FormSanPham())));
            btnY += 55;
            pnlSidebar.Controls.Add(CreateMenuButton("👥 Quản Lý Khách Hàng", Color.FromArgb(30, 41, 59), btnY, (s, e) => OpenForm(new FormKhachHang())));
            btnY += 55;
            pnlSidebar.Controls.Add(CreateMenuButton("👔 Quản Lý Nhân Viên", Color.FromArgb(30, 41, 59), btnY, (s, e) => OpenForm(new FormNhanVien())));
            btnY += 55;
            pnlSidebar.Controls.Add(CreateMenuButton("📊 Báo Cáo - Thống Kê", Color.FromArgb(30, 41, 59), btnY, (s, e) => OpenForm(new FormBaoCao())));
            btnY += 70;

            var pnlFooter = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 120,
                BackColor = Color.FromArgb(15, 23, 42)
            };
            var lblStudent = new Label
            {
                Text = "Họ tên SV: Nguyễn Minh Đức\nKiến trúc: 3-Tier (DTO-DAL-BUS)\nGiao dịch: SqlTransaction ACID\nAn toàn: 100% SqlParameter",
                ForeColor = Color.FromArgb(148, 163, 184),
                Font = new Font("Segoe UI", 8.5f),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlFooter.Controls.Add(lblStudent);
            pnlSidebar.Controls.Add(pnlFooter);

            // Right Main Content Panel
            var pnlContent = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(25)
            };

            // Top Header Bar
            var pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.White
            };
            var lblHeaderTitle = new Label
            {
                Text = "TỔNG QUAN HỆ THỐNG BÁN HÀNG & LẬP HÓA ĐƠN SIÊU THỊ MINI",
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0)
            };
            pnlHeader.Controls.Add(lblHeaderTitle);

            // KPI Summary Cards
            var pnlCards = new Panel
            {
                Location = new Point(25, 80),
                Size = new Size(880, 115)
            };

            var card1 = CreateKPICard("DOANH THU HÔM NAY", Color.FromArgb(16, 185, 129), 0, 0, 270);
            lblDoanhThuHomNay = CreateKPIValue("0 VNĐ", card1);
            pnlCards.Controls.Add(card1);

            var card2 = CreateKPICard("TỔNG SỐ HÓA ĐƠN", Color.FromArgb(59, 130, 246), 295, 0, 270);
            lblSoHoaDon = CreateKPIValue("0", card2);
            pnlCards.Controls.Add(card2);

            var card3 = CreateKPICard("CHỨC NĂNG CHÍNH", Color.FromArgb(139, 92, 246), 590, 0, 270);
            var btnQuickSell = new Button
            {
                Text = "Mở Form Bán Hàng ➜",
                Location = new Point(15, 45),
                Size = new Size(240, 45),
                BackColor = Color.FromArgb(79, 70, 229),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10.5f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnQuickSell.FlatAppearance.BorderSize = 0;
            btnQuickSell.Click += (s, e) => OpenForm(new FormBanHang());
            card3.Controls.Add(btnQuickSell);
            pnlCards.Controls.Add(card3);

            // History Table
            var grpHistory = new GroupBox
            {
                Text = "Lịch sử hóa đơn bán hàng gần nhất",
                Location = new Point(25, 215),
                Size = new Size(880, 460),
                BackColor = Color.White,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold)
            };

            dgvLichSuHoaDon = new DataGridView
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
            dgvLichSuHoaDon.CellDoubleClick += DgvLichSuHoaDon_CellDoubleClick;
            grpHistory.Controls.Add(dgvLichSuHoaDon);

            pnlContent.Controls.Add(grpHistory);
            pnlContent.Controls.Add(pnlCards);
            pnlContent.Controls.Add(pnlHeader);

            this.Controls.Add(pnlContent);
            this.Controls.Add(pnlSidebar);
        }

        private Button CreateMenuButton(string text, Color bg, int y, EventHandler onClick)
        {
            var btn = new Button
            {
                Text = text,
                Location = new Point(15, y),
                Size = new Size(230, 44),
                BackColor = bg,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(12, 0, 0, 0),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.Click += onClick;
            return btn;
        }

        private Panel CreateKPICard(string title, Color barColor, int x, int y, int w)
        {
            var pnl = new Panel
            {
                Location = new Point(x, y),
                Size = new Size(w, 110),
                BackColor = Color.White
            };

            var bar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 5,
                BackColor = barColor
            };
            pnl.Controls.Add(bar);

            var lbl = new Label
            {
                Text = title,
                Location = new Point(15, 15),
                Size = new Size(w - 30, 20),
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 116, 139)
            };
            pnl.Controls.Add(lbl);

            return pnl;
        }

        private Label CreateKPIValue(string initVal, Panel parent)
        {
            var lbl = new Label
            {
                Text = initVal,
                Location = new Point(15, 42),
                Size = new Size(parent.Width - 30, 50),
                Font = new Font("Segoe UI", 16f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42)
            };
            parent.Controls.Add(lbl);
            return lbl;
        }

        private void OpenForm(Form f)
        {
            f.ShowDialog(this);
            LoadDashboardData();
        }

        private void LoadDashboardData()
        {
            try
            {
                decimal dt = _thongKeBUS.GetDoanhThuNgay(DateTime.Today);
                lblDoanhThuHomNay.Text = $"{dt:N0} VNĐ";

                int soHD = _thongKeBUS.GetTongSoHoaDon();
                lblSoHoaDon.Text = $"{soHD} Hóa đơn";

                var listHD = _hoaDonBUS.GetAllHoaDon();
                dgvLichSuHoaDon.DataSource = listHD;

                if (dgvLichSuHoaDon.Columns["MaHD"] != null) dgvLichSuHoaDon.Columns["MaHD"].HeaderText = "Mã HĐ";
                if (dgvLichSuHoaDon.Columns["NgayLap"] != null)
                {
                    dgvLichSuHoaDon.Columns["NgayLap"].HeaderText = "Ngày Lập";
                    dgvLichSuHoaDon.Columns["NgayLap"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                }
                if (dgvLichSuHoaDon.Columns["TenNV"] != null) dgvLichSuHoaDon.Columns["TenNV"].HeaderText = "Nhân Viên";
                if (dgvLichSuHoaDon.Columns["TenKH"] != null) dgvLichSuHoaDon.Columns["TenKH"].HeaderText = "Khách Hàng";
                if (dgvLichSuHoaDon.Columns["TongTien"] != null)
                {
                    dgvLichSuHoaDon.Columns["TongTien"].HeaderText = "Tổng Tiền (VNĐ)";
                    dgvLichSuHoaDon.Columns["TongTien"].DefaultCellStyle.Format = "N0";
                }
                if (dgvLichSuHoaDon.Columns["MaNV"] != null) dgvLichSuHoaDon.Columns["MaNV"].Visible = false;
                if (dgvLichSuHoaDon.Columns["MaKH"] != null) dgvLichSuHoaDon.Columns["MaKH"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối CSDL SalesDB:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvLichSuHoaDon_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvLichSuHoaDon.CurrentRow != null)
            {
                int maHD = Convert.ToInt32(dgvLichSuHoaDon.CurrentRow.Cells["MaHD"].Value);
                var chiTiets = _hoaDonBUS.GetChiTietByMaHD(maHD);

                string detailMsg = $"CHI TIẾT HÓA ĐƠN #{maHD}:\n\n";
                foreach (var ct in chiTiets)
                {
                    detailMsg += $"• [{ct.MaSP}] {ct.TenSP} - SL: {ct.SoLuong} x {ct.DonGia:N0} đ = {ct.ThanhTien:N0} đ\n";
                }
                detailMsg += $"\nTổng tiền: {Convert.ToDecimal(dgvLichSuHoaDon.CurrentRow.Cells["TongTien"].Value):N0} VNĐ";

                MessageBox.Show(detailMsg, $"Hóa Đơn #{maHD}", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
