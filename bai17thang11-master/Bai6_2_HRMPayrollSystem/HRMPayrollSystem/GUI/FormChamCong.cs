using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using HRMPayrollSystem.BUS;

namespace HRMPayrollSystem.GUI
{
    public class FormChamCong : Form
    {
        private readonly ChamCongBUS _chamCongBUS = new();

        private SqlDataAdapter? _adapter;
        private DataTable? _table;
        private SqlConnection? _connection;

        private NumericUpDown numThang = null!;
        private NumericUpDown numNam = null!;
        private Button btnTaiDuLieu = null!;
        private Button btnLuuChamCong = null!;
        private Label lblTrangThai = null!;
        private DataGridView dgvChamCong = null!;

        public FormChamCong()
        {
            InitializeComponent();
            TaiDuLieuChamCong();
        }

        private void InitializeComponent()
        {
            this.Text = "Chấm Công Ngắt Kết Nối - HRM System (Disconnected Model: SqlDataAdapter & DataTable)";
            this.Size = new Size(1100, 680);
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
                Text = "BẢNG CHẤM CÔNG THÁNG (DISCONNECTED MODEL: SqlDataAdapter & DataTable)",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 13.5f, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0)
            };
            pnlHeader.Controls.Add(lblTitle);

            // Filter & Action Toolbar
            var pnlToolbar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 85,
                BackColor = Color.White,
                Padding = new Padding(20, 15, 20, 15)
            };

            var lblThang = new Label { Text = "Tháng:", Location = new Point(20, 30), AutoSize = true, Font = new Font("Segoe UI", 10f, FontStyle.Bold) };
            numThang = new NumericUpDown { Location = new Point(80, 26), Size = new Size(60, 28), Minimum = 1, Maximum = 12, Value = 11, Font = new Font("Segoe UI", 10f) };

            var lblNam = new Label { Text = "Năm:", Location = new Point(160, 30), AutoSize = true, Font = new Font("Segoe UI", 10f, FontStyle.Bold) };
            numNam = new NumericUpDown { Location = new Point(210, 26), Size = new Size(80, 28), Minimum = 2020, Maximum = 2035, Value = 2026, Font = new Font("Segoe UI", 10f) };

            btnTaiDuLieu = new Button
            {
                Text = "🔄 Nạp Dữ Liệu (Fill DataTable)",
                Location = new Point(310, 24),
                Size = new Size(230, 34),
                BackColor = Color.FromArgb(59, 130, 246),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnTaiDuLieu.FlatAppearance.BorderSize = 0;
            btnTaiDuLieu.Click += (s, e) => TaiDuLieuChamCong();

            btnLuuChamCong = new Button
            {
                Text = "💾 Lưu Chấm Công (adapter.Update)",
                Location = new Point(560, 24),
                Size = new Size(270, 34),
                BackColor = Color.FromArgb(16, 185, 129),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnLuuChamCong.FlatAppearance.BorderSize = 0;
            btnLuuChamCong.Click += BtnLuuChamCong_Click;

            lblTrangThai = new Label
            {
                Text = "Trạng thái: Sẵn sàng nhập liệu",
                Location = new Point(850, 30),
                AutoSize = true,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(5, 150, 105)
            };

            pnlToolbar.Controls.AddRange(new Control[] { lblThang, numThang, lblNam, numNam, btnTaiDuLieu, btnLuuChamCong, lblTrangThai });

            // Help Banner
            var pnlNotice = new Panel
            {
                Dock = DockStyle.Top,
                Height = 45,
                BackColor = Color.FromArgb(238, 242, 255),
                Padding = new Padding(20, 10, 20, 10)
            };
            var lblNotice = new Label
            {
                Text = "💡 Hướng dẫn: Bạn có thể nhập/sửa trực tiếp 'Số Ngày Công' (tối đa 31) và 'Số Giờ OT' trên lưới. Sau đó bấm [Lưu Chấm Công] để đồng bộ về CSDL trong 1 thao tác.",
                ForeColor = Color.FromArgb(67, 56, 202),
                Font = new Font("Segoe UI", 9f, FontStyle.Italic),
                Dock = DockStyle.Fill
            };
            pnlNotice.Controls.Add(lblNotice);

            // DataGridView for Disconnected Model
            dgvChamCong = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                SelectionMode = DataGridViewSelectionMode.CellSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                Font = new Font("Segoe UI", 9.5f)
            };

            this.Controls.Add(dgvChamCong);
            this.Controls.Add(pnlNotice);
            this.Controls.Add(pnlToolbar);
            this.Controls.Add(pnlHeader);
        }

        private string GetThangNam() => $"{(int)numThang.Value:D2}/{numNam.Value}";

        private void TaiDuLieuChamCong()
        {
            string thangNam = GetThangNam();

            // Đóng connection cũ nếu có
            _connection?.Dispose();

            // Nạp dữ liệu vào DataTable ngắt kết nối
            var (adapter, table, conn) = _chamCongBUS.GetChamCongDisconnected(thangNam);
            _adapter = adapter;
            _table = table;
            _connection = conn;

            dgvChamCong.DataSource = _table;

            // Cấu hình các cột hiển thị
            if (dgvChamCong.Columns["MaCong"] != null)
            {
                dgvChamCong.Columns["MaCong"].HeaderText = "Mã Công";
                dgvChamCong.Columns["MaCong"].ReadOnly = true;
                dgvChamCong.Columns["MaCong"].DefaultCellStyle.BackColor = Color.FromArgb(241, 245, 249);
            }
            if (dgvChamCong.Columns["MaNV"] != null)
            {
                dgvChamCong.Columns["MaNV"].HeaderText = "Mã NV";
                dgvChamCong.Columns["MaNV"].ReadOnly = true;
                dgvChamCong.Columns["MaNV"].DefaultCellStyle.BackColor = Color.FromArgb(241, 245, 249);
            }
            if (dgvChamCong.Columns["HoTen"] != null)
            {
                dgvChamCong.Columns["HoTen"].HeaderText = "Họ và Tên Nhân Viên";
                dgvChamCong.Columns["HoTen"].ReadOnly = true;
                dgvChamCong.Columns["HoTen"].DefaultCellStyle.BackColor = Color.FromArgb(241, 245, 249);
            }
            if (dgvChamCong.Columns["TenPB"] != null)
            {
                dgvChamCong.Columns["TenPB"].HeaderText = "Phòng Ban";
                dgvChamCong.Columns["TenPB"].ReadOnly = true;
                dgvChamCong.Columns["TenPB"].DefaultCellStyle.BackColor = Color.FromArgb(241, 245, 249);
            }
            if (dgvChamCong.Columns["ThangNam"] != null)
            {
                dgvChamCong.Columns["ThangNam"].HeaderText = "Tháng/Năm";
                dgvChamCong.Columns["ThangNam"].ReadOnly = true;
                dgvChamCong.Columns["ThangNam"].DefaultCellStyle.BackColor = Color.FromArgb(241, 245, 249);
            }
            if (dgvChamCong.Columns["SoNgayCong"] != null)
            {
                dgvChamCong.Columns["SoNgayCong"].HeaderText = "Số Ngày Công (Sửa trực tiếp)";
                dgvChamCong.Columns["SoNgayCong"].ReadOnly = false;
                dgvChamCong.Columns["SoNgayCong"].DefaultCellStyle.BackColor = Color.FromArgb(254, 243, 199); // Highlight editable
            }
            if (dgvChamCong.Columns["SoGioOT"] != null)
            {
                dgvChamCong.Columns["SoGioOT"].HeaderText = "Số Giờ OT (Sửa trực tiếp)";
                dgvChamCong.Columns["SoGioOT"].ReadOnly = false;
                dgvChamCong.Columns["SoGioOT"].DefaultCellStyle.BackColor = Color.FromArgb(254, 243, 199); // Highlight editable
            }
            if (dgvChamCong.Columns["DaKhoa"] != null)
            {
                dgvChamCong.Columns["DaKhoa"].HeaderText = "Đã Khóa Lương";
                dgvChamCong.Columns["DaKhoa"].ReadOnly = true;
            }

            bool daKhoa = _chamCongBUS.KiemTraDaKhoa(thangNam);
            if (daKhoa)
            {
                lblTrangThai.Text = "Trạng thái: [ĐÃ KHÓA] Dữ liệu tháng này đã chốt lương";
                lblTrangThai.ForeColor = Color.FromArgb(220, 38, 38);
                btnLuuChamCong.Enabled = false;
                dgvChamCong.ReadOnly = true;
            }
            else
            {
                lblTrangThai.Text = "Trạng thái: [MỞ] Sẵn sàng nhập liệu và lưu";
                lblTrangThai.ForeColor = Color.FromArgb(5, 150, 105);
                btnLuuChamCong.Enabled = true;
                dgvChamCong.ReadOnly = false;
            }
        }

        private void BtnLuuChamCong_Click(object? sender, EventArgs e)
        {
            if (_adapter == null || _table == null)
            {
                MessageBox.Show("Chưa có dữ liệu chấm công để lưu!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kết thúc chỉnh sửa cell trên grid
            dgvChamCong.EndEdit();

            string thangNam = GetThangNam();

            // Kiểm tra tính hợp lệ dữ liệu
            foreach (DataRow row in _table.Rows)
            {
                if (row.RowState == DataRowState.Deleted) continue;

                double ngayCong = Convert.ToDouble(row["SoNgayCong"]);
                double gioOT = Convert.ToDouble(row["SoGioOT"]);

                if (ngayCong < 0 || ngayCong > 31)
                {
                    MessageBox.Show($"Số ngày công của nhân viên [{row["HoTen"]}] phải nằm trong khoảng từ 0 đến 31!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (gioOT < 0)
                {
                    MessageBox.Show($"Số giờ OT của nhân viên [{row["HoTen"]}] không được âm!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // Gọi đồng bộ Disconnected Model (adapter.Update)
            var (ok, msg, rows) = _chamCongBUS.LuuChamCong(_adapter, _table, thangNam);
            MessageBox.Show(msg, ok ? "Thông báo" : "Lỗi", MessageBoxButtons.OK, ok ? MessageBoxIcon.Information : MessageBoxIcon.Error);

            if (ok)
            {
                TaiDuLieuChamCong();
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            _connection?.Dispose();
        }
    }
}
