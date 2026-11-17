using System;
using System.Drawing;
using System.Windows.Forms;
using RetailSalesSystem.BUS;
using RetailSalesSystem.DTO;

namespace RetailSalesSystem.GUI
{
    public class FormKhachHang : Form
    {
        private readonly KhachHangBUS _bus = new();

        private DataGridView dgvKhachHang = null!;
        private TextBox txtMaKH = null!;
        private TextBox txtTenKH = null!;
        private TextBox txtDienThoai = null!;
        private NumericUpDown numDiemTichLuy = null!;
        private TextBox txtTimKiem = null!;
        private Button btnThem = null!;
        private Button btnSua = null!;
        private Button btnXoa = null!;
        private Button btnLamMoi = null!;
        private Button btnTimKiem = null!;

        public FormKhachHang()
        {
            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            this.Text = "Quản lý Khách hàng - Retail Sales System (3-Tier CRUD)";
            this.Size = new Size(1000, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            this.BackColor = Color.FromArgb(245, 247, 250);

            // Title Header
            var pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(15, 23, 42)
            };
            var lblTitle = new Label
            {
                Text = "QUẢN LÝ THÔNG TIN KHÁCH HÀNG & ĐIỂM TÍCH LŨY",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0)
            };
            pnlHeader.Controls.Add(lblTitle);

            // Left Input Panel
            var pnlInput = new GroupBox
            {
                Text = "Thông tin khách hàng",
                Location = new Point(20, 75),
                Size = new Size(360, 520),
                BackColor = Color.White,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold)
            };

            int y = 35;
            AddLabel(pnlInput, "Mã khách hàng (Tự động):", 20, y);
            txtMaKH = new TextBox { Location = new Point(20, y + 25), Size = new Size(315, 28), Font = new Font("Segoe UI", 9.5f), ReadOnly = true, BackColor = Color.FromArgb(241, 245, 249) };
            pnlInput.Controls.Add(txtMaKH);

            y += 65;
            AddLabel(pnlInput, "Họ và tên khách hàng (*):", 20, y);
            txtTenKH = new TextBox { Location = new Point(20, y + 25), Size = new Size(315, 28), Font = new Font("Segoe UI", 9.5f) };
            pnlInput.Controls.Add(txtTenKH);

            y += 65;
            AddLabel(pnlInput, "Số điện thoại (*):", 20, y);
            txtDienThoai = new TextBox { Location = new Point(20, y + 25), Size = new Size(315, 28), Font = new Font("Segoe UI", 9.5f) };
            pnlInput.Controls.Add(txtDienThoai);

            y += 65;
            AddLabel(pnlInput, "Điểm tích lũy:", 20, y);
            numDiemTichLuy = new NumericUpDown
            {
                Location = new Point(20, y + 25),
                Size = new Size(315, 28),
                Maximum = 1000000,
                Font = new Font("Segoe UI", 9.5f)
            };
            pnlInput.Controls.Add(numDiemTichLuy);

            // Buttons
            y += 80;
            btnThem = CreateButton("Thêm mới", Color.FromArgb(16, 185, 129), 20, y, 150);
            btnSua = CreateButton("Cập nhật", Color.FromArgb(59, 130, 246), 185, y, 150);
            y += 50;
            btnXoa = CreateButton("Xóa KH", Color.FromArgb(239, 68, 68), 20, y, 150);
            btnLamMoi = CreateButton("Làm mới", Color.FromArgb(107, 114, 128), 185, y, 150);

            pnlInput.Controls.AddRange(new Control[] { btnThem, btnSua, btnXoa, btnLamMoi });

            // Right Panel (Search & Grid)
            var pnlRight = new Panel
            {
                Location = new Point(400, 75),
                Size = new Size(570, 520)
            };

            var grpSearch = new GroupBox
            {
                Text = "Tìm kiếm khách hàng (SqlParameter %)",
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.White,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold)
            };
            txtTimKiem = new TextBox
            {
                Location = new Point(15, 32),
                Size = new Size(390, 28),
                Font = new Font("Segoe UI", 9.5f),
                PlaceholderText = "Nhập tên hoặc số điện thoại..."
            };
            btnTimKiem = CreateButton("Tìm kiếm", Color.FromArgb(79, 70, 229), 415, 28, 135, 34);

            grpSearch.Controls.Add(txtTimKiem);
            grpSearch.Controls.Add(btnTimKiem);

            dgvKhachHang = new DataGridView
            {
                Dock = DockStyle.Bottom,
                Height = 425,
                BackgroundColor = Color.White,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                AllowUserToAddRows = false
            };

            pnlRight.Controls.Add(grpSearch);
            pnlRight.Controls.Add(dgvKhachHang);

            this.Controls.Add(pnlRight);
            this.Controls.Add(pnlInput);
            this.Controls.Add(pnlHeader);

            // Events
            btnThem.Click += BtnThem_Click;
            btnSua.Click += BtnSua_Click;
            btnXoa.Click += BtnXoa_Click;
            btnLamMoi.Click += (s, e) => ResetForm();
            btnTimKiem.Click += BtnTimKiem_Click;
            txtTimKiem.TextChanged += (s, e) => BtnTimKiem_Click(s, e);
            dgvKhachHang.CellClick += DgvKhachHang_CellClick;
        }

        private void AddLabel(Control parent, string text, int x, int y)
        {
            var lbl = new Label
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true,
                Font = new Font("Segoe UI", 9f, FontStyle.Regular),
                ForeColor = Color.FromArgb(51, 65, 85)
            };
            parent.Controls.Add(lbl);
        }

        private Button CreateButton(string text, Color bg, int x, int y, int w, int h = 38)
        {
            var btn = new Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(w, h),
                BackColor = bg,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold)
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        private void LoadData()
        {
            dgvKhachHang.DataSource = _bus.GetAll();
            FormatGrid();
        }

        private void FormatGrid()
        {
            if (dgvKhachHang.Columns["MaKH"] != null) dgvKhachHang.Columns["MaKH"].HeaderText = "Mã KH";
            if (dgvKhachHang.Columns["TenKH"] != null) dgvKhachHang.Columns["TenKH"].HeaderText = "Họ và Tên";
            if (dgvKhachHang.Columns["DienThoai"] != null) dgvKhachHang.Columns["DienThoai"].HeaderText = "Số Điện Thoại";
            if (dgvKhachHang.Columns["DiemTichLuy"] != null) dgvKhachHang.Columns["DiemTichLuy"].HeaderText = "Điểm Tích Lũy";
        }

        private void DgvKhachHang_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvKhachHang.CurrentRow != null)
            {
                var row = dgvKhachHang.CurrentRow;
                txtMaKH.Text = row.Cells["MaKH"].Value?.ToString() ?? "";
                txtTenKH.Text = row.Cells["TenKH"].Value?.ToString() ?? "";
                txtDienThoai.Text = row.Cells["DienThoai"].Value?.ToString() ?? "";
                numDiemTichLuy.Value = Convert.ToDecimal(row.Cells["DiemTichLuy"].Value ?? 0);
            }
        }

        private void BtnThem_Click(object? sender, EventArgs e)
        {
            var kh = new KhachHangDTO
            {
                TenKH = txtTenKH.Text.Trim(),
                DienThoai = txtDienThoai.Text.Trim(),
                DiemTichLuy = (int)numDiemTichLuy.Value
            };

            var (ok, msg) = _bus.Add(kh);
            MessageBox.Show(msg, ok ? "Thông báo" : "Cảnh báo", MessageBoxButtons.OK, ok ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            if (ok)
            {
                LoadData();
                ResetForm();
            }
        }

        private void BtnSua_Click(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtMaKH.Text, out int maKH))
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần cập nhật!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var kh = new KhachHangDTO
            {
                MaKH = maKH,
                TenKH = txtTenKH.Text.Trim(),
                DienThoai = txtDienThoai.Text.Trim(),
                DiemTichLuy = (int)numDiemTichLuy.Value
            };

            var (ok, msg) = _bus.Update(kh);
            MessageBox.Show(msg, ok ? "Thông báo" : "Cảnh báo", MessageBoxButtons.OK, ok ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            if (ok)
            {
                LoadData();
                ResetForm();
            }
        }

        private void BtnXoa_Click(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtMaKH.Text, out int maKH))
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"Bạn có chắc chắn muốn xóa khách hàng #{maKH} không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var (ok, msg) = _bus.Delete(maKH);
                MessageBox.Show(msg, ok ? "Thông báo" : "Lỗi", MessageBoxButtons.OK, ok ? MessageBoxIcon.Information : MessageBoxIcon.Error);
                if (ok)
                {
                    LoadData();
                    ResetForm();
                }
            }
        }

        private void BtnTimKiem_Click(object? sender, EventArgs e)
        {
            string kw = txtTimKiem.Text.Trim();
            dgvKhachHang.DataSource = _bus.Search(kw);
            FormatGrid();
        }

        private void ResetForm()
        {
            txtMaKH.Clear();
            txtTenKH.Clear();
            txtDienThoai.Clear();
            numDiemTichLuy.Value = 0;
            txtTimKiem.Clear();
            LoadData();
        }
    }
}
