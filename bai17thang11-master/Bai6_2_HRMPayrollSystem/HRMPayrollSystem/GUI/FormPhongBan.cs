using System;
using System.Drawing;
using System.Windows.Forms;
using HRMPayrollSystem.BUS;
using HRMPayrollSystem.DTO;

namespace HRMPayrollSystem.GUI
{
    public class FormPhongBan : Form
    {
        private readonly PhongBanBUS _bus = new();

        private DataGridView dgvPhongBan = null!;
        private TextBox txtMaPB = null!;
        private TextBox txtTenPB = null!;
        private TextBox txtMota = null!;
        private Button btnThem = null!;
        private Button btnSua = null!;
        private Button btnXoa = null!;
        private Button btnLamMoi = null!;

        public FormPhongBan()
        {
            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            this.Text = "Quản lý Phòng Ban - HRM System (3-Tier CRUD)";
            this.Size = new Size(850, 550);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            this.BackColor = Color.FromArgb(248, 250, 252);

            var pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(15, 23, 42)
            };
            var lblTitle = new Label
            {
                Text = "DANH MỤC PHÒNG BAN CÔNG TY",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0)
            };
            pnlHeader.Controls.Add(lblTitle);

            var pnlInput = new GroupBox
            {
                Text = "Thông tin phòng ban",
                Location = new Point(20, 75),
                Size = new Size(330, 420),
                BackColor = Color.White,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold)
            };

            int y = 30;
            AddLabel(pnlInput, "Mã phòng ban (Tự động):", 20, y);
            txtMaPB = new TextBox { Location = new Point(20, y + 25), Size = new Size(290, 26), ReadOnly = true, BackColor = Color.FromArgb(241, 245, 249), Font = new Font("Segoe UI", 9f) };
            pnlInput.Controls.Add(txtMaPB);

            y += 65;
            AddLabel(pnlInput, "Tên phòng ban (*):", 20, y);
            txtTenPB = new TextBox { Location = new Point(20, y + 25), Size = new Size(290, 26), Font = new Font("Segoe UI", 9f) };
            pnlInput.Controls.Add(txtTenPB);

            y += 65;
            AddLabel(pnlInput, "Mô tả / Chức năng:", 20, y);
            txtMota = new TextBox { Location = new Point(20, y + 25), Size = new Size(290, 60), Multiline = true, Font = new Font("Segoe UI", 9f) };
            pnlInput.Controls.Add(txtMota);

            y += 105;
            btnThem = CreateButton("Thêm mới", Color.FromArgb(16, 185, 129), 20, y, 135);
            btnSua = CreateButton("Cập nhật", Color.FromArgb(59, 130, 246), 175, y, 135);
            y += 45;
            btnXoa = CreateButton("Xóa PB", Color.FromArgb(239, 68, 68), 20, y, 135);
            btnLamMoi = CreateButton("Làm mới", Color.FromArgb(107, 114, 128), 175, y, 135);

            pnlInput.Controls.AddRange(new Control[] { btnThem, btnSua, btnXoa, btnLamMoi });

            dgvPhongBan = new DataGridView
            {
                Location = new Point(370, 85),
                Size = new Size(440, 410),
                BackgroundColor = Color.White,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                AllowUserToAddRows = false
            };

            this.Controls.Add(dgvPhongBan);
            this.Controls.Add(pnlInput);
            this.Controls.Add(pnlHeader);

            btnThem.Click += BtnThem_Click;
            btnSua.Click += BtnSua_Click;
            btnXoa.Click += BtnXoa_Click;
            btnLamMoi.Click += (s, e) => ResetForm();
            dgvPhongBan.CellClick += DgvPhongBan_CellClick;
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

        private void LoadData()
        {
            dgvPhongBan.DataSource = _bus.GetAll();
            if (dgvPhongBan.Columns["MaPB"] != null) dgvPhongBan.Columns["MaPB"].HeaderText = "Mã PB";
            if (dgvPhongBan.Columns["TenPB"] != null) dgvPhongBan.Columns["TenPB"].HeaderText = "Tên Phòng Ban";
            if (dgvPhongBan.Columns["Mota"] != null) dgvPhongBan.Columns["Mota"].HeaderText = "Mô Tả";
        }

        private void DgvPhongBan_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvPhongBan.CurrentRow != null)
            {
                var row = dgvPhongBan.CurrentRow;
                txtMaPB.Text = row.Cells["MaPB"].Value?.ToString() ?? "";
                txtTenPB.Text = row.Cells["TenPB"].Value?.ToString() ?? "";
                txtMota.Text = row.Cells["Mota"].Value?.ToString() ?? "";
            }
        }

        private void BtnThem_Click(object? sender, EventArgs e)
        {
            var pb = new PhongBanDTO
            {
                TenPB = txtTenPB.Text.Trim(),
                Mota = txtMota.Text.Trim()
            };

            var (ok, msg) = _bus.Add(pb);
            MessageBox.Show(msg, ok ? "Thông báo" : "Cảnh báo", MessageBoxButtons.OK, ok ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            if (ok)
            {
                LoadData();
                ResetForm();
            }
        }

        private void BtnSua_Click(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtMaPB.Text, out int maPB))
            {
                MessageBox.Show("Vui lòng chọn phòng ban cần cập nhật!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var pb = new PhongBanDTO
            {
                MaPB = maPB,
                TenPB = txtTenPB.Text.Trim(),
                Mota = txtMota.Text.Trim()
            };

            var (ok, msg) = _bus.Update(pb);
            MessageBox.Show(msg, ok ? "Thông báo" : "Cảnh báo", MessageBoxButtons.OK, ok ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            if (ok)
            {
                LoadData();
                ResetForm();
            }
        }

        private void BtnXoa_Click(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtMaPB.Text, out int maPB))
            {
                MessageBox.Show("Vui lòng chọn phòng ban cần xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"Bạn có chắc chắn muốn xóa phòng ban #{maPB} không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var (ok, msg) = _bus.Delete(maPB);
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
            txtMaPB.Clear();
            txtTenPB.Clear();
            txtMota.Clear();
            LoadData();
        }
    }
}
