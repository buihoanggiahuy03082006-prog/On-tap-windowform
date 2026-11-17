using System;
using System.Drawing;
using System.Windows.Forms;
using HRMPayrollSystem.BUS;
using HRMPayrollSystem.DTO;

namespace HRMPayrollSystem.GUI
{
    public class FormNhanVien : Form
    {
        private readonly NhanVienBUS _nhanVienBUS = new();
        private readonly PhongBanBUS _phongBanBUS = new();

        private DataGridView dgvNhanVien = null!;
        private TextBox txtMaNV = null!;
        private TextBox txtHoTen = null!;
        private DateTimePicker dtpNgaySinh = null!;
        private TextBox txtCCCD = null!;
        private ComboBox cboPhongBan = null!;
        private NumericUpDown numLuongCoBan = null!;
        private NumericUpDown numHeSoLuong = null!;

        // Chuyển phòng ban
        private ComboBox cboChuyenPhongBan = null!;
        private Button btnChuyenPB = null!;

        // Tìm kiếm
        private TextBox txtTimKiem = null!;
        private ComboBox cboLocPhongBan = null!;
        private Button btnTimKiem = null!;

        // Buttons
        private Button btnThem = null!;
        private Button btnSua = null!;
        private Button btnXoa = null!;
        private Button btnLamMoi = null!;

        public FormNhanVien()
        {
            InitializeComponent();
            LoadPhongBanCombos();
            LoadData();
        }

        private void InitializeComponent()
        {
            this.Text = "Quản lý Hồ Sơ Nhân Sự - HRM System (3-Tier CRUD)";
            this.Size = new Size(1100, 720);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            this.BackColor = Color.FromArgb(248, 250, 252);

            // Header
            var pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 65,
                BackColor = Color.FromArgb(15, 23, 42)
            };
            var lblTitle = new Label
            {
                Text = "QUẢN LÝ HỒ SƠ NHÂN VIÊN & PHÒNG BAN (3-TIER CRUD)",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 13.5f, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0)
            };
            pnlHeader.Controls.Add(lblTitle);

            // Left Input Panel
            var pnlInput = new GroupBox
            {
                Text = "Thông tin nhân sự",
                Location = new Point(20, 80),
                Size = new Size(390, 580),
                BackColor = Color.White,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold)
            };

            int y = 25;
            AddLabel(pnlInput, "Mã nhân viên (Tự động):", 20, y);
            txtMaNV = new TextBox { Location = new Point(20, y + 22), Size = new Size(345, 26), Font = new Font("Segoe UI", 9f), ReadOnly = true, BackColor = Color.FromArgb(241, 245, 249) };
            pnlInput.Controls.Add(txtMaNV);

            y += 54;
            AddLabel(pnlInput, "Họ và tên nhân viên (*):", 20, y);
            txtHoTen = new TextBox { Location = new Point(20, y + 22), Size = new Size(345, 26), Font = new Font("Segoe UI", 9f) };
            pnlInput.Controls.Add(txtHoTen);

            y += 54;
            AddLabel(pnlInput, "Ngày sinh:", 20, y);
            dtpNgaySinh = new DateTimePicker { Location = new Point(20, y + 22), Size = new Size(345, 26), Format = DateTimePickerFormat.Short, Value = new DateTime(1995, 1, 1), Font = new Font("Segoe UI", 9f) };
            pnlInput.Controls.Add(dtpNgaySinh);

            y += 54;
            AddLabel(pnlInput, "Số CCCD / CMND (*):", 20, y);
            txtCCCD = new TextBox { Location = new Point(20, y + 22), Size = new Size(345, 26), Font = new Font("Segoe UI", 9f) };
            pnlInput.Controls.Add(txtCCCD);

            y += 54;
            AddLabel(pnlInput, "Phòng ban (*):", 20, y);
            cboPhongBan = new ComboBox { Location = new Point(20, y + 22), Size = new Size(345, 26), DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9f) };
            pnlInput.Controls.Add(cboPhongBan);

            y += 54;
            AddLabel(pnlInput, "Lương cơ bản (VNĐ) (*):", 20, y);
            numLuongCoBan = new NumericUpDown
            {
                Location = new Point(20, y + 22),
                Size = new Size(345, 26),
                Maximum = 1000000000,
                Increment = 500000,
                ThousandsSeparator = true,
                Font = new Font("Segoe UI", 9f)
            };
            pnlInput.Controls.Add(numLuongCoBan);

            y += 54;
            AddLabel(pnlInput, "Hệ số lương (*):", 20, y);
            numHeSoLuong = new NumericUpDown
            {
                Location = new Point(20, y + 22),
                Size = new Size(345, 26),
                DecimalPlaces = 2,
                Increment = 0.1m,
                Minimum = 0.1m,
                Maximum = 20m,
                Value = 1.0m,
                Font = new Font("Segoe UI", 9f)
            };
            pnlInput.Controls.Add(numHeSoLuong);

            // CRUD Action Buttons
            y += 56;
            btnThem = CreateButton("Thêm NV", Color.FromArgb(16, 185, 129), 20, y, 165);
            btnSua = CreateButton("Cập nhật", Color.FromArgb(59, 130, 246), 200, y, 165);
            y += 42;
            btnXoa = CreateButton("Xóa NV", Color.FromArgb(239, 68, 68), 20, y, 165);
            btnLamMoi = CreateButton("Làm mới", Color.FromArgb(107, 114, 128), 200, y, 165);

            pnlInput.Controls.AddRange(new Control[] { btnThem, btnSua, btnXoa, btnLamMoi });

            // Right Panel (Search, Chuyển PB, Grid)
            var pnlRight = new Panel
            {
                Location = new Point(430, 80),
                Size = new Size(645, 580)
            };

            // Search Bar
            var grpSearch = new GroupBox
            {
                Text = "Tìm kiếm nhân viên (Parameterized Query - Tên / CCCD / Phòng ban)",
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.White,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold)
            };
            txtTimKiem = new TextBox { Location = new Point(15, 30), Size = new Size(240, 26), PlaceholderText = "Nhập tên hoặc số CCCD...", Font = new Font("Segoe UI", 9f) };
            cboLocPhongBan = new ComboBox { Location = new Point(265, 30), Size = new Size(200, 26), DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9f) };
            btnTimKiem = CreateButton("Tìm kiếm", Color.FromArgb(79, 70, 229), 475, 27, 150, 32);

            grpSearch.Controls.AddRange(new Control[] { txtTimKiem, cboLocPhongBan, btnTimKiem });

            // Chuyển phòng ban Bar
            var grpTransfer = new GroupBox
            {
                Text = "Nghiệp vụ: Chuyển phòng ban nhanh cho nhân viên đang chọn",
                Location = new Point(0, 90),
                Size = new Size(645, 70),
                BackColor = Color.White,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold)
            };
            var lblChuyen = new Label { Text = "Chuyển sang:", Location = new Point(15, 30), AutoSize = true, Font = new Font("Segoe UI", 9f) };
            cboChuyenPhongBan = new ComboBox { Location = new Point(110, 27), Size = new Size(310, 26), DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9f) };
            btnChuyenPB = CreateButton("Xác nhận chuyển", Color.FromArgb(245, 158, 11), 435, 25, 190, 32);
            grpTransfer.Controls.AddRange(new Control[] { lblChuyen, cboChuyenPhongBan, btnChuyenPB });

            // Grid
            dgvNhanVien = new DataGridView
            {
                Location = new Point(0, 170),
                Size = new Size(645, 410),
                BackgroundColor = Color.White,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                AllowUserToAddRows = false
            };

            pnlRight.Controls.Add(dgvNhanVien);
            pnlRight.Controls.Add(grpTransfer);
            pnlRight.Controls.Add(grpSearch);

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
            cboLocPhongBan.SelectedIndexChanged += (s, e) => BtnTimKiem_Click(s, e);
            btnChuyenPB.Click += BtnChuyenPB_Click;
            dgvNhanVien.CellClick += DgvNhanVien_CellClick;
        }

        private void AddLabel(Control parent, string text, int x, int y)
        {
            var lbl = new Label
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true,
                Font = new Font("Segoe UI", 8.5f, FontStyle.Regular),
                ForeColor = Color.FromArgb(51, 65, 85)
            };
            parent.Controls.Add(lbl);
        }

        private Button CreateButton(string text, Color bg, int x, int y, int w, int h = 34)
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
                Font = new Font("Segoe UI", 9f, FontStyle.Bold)
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        private void LoadPhongBanCombos()
        {
            var listPB = _phongBanBUS.GetAll();
            cboPhongBan.DataSource = listPB;
            cboPhongBan.DisplayMember = "TenPB";
            cboPhongBan.ValueMember = "MaPB";

            // Chuyển PB
            cboChuyenPhongBan.DataSource = listPB.ToList();
            cboChuyenPhongBan.DisplayMember = "TenPB";
            cboChuyenPhongBan.ValueMember = "MaPB";

            // Lọc PB
            var locList = new System.Collections.Generic.List<PhongBanDTO>
            {
                new PhongBanDTO { MaPB = 0, TenPB = "-- Tất cả phòng ban --" }
            };
            locList.AddRange(listPB);
            cboLocPhongBan.DataSource = locList;
            cboLocPhongBan.DisplayMember = "TenPB";
            cboLocPhongBan.ValueMember = "MaPB";
        }

        private void LoadData()
        {
            dgvNhanVien.DataSource = _nhanVienBUS.GetAll();
            FormatGrid();
        }

        private void FormatGrid()
        {
            if (dgvNhanVien.Columns["MaNV"] != null) dgvNhanVien.Columns["MaNV"].HeaderText = "Mã NV";
            if (dgvNhanVien.Columns["HoTen"] != null) dgvNhanVien.Columns["HoTen"].HeaderText = "Họ và Tên";
            if (dgvNhanVien.Columns["NgaySinh"] != null)
            {
                dgvNhanVien.Columns["NgaySinh"].HeaderText = "Ngày Sinh";
                dgvNhanVien.Columns["NgaySinh"].DefaultCellStyle.Format = "dd/MM/yyyy";
            }
            if (dgvNhanVien.Columns["CCCD"] != null) dgvNhanVien.Columns["CCCD"].HeaderText = "CCCD";
            if (dgvNhanVien.Columns["TenPB"] != null) dgvNhanVien.Columns["TenPB"].HeaderText = "Phòng Ban";
            if (dgvNhanVien.Columns["LuongCoBan"] != null)
            {
                dgvNhanVien.Columns["LuongCoBan"].HeaderText = "Lương CB";
                dgvNhanVien.Columns["LuongCoBan"].DefaultCellStyle.Format = "N0";
            }
            if (dgvNhanVien.Columns["HeSoLuong"] != null) dgvNhanVien.Columns["HeSoLuong"].HeaderText = "Hệ Số";
            if (dgvNhanVien.Columns["MaPB"] != null) dgvNhanVien.Columns["MaPB"].Visible = false;
        }

        private void DgvNhanVien_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvNhanVien.CurrentRow != null)
            {
                var row = dgvNhanVien.CurrentRow;
                txtMaNV.Text = row.Cells["MaNV"].Value?.ToString() ?? "";
                txtHoTen.Text = row.Cells["HoTen"].Value?.ToString() ?? "";
                if (row.Cells["NgaySinh"].Value is DateTime dt) dtpNgaySinh.Value = dt;
                txtCCCD.Text = row.Cells["CCCD"].Value?.ToString() ?? "";
                cboPhongBan.SelectedValue = Convert.ToInt32(row.Cells["MaPB"].Value);
                numLuongCoBan.Value = Convert.ToDecimal(row.Cells["LuongCoBan"].Value ?? 0);
                numHeSoLuong.Value = Convert.ToDecimal(row.Cells["HeSoLuong"].Value ?? 1.0);
            }
        }

        private void BtnThem_Click(object? sender, EventArgs e)
        {
            var nv = new NhanVienDTO
            {
                HoTen = txtHoTen.Text.Trim(),
                NgaySinh = dtpNgaySinh.Value,
                CCCD = txtCCCD.Text.Trim(),
                MaPB = Convert.ToInt32(cboPhongBan.SelectedValue),
                LuongCoBan = numLuongCoBan.Value,
                HeSoLuong = (double)numHeSoLuong.Value
            };

            var (ok, msg) = _nhanVienBUS.Add(nv);
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
                HoTen = txtHoTen.Text.Trim(),
                NgaySinh = dtpNgaySinh.Value,
                CCCD = txtCCCD.Text.Trim(),
                MaPB = Convert.ToInt32(cboPhongBan.SelectedValue),
                LuongCoBan = numLuongCoBan.Value,
                HeSoLuong = (double)numHeSoLuong.Value
            };

            var (ok, msg) = _nhanVienBUS.Update(nv);
            MessageBox.Show(msg, ok ? "Thông báo" : "Cảnh báo", MessageBoxButtons.OK, ok ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            if (ok)
            {
                LoadData();
                ResetForm();
            }
        }

        private void BtnChuyenPB_Click(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtMaNV.Text, out int maNV))
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần chuyển phòng ban!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maPBMoi = Convert.ToInt32(cboChuyenPhongBan.SelectedValue);
            string tenPBMoi = cboChuyenPhongBan.Text;

            if (MessageBox.Show($"Xác nhận chuyển nhân viên [{txtHoTen.Text}] sang phòng ban [{tenPBMoi}]?", "Xác nhận chuyển phòng ban", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var (ok, msg) = _nhanVienBUS.ChuyenPhongBan(maNV, maPBMoi);
                MessageBox.Show(msg, ok ? "Thông báo" : "Lỗi", MessageBoxButtons.OK, ok ? MessageBoxIcon.Information : MessageBoxIcon.Error);
                if (ok)
                {
                    LoadData();
                }
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
                var (ok, msg) = _nhanVienBUS.Delete(maNV);
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
            int? maPB = Convert.ToInt32(cboLocPhongBan.SelectedValue);
            if (maPB == 0) maPB = null;

            dgvNhanVien.DataSource = _nhanVienBUS.Search(kw, maPB);
            FormatGrid();
        }

        private void ResetForm()
        {
            txtMaNV.Clear();
            txtHoTen.Clear();
            dtpNgaySinh.Value = new DateTime(1995, 1, 1);
            txtCCCD.Clear();
            if (cboPhongBan.Items.Count > 0) cboPhongBan.SelectedIndex = 0;
            numLuongCoBan.Value = 0;
            numHeSoLuong.Value = 1.0m;
            txtTimKiem.Clear();
            cboLocPhongBan.SelectedIndex = 0;
            LoadData();
        }
    }
}
