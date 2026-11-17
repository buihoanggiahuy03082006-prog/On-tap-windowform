using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using RetailSalesSystem.BUS;
using RetailSalesSystem.DTO;

namespace RetailSalesSystem.GUI
{
    public class FormBanHang : Form
    {
        private readonly SanPhamBUS _sanPhamBUS = new();
        private readonly KhachHangBUS _khachHangBUS = new();
        private readonly NhanVienBUS _nhanVienBUS = new();
        private readonly HoaDonBUS _hoaDonBUS = new();

        private readonly List<CartItemDTO> _cartItems = new();

        // Controls
        private ComboBox cboNhanVien = null!;
        private ComboBox cboKhachHang = null!;
        private CheckBox chkKhachLe = null!;
        private ComboBox cboSanPham = null!;
        private Label lblTonKho = null!;
        private Label lblDonGia = null!;
        private NumericUpDown numSoLuong = null!;
        private Button btnThemVaoGio = null!;
        private Button btnXoaDong = null!;
        private Button btnXoaTatCa = null!;

        private DataGridView dgvGioHang = null!;
        private Label lblTongTien = null!;
        private Label lblDiemTichLuy = null!;
        private Button btnThanhToan = null!;
        private Button btnTestRollback = null!;

        public FormBanHang()
        {
            InitializeComponent();
            LoadComboBoxData();
        }

        private void InitializeComponent()
        {
            this.Text = "Hệ thống Bán Hàng & Lập Hóa Đơn - Giao dịch SqlTransaction ACID";
            this.Size = new Size(1150, 720);
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
                Text = "LẬP HÓA ĐƠN BÁN HÀNG & THANH TOÁN (ACID SqlTransaction)",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 14f, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0)
            };
            pnlHeader.Controls.Add(lblTitle);

            // Left Section: Thông tin hóa đơn & Chọn sản phẩm
            var grpCustomer = new GroupBox
            {
                Text = "1. Thông tin giao dịch & Khách hàng",
                Location = new Point(20, 80),
                Size = new Size(420, 190),
                BackColor = Color.White,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold)
            };

            AddLabel(grpCustomer, "Nhân viên thu ngân (*):", 20, 30);
            cboNhanVien = new ComboBox
            {
                Location = new Point(20, 52),
                Size = new Size(380, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9.5f)
            };
            grpCustomer.Controls.Add(cboNhanVien);

            AddLabel(grpCustomer, "Khách hàng nhận điểm thưởng:", 20, 90);
            cboKhachHang = new ComboBox
            {
                Location = new Point(20, 112),
                Size = new Size(380, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9.5f)
            };
            grpCustomer.Controls.Add(cboKhachHang);

            chkKhachLe = new CheckBox
            {
                Text = "Khách vãng lai / Khách lẻ (Không tích điểm)",
                Location = new Point(20, 150),
                AutoSize = true,
                Font = new Font("Segoe UI", 9f, FontStyle.Regular),
                ForeColor = Color.FromArgb(71, 85, 105)
            };
            chkKhachLe.CheckedChanged += (s, e) =>
            {
                cboKhachHang.Enabled = !chkKhachLe.Checked;
                CapNhatTongTienVaDiem();
            };
            grpCustomer.Controls.Add(chkKhachLe);

            // Group Box Chọn Hàng Hóa
            var grpProduct = new GroupBox
            {
                Text = "2. Chọn sản phẩm vào giỏ hàng",
                Location = new Point(20, 285),
                Size = new Size(420, 270),
                BackColor = Color.White,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold)
            };

            AddLabel(grpProduct, "Chọn sản phẩm / Hàng hóa (*):", 20, 30);
            cboSanPham = new ComboBox
            {
                Location = new Point(20, 52),
                Size = new Size(380, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9.5f)
            };
            cboSanPham.SelectedIndexChanged += CboSanPham_SelectedIndexChanged;
            grpProduct.Controls.Add(cboSanPham);

            lblDonGia = new Label
            {
                Text = "Đơn giá: 0 VNĐ",
                Location = new Point(20, 92),
                Size = new Size(380, 25),
                ForeColor = Color.FromArgb(37, 99, 235),
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold)
            };
            grpProduct.Controls.Add(lblDonGia);

            lblTonKho = new Label
            {
                Text = "Tồn kho khả dụng: 0",
                Location = new Point(20, 120),
                Size = new Size(380, 25),
                ForeColor = Color.FromArgb(5, 150, 105),
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold)
            };
            grpProduct.Controls.Add(lblTonKho);

            AddLabel(grpProduct, "Số lượng mua:", 20, 152);
            numSoLuong = new NumericUpDown
            {
                Location = new Point(20, 175),
                Size = new Size(180, 28),
                Minimum = 1,
                Maximum = 10000,
                Value = 1,
                Font = new Font("Segoe UI", 9.5f)
            };
            grpProduct.Controls.Add(numSoLuong);

            btnThemVaoGio = CreateButton("+ Thêm vào giỏ", Color.FromArgb(16, 185, 129), 215, 172, 185, 34);
            btnThemVaoGio.Click += BtnThemVaoGio_Click;
            grpProduct.Controls.Add(btnThemVaoGio);

            btnTestRollback = CreateButton("⚠️ Thử lỗi Rollback (Vượt tồn)", Color.FromArgb(239, 68, 68), 20, 218, 380, 34);
            btnTestRollback.Click += BtnTestRollback_Click;
            grpProduct.Controls.Add(btnTestRollback);

            // Right Section: Giỏ hàng & Thanh toán
            var pnlRight = new Panel
            {
                Location = new Point(460, 80),
                Size = new Size(660, 580)
            };

            var grpCart = new GroupBox
            {
                Text = "3. Chi tiết giỏ hàng",
                Dock = DockStyle.Top,
                Height = 400,
                BackColor = Color.White,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold)
            };

            dgvGioHang = new DataGridView
            {
                Location = new Point(15, 30),
                Size = new Size(630, 315),
                BackgroundColor = Color.White,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                AllowUserToAddRows = false
            };
            grpCart.Controls.Add(dgvGioHang);

            btnXoaDong = CreateButton("Xóa mặt hàng đã chọn", Color.FromArgb(220, 38, 38), 15, 355, 190, 32);
            btnXoaDong.Click += BtnXoaDong_Click;
            grpCart.Controls.Add(btnXoaDong);

            btnXoaTatCa = CreateButton("Làm trống giỏ hàng", Color.FromArgb(100, 116, 139), 215, 355, 180, 32);
            btnXoaTatCa.Click += (s, e) =>
            {
                _cartItems.Clear();
                RefreshCartGrid();
            };
            grpCart.Controls.Add(btnXoaTatCa);

            // Bottom Payment Panel
            var pnlPayment = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 165,
                BackColor = Color.FromArgb(238, 242, 255)
            };

            lblTongTien = new Label
            {
                Text = "TỔNG TIỀN PHẢI TRẢ: 0 VNĐ",
                Location = new Point(20, 15),
                Size = new Size(620, 32),
                Font = new Font("Segoe UI", 15f, FontStyle.Bold),
                ForeColor = Color.FromArgb(220, 38, 38)
            };
            pnlPayment.Controls.Add(lblTongTien);

            lblDiemTichLuy = new Label
            {
                Text = "Điểm tích lũy cộng thêm (+1 điểm / 100.000 VNĐ): +0 điểm",
                Location = new Point(20, 52),
                Size = new Size(620, 24),
                Font = new Font("Segoe UI", 10.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(5, 150, 105)
            };
            pnlPayment.Controls.Add(lblDiemTichLuy);

            btnThanhToan = new Button
            {
                Text = "THANH TOÁN HÓA ĐƠN (SqlTransaction)",
                Location = new Point(20, 90),
                Size = new Size(620, 55),
                BackColor = Color.FromArgb(79, 70, 229),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnThanhToan.FlatAppearance.BorderSize = 0;
            btnThanhToan.Click += BtnThanhToan_Click;
            pnlPayment.Controls.Add(btnThanhToan);

            pnlRight.Controls.Add(grpCart);
            pnlRight.Controls.Add(pnlPayment);

            this.Controls.Add(pnlRight);
            this.Controls.Add(grpProduct);
            this.Controls.Add(grpCustomer);
            this.Controls.Add(pnlHeader);
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
                Font = new Font("Segoe UI", 9f, FontStyle.Bold)
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        private void LoadComboBoxData()
        {
            // Nhân viên
            cboNhanVien.DataSource = _nhanVienBUS.GetAll();
            cboNhanVien.DisplayMember = "TenNV";
            cboNhanVien.ValueMember = "MaNV";

            // Khách hàng
            cboKhachHang.DataSource = _khachHangBUS.GetAll();
            cboKhachHang.DisplayMember = "TenKH";
            cboKhachHang.ValueMember = "MaKH";

            // Sản phẩm
            var sanPhams = _sanPhamBUS.GetAll();
            cboSanPham.DataSource = sanPhams;
            cboSanPham.DisplayMember = "TenSP";
            cboSanPham.ValueMember = "MaSP";

            RefreshCartGrid();
        }

        private void CboSanPham_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cboSanPham.SelectedItem is SanPhamDTO sp)
            {
                lblDonGia.Text = $"Đơn giá: {sp.GiaBan:N0} VNĐ";
                lblTonKho.Text = $"Tồn kho khả dụng: {sp.SoLuongTon}";
                lblTonKho.ForeColor = sp.SoLuongTon > 5 ? Color.FromArgb(5, 150, 105) : Color.FromArgb(220, 38, 38);
            }
        }

        private void BtnThemVaoGio_Click(object? sender, EventArgs e)
        {
            if (cboSanPham.SelectedItem is not SanPhamDTO sp)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int soLuong = (int)numSoLuong.Value;
            var existing = _cartItems.FirstOrDefault(x => x.MaSP == sp.MaSP);
            if (existing != null)
            {
                existing.SoLuong += soLuong;
            }
            else
            {
                _cartItems.Add(new CartItemDTO
                {
                    MaSP = sp.MaSP,
                    TenSP = sp.TenSP,
                    DonGia = sp.GiaBan,
                    SoLuong = soLuong,
                    SoLuongTon = sp.SoLuongTon
                });
            }

            RefreshCartGrid();
        }

        private void BtnTestRollback_Click(object? sender, EventArgs e)
        {
            // Tìm sản phẩm có tồn kho ít để thử nghiệm vượt tồn kho
            var sp = _sanPhamBUS.GetAll().FirstOrDefault(s => s.MaSP == "SP10") ?? _sanPhamBUS.GetAll().FirstOrDefault();
            if (sp != null)
            {
                int soLuongVuot = sp.SoLuongTon + 50; // Chắc chắn vượt tồn kho
                _cartItems.Clear();
                _cartItems.Add(new CartItemDTO
                {
                    MaSP = sp.MaSP,
                    TenSP = sp.TenSP,
                    DonGia = sp.GiaBan,
                    SoLuong = soLuongVuot,
                    SoLuongTon = sp.SoLuongTon
                });

                RefreshCartGrid();
                MessageBox.Show(
                    $"Đã thêm thử nghiệm: Sản phẩm '{sp.TenSP}' có tồn kho thực tế là {sp.SoLuongTon}, nhưng số lượng đặt mua trong giỏ là {soLuongVuot}.\n\nBây giờ bạn hãy bấm nút [THANH TOÁN HÓA ĐƠN] bên dưới để kiểm chứng giao dịch bị hủy và tự động gọi trans.Rollback() theo chuẩn ACID!",
                    "Kịch bản thử nghiệm Rollback",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
        }

        private void BtnXoaDong_Click(object? sender, EventArgs e)
        {
            if (dgvGioHang.CurrentRow?.DataBoundItem is CartItemDTO item)
            {
                _cartItems.Remove(item);
                RefreshCartGrid();
            }
        }

        private void RefreshCartGrid()
        {
            dgvGioHang.DataSource = null;
            dgvGioHang.DataSource = _cartItems.ToList();

            if (dgvGioHang.Columns["MaSP"] != null) dgvGioHang.Columns["MaSP"].HeaderText = "Mã SP";
            if (dgvGioHang.Columns["TenSP"] != null) dgvGioHang.Columns["TenSP"].HeaderText = "Tên Sản Phẩm";
            if (dgvGioHang.Columns["DonGia"] != null)
            {
                dgvGioHang.Columns["DonGia"].HeaderText = "Đơn Giá";
                dgvGioHang.Columns["DonGia"].DefaultCellStyle.Format = "N0";
            }
            if (dgvGioHang.Columns["SoLuong"] != null) dgvGioHang.Columns["SoLuong"].HeaderText = "Số Lượng";
            if (dgvGioHang.Columns["ThanhTien"] != null)
            {
                dgvGioHang.Columns["ThanhTien"].HeaderText = "Thành Tiền (VNĐ)";
                dgvGioHang.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";
            }
            if (dgvGioHang.Columns["SoLuongTon"] != null)
            {
                dgvGioHang.Columns["SoLuongTon"].HeaderText = "Tồn Kho Hiện Tại";
            }

            CapNhatTongTienVaDiem();
        }

        private void CapNhatTongTienVaDiem()
        {
            decimal tongTien = _cartItems.Sum(x => x.ThanhTien);
            lblTongTien.Text = $"TỔNG TIỀN PHẢI TRẢ: {tongTien:N0} VNĐ";

            if (chkKhachLe.Checked || cboKhachHang.SelectedValue == null)
            {
                lblDiemTichLuy.Text = "Khách lẻ / vãng lai: Không tích lũy điểm thưởng";
                lblDiemTichLuy.ForeColor = Color.Gray;
            }
            else
            {
                int diemCong = (int)(tongTien / 100000m);
                lblDiemTichLuy.Text = $"Điểm tích lũy cộng thêm (+1 điểm / 100.000 VNĐ): +{diemCong} điểm";
                lblDiemTichLuy.ForeColor = Color.FromArgb(5, 150, 105);
            }
        }

        private void BtnThanhToan_Click(object? sender, EventArgs e)
        {
            if (cboNhanVien.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn nhân viên thu ngân!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_cartItems.Count == 0)
            {
                MessageBox.Show("Giỏ hàng đang trống! Vui lòng chọn sản phẩm trước khi thanh toán.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maNV = Convert.ToInt32(cboNhanVien.SelectedValue);
            int? maKH = chkKhachLe.Checked ? null : (int?)Convert.ToInt32(cboKhachHang.SelectedValue);
            decimal tongTien = _cartItems.Sum(x => x.ThanhTien);

            var hoaDon = new HoaDonDTO
            {
                NgayLap = DateTime.Now,
                MaNV = maNV,
                MaKH = maKH,
                TongTien = tongTien
            };

            // THỰC THI GIAO DỊCH SqlTransaction
            var result = _hoaDonBUS.ThanhToan(hoaDon, _cartItems);

            if (result.Success)
            {
                MessageBox.Show(
                    $"{result.Message}\n\n" +
                    $"• Mã Hóa Đơn: #{result.MaHD}\n" +
                    $"• Tổng Tiền: {tongTien:N0} VNĐ\n" +
                    $"• Số Lượng Mặt Hàng: {_cartItems.Count}\n" +
                    $"• Trạng Thái Transaction: COMMIT THÀNH CÔNG!\n" +
                    $"• Đã trừ số lượng tồn kho tự động trong bảng SanPham.\n" +
                    (maKH.HasValue ? $"• Đã cộng +{(int)(tongTien / 100000m)} điểm cho khách hàng #{maKH.Value}." : "• Khách lẻ: Không tích điểm."),
                    "Giao Dịch SqlTransaction Thành Công (ACID Commit)",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                // Dọn giỏ và nạp lại dữ liệu sản phẩm
                _cartItems.Clear();
                RefreshCartGrid();
                LoadComboBoxData();
            }
            else
            {
                // Giao dịch bị Rollback
                MessageBox.Show(
                    $"{result.Message}\n\n" +
                    "• Trạng Thái: GIAO DỊCH ĐÃ BỊ HỦY BỎ (TRANSACTION ROLLBACK)!\n" +
                    "• Tính toàn vẹn dữ liệu (ACID) được bảo toàn 100%:\n" +
                    "  - Không có hóa đơn rác nào được chèn vào HoaDon.\n" +
                    "  - Không có chi tiết nào được lưu vào ChiTietHoaDon.\n" +
                    "  - Tồn kho của các sản phẩm khác không bị trừ sai lệch.\n" +
                    "  - Điểm của khách hàng không bị thay đổi.",
                    "Lỗi Giao Dịch - Kích Hoạt Rollback Thành Công",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}
