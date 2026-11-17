using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HRMPayrollSystem.BUS;
using HRMPayrollSystem.DTO;

namespace HRMPayrollSystem.GUI
{
    public class FormTinhLuong : Form
    {
        private readonly TinhLuongBUS _tinhLuongBUS = new();
        private readonly ChamCongBUS _chamCongBUS = new();

        private List<BangLuongDTO> _currentList = new();

        private NumericUpDown numThang = null!;
        private NumericUpDown numNam = null!;
        private Button btnTinhLuong = null!;
        private Button btnChotLuong = null!;
        private Button btnXemDaChot = null!;
        private Label lblTongQuyLuong = null!;
        private Label lblTrangThai = null!;
        private DataGridView dgvLuong = null!;

        public FormTinhLuong()
        {
            InitializeComponent();
            TinhLuong();
        }

        private void InitializeComponent()
        {
            this.Text = "Tính Lương & Chốt Lương Tháng - HRM System (SqlTransaction ACID)";
            this.Size = new Size(1150, 700);
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
                Text = "TÍNH LƯƠNG & CHỐT LƯƠNG TOÀN CÔNG TY (GIAO DỊCH SqlTransaction)",
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
                Height = 90,
                BackColor = Color.White,
                Padding = new Padding(20, 15, 20, 15)
            };

            var lblThang = new Label { Text = "Tháng:", Location = new Point(20, 25), AutoSize = true, Font = new Font("Segoe UI", 10f, FontStyle.Bold) };
            numThang = new NumericUpDown { Location = new Point(80, 22), Size = new Size(60, 28), Minimum = 1, Maximum = 12, Value = 11, Font = new Font("Segoe UI", 10f) };

            var lblNam = new Label { Text = "Năm:", Location = new Point(160, 25), AutoSize = true, Font = new Font("Segoe UI", 10f, FontStyle.Bold) };
            numNam = new NumericUpDown { Location = new Point(210, 22), Size = new Size(80, 28), Minimum = 2020, Maximum = 2035, Value = 2026, Font = new Font("Segoe UI", 10f) };

            btnTinhLuong = new Button
            {
                Text = "⚙️ Tính Lương Tự Động (Theo Công Thức)",
                Location = new Point(310, 20),
                Size = new Size(270, 34),
                BackColor = Color.FromArgb(59, 130, 246),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnTinhLuong.FlatAppearance.BorderSize = 0;
            btnTinhLuong.Click += (s, e) => TinhLuong();

            btnChotLuong = new Button
            {
                Text = "🔒 Chốt Lương Tháng (SqlTransaction)",
                Location = new Point(600, 20),
                Size = new Size(270, 34),
                BackColor = Color.FromArgb(220, 38, 38),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnChotLuong.FlatAppearance.BorderSize = 0;
            btnChotLuong.Click += BtnChotLuong_Click;

            btnXemDaChot = new Button
            {
                Text = "📋 Xem Bảng Lương Đã Chốt",
                Location = new Point(890, 20),
                Size = new Size(220, 34),
                BackColor = Color.FromArgb(100, 116, 139),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnXemDaChot.FlatAppearance.BorderSize = 0;
            btnXemDaChot.Click += (s, e) => XemBangLuongDaChot();

            lblTrangThai = new Label
            {
                Text = "Trạng thái: Đang xem bảng tính lương dự kiến",
                Location = new Point(20, 60),
                AutoSize = true,
                Font = new Font("Segoe UI", 9f, FontStyle.Italic),
                ForeColor = Color.FromArgb(71, 85, 105)
            };

            pnlToolbar.Controls.AddRange(new Control[] { lblThang, numThang, lblNam, numNam, btnTinhLuong, btnChotLuong, btnXemDaChot, lblTrangThai });

            // Formula Banner
            var pnlFormula = new Panel
            {
                Dock = DockStyle.Top,
                Height = 45,
                BackColor = Color.FromArgb(238, 242, 255),
                Padding = new Padding(20, 10, 20, 10)
            };
            var lblFormula = new Label
            {
                Text = "📐 Công thức tính lương đề bài: Thực Lĩnh = (Lương Cơ Bản × Hệ Số Lương ÷ 22) × Số Ngày Công + (Số Giờ OT × 50,000 VNĐ)",
                ForeColor = Color.FromArgb(67, 56, 202),
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Dock = DockStyle.Fill
            };
            pnlFormula.Controls.Add(lblFormula);

            // Bottom Summary Panel
            var pnlSummary = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 70,
                BackColor = Color.White,
                Padding = new Padding(20, 15, 20, 15)
            };

            lblTongQuyLuong = new Label
            {
                Text = "TỔNG QUỸ LƯƠNG PHẢI CHI TRẢ: 0 VNĐ",
                Font = new Font("Segoe UI", 14f, FontStyle.Bold),
                ForeColor = Color.FromArgb(220, 38, 38),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleRight
            };
            pnlSummary.Controls.Add(lblTongQuyLuong);

            // Grid
            dgvLuong = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                Font = new Font("Segoe UI", 9.5f)
            };

            this.Controls.Add(dgvLuong);
            this.Controls.Add(pnlSummary);
            this.Controls.Add(pnlFormula);
            this.Controls.Add(pnlToolbar);
            this.Controls.Add(pnlHeader);
        }

        private string GetThangNam() => $"{(int)numThang.Value:D2}/{numNam.Value}";

        private void TinhLuong()
        {
            string thangNam = GetThangNam();
            _currentList = _tinhLuongBUS.TinhLuongDuKien(thangNam);
            dgvLuong.DataSource = _currentList;
            FormatGrid();

            decimal tong = _currentList.Sum(x => x.ThucLinh);
            lblTongQuyLuong.Text = $"TỔNG QUỸ LƯƠNG ({_currentList.Count} nhân viên): {tong:N0} VNĐ";

            bool daKhoa = _chamCongBUS.KiemTraDaKhoa(thangNam);
            if (daKhoa)
            {
                lblTrangThai.Text = $"Trạng thái tháng {thangNam}: [ĐÃ CHỐT LƯƠNG & KHÓA CHẤM CÔNG]";
                lblTrangThai.ForeColor = Color.FromArgb(220, 38, 38);
                btnChotLuong.Enabled = false;
            }
            else
            {
                lblTrangThai.Text = $"Trạng thái tháng {thangNam}: [CHƯA CHỐT LƯƠNG] - Sẵn sàng thực thi giao dịch chốt lương!";
                lblTrangThai.ForeColor = Color.FromArgb(5, 150, 105);
                btnChotLuong.Enabled = true;
            }
        }

        private void XemBangLuongDaChot()
        {
            string thangNam = GetThangNam();
            var listDaChot = _tinhLuongBUS.GetBangLuongDaChot(thangNam);
            if (listDaChot.Count == 0)
            {
                MessageBox.Show($"Tháng {thangNam} chưa có dữ liệu bảng lương đã chốt!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            dgvLuong.DataSource = listDaChot;
            FormatGrid();

            decimal tong = listDaChot.Sum(x => x.ThucLinh);
            lblTongQuyLuong.Text = $"TỔNG QUỸ LƯƠNG ĐÃ CHỐT ({listDaChot.Count} nhân viên): {tong:N0} VNĐ";
            lblTrangThai.Text = $"Trạng thái: Đang hiển thị bảng lương chính thức đã chốt tháng {thangNam}";
            lblTrangThai.ForeColor = Color.FromArgb(30, 41, 59);
        }

        private void FormatGrid()
        {
            if (dgvLuong.Columns["MaNV"] != null) dgvLuong.Columns["MaNV"].HeaderText = "Mã NV";
            if (dgvLuong.Columns["HoTen"] != null) dgvLuong.Columns["HoTen"].HeaderText = "Họ và Tên";
            if (dgvLuong.Columns["TenPB"] != null) dgvLuong.Columns["TenPB"].HeaderText = "Phòng Ban";
            if (dgvLuong.Columns["ThangNam"] != null) dgvLuong.Columns["ThangNam"].HeaderText = "Tháng/Năm";
            if (dgvLuong.Columns["LuongCoBan"] != null)
            {
                dgvLuong.Columns["LuongCoBan"].HeaderText = "Lương CB (VNĐ)";
                dgvLuong.Columns["LuongCoBan"].DefaultCellStyle.Format = "N0";
            }
            if (dgvLuong.Columns["HeSoLuong"] != null) dgvLuong.Columns["HeSoLuong"].HeaderText = "Hệ Số";
            if (dgvLuong.Columns["SoNgayCong"] != null) dgvLuong.Columns["SoNgayCong"].HeaderText = "Số Công";
            if (dgvLuong.Columns["SoGioOT"] != null) dgvLuong.Columns["SoGioOT"].HeaderText = "Giờ OT";
            if (dgvLuong.Columns["ThucLinh"] != null)
            {
                dgvLuong.Columns["ThucLinh"].HeaderText = "Thực Lĩnh (VNĐ)";
                dgvLuong.Columns["ThucLinh"].DefaultCellStyle.Format = "N0";
                dgvLuong.Columns["ThucLinh"].DefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
                dgvLuong.Columns["ThucLinh"].DefaultCellStyle.ForeColor = Color.FromArgb(220, 38, 38);
            }
            if (dgvLuong.Columns["MaLuong"] != null) dgvLuong.Columns["MaLuong"].Visible = false;
        }

        private void BtnChotLuong_Click(object? sender, EventArgs e)
        {
            string thangNam = GetThangNam();
            if (_currentList == null || _currentList.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu nhân viên để chốt lương!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show(
                $"Xác nhận mở giao dịch SqlTransaction chốt lương tháng {thangNam}?\n\n" +
                $"• Số lượng nhân viên: {_currentList.Count}\n" +
                $"• Tổng thực lĩnh: {_currentList.Sum(x => x.ThucLinh):N0} VNĐ\n" +
                $"• Hệ thống sẽ tự động ghi nhận vào BangLuong và KHÓA BẢNG CHẤM CÔNG của tháng {thangNam}.",
                "Xác nhận Chốt Lương (SqlTransaction)",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirm == DialogResult.Yes)
            {
                // THỰC THI GIAO DỊCH SqlTransaction
                var result = _tinhLuongBUS.ChotLuongThang(thangNam, _currentList);

                if (result.Success)
                {
                    MessageBox.Show(
                        $"{result.Message}\n\n" +
                        "• Trạng Thái Transaction: COMMIT THÀNH CÔNG!\n" +
                        "• Toàn bộ dữ liệu bảng lương đã được ghi nhận an toàn (ACID).\n" +
                        "• Dữ liệu chấm công đã được khóa chống sửa đổi.",
                        "Chốt Lương Thành Công",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                    TinhLuong();
                }
                else
                {
                    MessageBox.Show(
                        $"{result.Message}\n\n" +
                        "• Giao dịch đã được ROLLBACK an toàn, không có dữ liệu sai lệch nào được lưu.",
                        "Lỗi Giao Dịch Chốt Lương",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
        }
    }
}
