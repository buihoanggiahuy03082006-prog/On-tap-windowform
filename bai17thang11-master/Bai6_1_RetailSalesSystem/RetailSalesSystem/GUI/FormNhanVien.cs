using System;
using System.Drawing;
using System.Windows.Forms;
using RetailSalesSystem.BUS;
using RetailSalesSystem.DTO;

namespace RetailSalesSystem.GUI
{
    public class FormNhanVien : Form
    {
        private readonly NhanVienBUS _bus = new();

        private DataGridView dgvNhanVien = null!;
        private TextBox txtMaNV = null!;
        private TextBox txtTenNV = null!;
        private TextBox txtSoDienThoai = null!;
        private ComboBox cboChucVu = null!;
        private Button btnThem = null!;
        private Button btnSua = null!;
        private Button btnXoa = null!;
        private Button btnLamMoi = null!;

        public FormNhanVien()
        {
            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            this.Text = "Quản lý Nhân viên - Retail Sales System (3-Tier CRUD)";
            this.Size = new Size(950, 600);
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
                Text = "QUẢN LÝ THÔNG TIN NHÂN VIÊN BÁN HÀNG",
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
                Text = "Thông tin nhân viên",
                Location = new Point(20, 75),
                Size = new Size(360, 470),
                BackColor = Color.White,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold)
            };

            int y = 35;
            AddLabel(pnlInput, "Mã nhân viên (Tự động):", 20, y);
            txtMaNV = new TextBox { Location = new Point(20, y + 25), Size = new Size(315, 28), Font = new Font("Segoe UI", 9.5f), ReadOnly = true, BackColor = Color.FromArgb(241, 245, 249) };
            pnlInput.Controls.Add(txtMaNV);

            y += 65;
            AddLabel(pnlInput, "Họ và tên nhân viên (*):", 20, y);
            txtTenNV = new TextBox { Location = new Point(20, y + 25), Size = new Size(315, 28), Font = new Font("Segoe UI", 9.5f) };
            pnlInput.Controls.Add(txtTenNV);

            y += 65;
            AddLabel(pnlInput, "Số điện thoại (*):", 20, y);
            txtSoDienThoai = new TextBox { Location = new Point(20, y + 25), Size = new Size(315, 28), Font = new Font("Segoe UI", 9.5f) };
            pnlInput.Controls.Add(txtSoDienThoai);

            y += 65;
            AddLabel(pnlInput, "Chức vụ (*):", 20, y);
            cboChucVu = new ComboBox
            {
                Location = new Point(20, y + 25),
                Size = new Size(315, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9.5f)
            };
            cboChucVu.Items.AddRange(new object[] { "Thu ngân", "Quản lý", "Bán hàng", "Thủ kho" });
            cboChucVu.SelectedIndex = 0;
            pnlInput.Controls.Add(cboChucVu);

            // Buttons
            y += 75;
            btnThem = CreateButton("Thêm mới", Color.FromArgb(16, 185, 129), 20, y, 150);
            btnSua = CreateButton("Cập nhật", Color.FromArgb(59, 130, 246), 185, y, 150);
            y += 50;
            btnXoa = CreateButton("Xóa NV", Color.FromArgb(239, 68, 68), 20, y, 150);
            btnLamMoi = CreateButton("Làm mới", Color.FromArgb(107, 114, 128), 185, y, 150);

            pnlInput.Controls.AddRange(new Control[] { btnThem, btnSua, btnXoa, btnLamMoi });

            // Right Grid Panel
            dgvNhanVien = new DataGridView
            {
                Location = new Point(400, 85),
                Size = new Size(520, 460),
                BackgroundColor = Color.White,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                AllowUserToAddRows = false
            };

            this.Controls.Add(dgvNhanVien);
            this.Controls.Add(pnlInput);
            this.Controls.Add(pnlHeader);

            // Events
            btnThem.Click += BtnThem_Click;
            btnSua.Click += BtnSua_Click;
            btnXoa.Click += BtnXoa_Click;
            btnLamMoi.Click += (s, e) => ResetForm();
            dgvNhanVien.CellClick += DgvNhanVien_CellClick;
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
            dgvNhanVien.DataSource = _bus.GetAll();
            FormatGrid();
        }

        private void FormatGrid()
        {
            if (dgvNhanVien.Columns["MaNV"] != null) dgvNhanVien.Columns["MaNV"].HeaderText = "Mã NV";
            if (dgvNhanVien.Columns["TenNV"] != null) dgvNhanVien.Columns["TenNV"].HeaderText = "Họ và Tên";
            if (dgvNhanVien.Columns["SoDienThoai"] != null) dgvNhanVien.Columns["SoDienThoai"].HeaderText = "Số Điện Thoại";
            if (dgvNhanVien.Columns["ChucVu"] != null) dgvNhanVien.Columns["ChucVu"].HeaderText = "Chức Vụ";
        }

        private void DgvNhanVien_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvNhanVien.CurrentRow != null)
            {
                var row = dgvNhanVien.CurrentRow;
                txtMaNV.Text = row.Cells["MaNV"].Value?.ToString() ?? "";
                txtTenNV.Text = row.Cells["TenNV"].Value?.ToString() ?? "";
                txtSoDienThoai.Text = row.Cells["SoDienThoai"].Value?.ToString() ?? "";
                cboChucVu.Text = row.Cells["ChucVu"].Value?.ToString() ?? "";
            }
        }

        private void BtnThem_Click(object? sender, EventArgs e)
        {
            var nv = new NhanVienDTO
            {
                TenNV = txtTenNV.Text.Trim(),
                SoDienThoai = txtSoDienThoai.Text.Trim(),
                ChucVu = cboChucVu.Text
            };

            var (ok, msg) = _bus.Add(nv);
            MessageBox.Show(msg, ok ? "Thông báo" : "Cảnh báo", MessageBoxButtons.OK, ok ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            if (ok)
            {
                LoadData();
                ResetForm();
            }
        }

        private void BtnSua_Click(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtMaNV.Text, out int maNV))
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần cập nhật!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var nv = new NhanVienDTO
            {
                MaNV = maNV,
                TenNV = txtTenNV.Text.Trim(),
                SoDienThoai = txtSoDienThoai.Text.Trim(),
                ChucVu = cboChucVu.Text
            };

            var (ok, msg) = _bus.Update(nv);
            MessageBox.Show(msg, ok ? "Thông báo" : "Cảnh báo", MessageBoxButtons.OK, ok ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            if (ok)
            {
                LoadData();
                ResetForm();
            }
        }

        private void BtnXoa_Click(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtMaNV.Text, out int maNV))
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"Bạn có chắc chắn muốn xóa nhân viên #{maNV} không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var (ok, msg) = _bus.Delete(maNV);
                MessageBox.Show(msg, ok ? "Thông báo" : "Lỗi", MessageBoxButtons.OK, ok ? MessageBoxIcon.Information : MessageBoxIcon.Error);
                if (ok)
                {
                    LoadData();
                    ResetForm();
                }
            }
        }

        private void ResetForm()
        {
            txtMaNV.Clear();
            txtTenNV.Clear();
            txtSoDienThoai.Clear();
            cboChucVu.SelectedIndex = 0;
            LoadData();
        }
    }
}
