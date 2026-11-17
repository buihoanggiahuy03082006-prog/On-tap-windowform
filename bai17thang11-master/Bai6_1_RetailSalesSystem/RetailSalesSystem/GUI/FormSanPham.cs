using System;
using System.Drawing;
using System.Windows.Forms;
using RetailSalesSystem.BUS;
using RetailSalesSystem.DTO;

namespace RetailSalesSystem.GUI
{
    public class FormSanPham : Form
    {
        private readonly SanPhamBUS _bus = new();

        private DataGridView dgvSanPham = null!;
        private TextBox txtMaSP = null!;
        private TextBox txtTenSP = null!;
        private NumericUpDown numGiaBan = null!;
        private NumericUpDown numSoLuongTon = null!;
        private TextBox txtTimKiem = null!;
        private Button btnThem = null!;
        private Button btnSua = null!;
        private Button btnXoa = null!;
        private Button btnLamMoi = null!;
        private Button btnTimKiem = null!;

        public FormSanPham()
        {
            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            this.Text = "Quản lý Sản phẩm - Retail Sales System (3-Tier CRUD)";
            this.Size = new Size(1000, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            this.BackColor = Color.FromArgb(245, 247, 250);

            // Title Header
            var pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(30, 41, 59)
            };
            var lblTitle = new Label
            {
                Text = "QUẢN LÝ DANH MỤC SẢN PHẨM / HÀNG HÓA",
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
                Text = "Thông tin sản phẩm",
                Location = new Point(20, 75),
                Size = new Size(360, 520),
                BackColor = Color.White,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold)
            };

            int y = 35;
            AddLabel(pnlInput, "Mã sản phẩm (*):", 20, y);
            txtMaSP = new TextBox { Location = new Point(20, y + 25), Size = new Size(315, 28), Font = new Font("Segoe UI", 9.5f) };
            pnlInput.Controls.Add(txtMaSP);

            y += 65;
            AddLabel(pnlInput, "Tên sản phẩm (*):", 20, y);
            txtTenSP = new TextBox { Location = new Point(20, y + 25), Size = new Size(315, 28), Font = new Font("Segoe UI", 9.5f) };
            pnlInput.Controls.Add(txtTenSP);

            y += 65;
            AddLabel(pnlInput, "Giá bán (VNĐ) (*):", 20, y);
            numGiaBan = new NumericUpDown
            {
                Location = new Point(20, y + 25),
                Size = new Size(315, 28),
                Maximum = 1000000000,
                Increment = 1000,
                ThousandsSeparator = true,
                Font = new Font("Segoe UI", 9.5f)
            };
            pnlInput.Controls.Add(numGiaBan);

            y += 65;
            AddLabel(pnlInput, "Số lượng tồn kho (*):", 20, y);
            numSoLuongTon = new NumericUpDown
            {
                Location = new Point(20, y + 25),
                Size = new Size(315, 28),
                Maximum = 100000,
                Font = new Font("Segoe UI", 9.5f)
            };
            pnlInput.Controls.Add(numSoLuongTon);

            // Action Buttons
            y += 80;
            btnThem = CreateButton("Thêm mới", Color.FromArgb(16, 185, 129), 20, y, 150);
            btnSua = CreateButton("Cập nhật", Color.FromArgb(59, 130, 246), 185, y, 150);
            y += 50;
            btnXoa = CreateButton("Xóa SP", Color.FromArgb(239, 68, 68), 20, y, 150);
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
                Text = "Tìm kiếm (Chống SQL Injection bằng SqlParameter)",
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
                PlaceholderText = "Nhập mã hoặc tên sản phẩm..."
            };
            btnTimKiem = CreateButton("Tìm kiếm", Color.FromArgb(79, 70, 229), 415, 28, 135, 34);

            grpSearch.Controls.Add(txtTimKiem);
            grpSearch.Controls.Add(btnTimKiem);

            dgvSanPham = new DataGridView
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
            pnlRight.Controls.Add(dgvSanPham);

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
            dgvSanPham.CellClick += DgvSanPham_CellClick;
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
            var list = _bus.GetAll();
            dgvSanPham.DataSource = list;
            FormatGrid();
        }

        private void FormatGrid()
        {
            if (dgvSanPham.Columns["MaSP"] != null) dgvSanPham.Columns["MaSP"].HeaderText = "Mã SP";
            if (dgvSanPham.Columns["TenSP"] != null) dgvSanPham.Columns["TenSP"].HeaderText = "Tên Sản Phẩm";
            if (dgvSanPham.Columns["GiaBan"] != null)
            {
                dgvSanPham.Columns["GiaBan"].HeaderText = "Giá Bán (VNĐ)";
                dgvSanPham.Columns["GiaBan"].DefaultCellStyle.Format = "N0";
            }
            if (dgvSanPham.Columns["SoLuongTon"] != null)
            {
                dgvSanPham.Columns["SoLuongTon"].HeaderText = "Tồn Kho";
            }
        }

        private void DgvSanPham_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvSanPham.CurrentRow != null)
            {
                var row = dgvSanPham.CurrentRow;
                txtMaSP.Text = row.Cells["MaSP"].Value?.ToString() ?? "";
                txtMaSP.ReadOnly = true; // Không sửa mã PK
                txtTenSP.Text = row.Cells["TenSP"].Value?.ToString() ?? "";
                numGiaBan.Value = Convert.ToDecimal(row.Cells["GiaBan"].Value ?? 0);
                numSoLuongTon.Value = Convert.ToInt32(row.Cells["SoLuongTon"].Value ?? 0);
            }
        }

        private void BtnThem_Click(object? sender, EventArgs e)
        {
            var sp = new SanPhamDTO
            {
                MaSP = txtMaSP.Text.Trim().ToUpper(),
                TenSP = txtTenSP.Text.Trim(),
                GiaBan = numGiaBan.Value,
                SoLuongTon = (int)numSoLuongTon.Value
            };

            var (ok, msg) = _bus.Add(sp);
            MessageBox.Show(msg, ok ? "Thông báo" : "Cảnh báo", MessageBoxButtons.OK, ok ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            if (ok)
            {
                LoadData();
                ResetForm();
            }
        }

        private void BtnSua_Click(object? sender, EventArgs e)
        {
            var sp = new SanPhamDTO
            {
                MaSP = txtMaSP.Text.Trim(),
                TenSP = txtTenSP.Text.Trim(),
                GiaBan = numGiaBan.Value,
                SoLuongTon = (int)numSoLuongTon.Value
            };

            var (ok, msg) = _bus.Update(sp);
            MessageBox.Show(msg, ok ? "Thông báo" : "Cảnh báo", MessageBoxButtons.OK, ok ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            if (ok)
            {
                LoadData();
                ResetForm();
            }
        }

        private void BtnXoa_Click(object? sender, EventArgs e)
        {
            string maSP = txtMaSP.Text.Trim();
            if (string.IsNullOrEmpty(maSP))
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"Bạn có chắc chắn muốn xóa sản phẩm [{maSP}] không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var (ok, msg) = _bus.Delete(maSP);
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
            dgvSanPham.DataSource = _bus.Search(kw);
            FormatGrid();
        }

        private void ResetForm()
        {
            txtMaSP.Clear();
            txtMaSP.ReadOnly = false;
            txtTenSP.Clear();
            numGiaBan.Value = 0;
            numSoLuongTon.Value = 0;
            txtTimKiem.Clear();
            LoadData();
        }
    }
}
