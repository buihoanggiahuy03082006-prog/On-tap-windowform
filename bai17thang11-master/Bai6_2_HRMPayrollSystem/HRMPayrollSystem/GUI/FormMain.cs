using System;
using System.Drawing;
using System.Windows.Forms;
using HRMPayrollSystem.BUS;

namespace HRMPayrollSystem.GUI
{
    public class FormMain : Form
    {
        private readonly NhanVienBUS _nhanVienBUS = new();
        private readonly PhongBanBUS _phongBanBUS = new();
        private readonly TinhLuongBUS _tinhLuongBUS = new();

        private Label lblTongNhanVien = null!;
        private Label lblTongPhongBan = null!;
        private Label lblQuyLuongThang11 = null!;
        private DataGridView dgvNhanSuPreview = null!;

        public FormMain()
        {
            InitializeComponent();
            LoadDashboardData();
        }

        private void InitializeComponent()
        {
            this.Text = "Hệ Thống Quản Lý Nhân Sự & Tính Lương (HRM & Payroll System) - 3-Tier Architecture";
            this.Size = new Size(1200, 750);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            this.BackColor = Color.FromArgb(241, 245, 249);

            // Left Sidebar
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
                Text = "🏢 HRM & PAYROLL\nQuản Lý Nhân Sự & Lương",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11.5f, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlBrand.Controls.Add(lblBrand);
            pnlSidebar.Controls.Add(pnlBrand);

            int btnY = 95;
            pnlSidebar.Controls.Add(CreateMenuButton("📝 CHẤM CÔNG THÁNG", Color.FromArgb(16, 185, 129), btnY, (s, e) => OpenForm(new FormChamCong())));
            btnY += 55;
            pnlSidebar.Controls.Add(CreateMenuButton("💰 TÍNH & CHỐT LƯƠNG", Color.FromArgb(220, 38, 38), btnY, (s, e) => OpenForm(new FormTinhLuong())));
            btnY += 55;
            pnlSidebar.Controls.Add(CreateMenuButton("👤 Hồ Sơ Nhân Viên", Color.FromArgb(30, 41, 59), btnY, (s, e) => OpenForm(new FormNhanVien())));
            btnY += 55;
            pnlSidebar.Controls.Add(CreateMenuButton("🏢 Danh Mục Phòng Ban", Color.FromArgb(30, 41, 59), btnY, (s, e) => OpenForm(new FormPhongBan())));
            btnY += 70;

            var pnlFooter = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 120,
                BackColor = Color.FromArgb(15, 23, 42)
            };
            var lblStudent = new Label
            {
                Text = "Họ tên SV: Nguyễn Minh Đức\nKiến trúc: 3-Tier (DTO-DAL-BUS)\nChấm công: Disconnected Model\nChốt lương: SqlTransaction ACID",
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
                Text = "TỔNG QUAN QUẢN TRỊ NHÂN SỰ & CHẤM CÔNG TÍNH LƯƠNG DOANH NGHIỆP",
                Font = new Font("Segoe UI", 12.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0)
            };
            pnlHeader.Controls.Add(lblHeaderTitle);

            // KPI Cards
            var pnlCards = new Panel
            {
                Location = new Point(25, 80),
                Size = new Size(880, 115)
            };

            var card1 = CreateKPICard("TỔNG NHÂN SỰ TOÀN CÔNG TY", Color.FromArgb(59, 130, 246), 0, 0, 270);
            lblTongNhanVien = CreateKPIValue("0 Người", card1);
            pnlCards.Controls.Add(card1);

            var card2 = CreateKPICard("TỔNG SỐ PHÒNG BAN", Color.FromArgb(16, 185, 129), 295, 0, 270);
            lblTongPhongBan = CreateKPIValue("0 Phòng", card2);
            pnlCards.Controls.Add(card2);

            var card3 = CreateKPICard("DỰ TOÁN QUỸ LƯƠNG THÁNG 11", Color.FromArgb(245, 158, 11), 590, 0, 270);
            lblQuyLuongThang11 = CreateKPIValue("0 VNĐ", card3);
            pnlCards.Controls.Add(card3);

            // Employee List Preview
            var grpPreview = new GroupBox
            {
                Text = "Danh sách cán bộ nhân viên công ty",
                Location = new Point(25, 215),
                Size = new Size(880, 460),
                BackColor = Color.White,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold)
            };

            dgvNhanSuPreview = new DataGridView
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
            grpPreview.Controls.Add(dgvNhanSuPreview);

            pnlContent.Controls.Add(grpPreview);
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
                Font = new Font("Segoe UI", 15f, FontStyle.Bold),
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
                var nvs = _nhanVienBUS.GetAll();
                lblTongNhanVien.Text = $"{nvs.Count} Nhân sự";

                var pbs = _phongBanBUS.GetAll();
                lblTongPhongBan.Text = $"{pbs.Count} Phòng ban";

                var luongs = _tinhLuongBUS.TinhLuongDuKien("11/2026");
                decimal tongQuyLuong = luongs.Sum(x => x.ThucLinh);
                lblQuyLuongThang11.Text = $"{tongQuyLuong:N0} VNĐ";

                dgvNhanSuPreview.DataSource = nvs;

                if (dgvNhanSuPreview.Columns["MaNV"] != null) dgvNhanSuPreview.Columns["MaNV"].HeaderText = "Mã NV";
                if (dgvNhanSuPreview.Columns["HoTen"] != null) dgvNhanSuPreview.Columns["HoTen"].HeaderText = "Họ và Tên";
                if (dgvNhanSuPreview.Columns["NgaySinh"] != null)
                {
                    dgvNhanSuPreview.Columns["NgaySinh"].HeaderText = "Ngày Sinh";
                    dgvNhanSuPreview.Columns["NgaySinh"].DefaultCellStyle.Format = "dd/MM/yyyy";
                }
                if (dgvNhanSuPreview.Columns["CCCD"] != null) dgvNhanSuPreview.Columns["CCCD"].HeaderText = "CCCD";
                if (dgvNhanSuPreview.Columns["TenPB"] != null) dgvNhanSuPreview.Columns["TenPB"].HeaderText = "Phòng Ban";
                if (dgvNhanSuPreview.Columns["LuongCoBan"] != null)
                {
                    dgvNhanSuPreview.Columns["LuongCoBan"].HeaderText = "Lương CB (VNĐ)";
                    dgvNhanSuPreview.Columns["LuongCoBan"].DefaultCellStyle.Format = "N0";
                }
                if (dgvNhanSuPreview.Columns["HeSoLuong"] != null) dgvNhanSuPreview.Columns["HeSoLuong"].HeaderText = "Hệ Số";
                if (dgvNhanSuPreview.Columns["MaPB"] != null) dgvNhanSuPreview.Columns["MaPB"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối CSDL HRMDB:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
