using System;
using System.Drawing;
using System.Windows.Forms;

namespace Bai5_1_DangKyTaiKhoan;

public class FormDangKy : Form
{
    private TextBox txtTenDangNhap = new();
    private TextBox txtMatKhau = new();
    private TextBox txtXacNhanMatKhau = new();
    private DateTimePicker dtpNgaySinh = new();
    private RadioButton rdoNam = new();
    private RadioButton rdoNu = new();
    private CheckBox chkDieuKhoan = new();
    private Button btnDangKy = new();
    private Button btnLamMoi = new();
    private ErrorProvider epCheck = new();

    public FormDangKy()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        Text = "Bài 5.1 - Đăng ký tài khoản";
        StartPosition = FormStartPosition.CenterScreen;
        ClientSize = new Size(570, 510);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        Font = new Font("Segoe UI", 10);

        var lblTitle = new Label
        {
            Text = "ĐĂNG KÝ TÀI KHOẢN",
            Font = new Font("Segoe UI", 17, FontStyle.Bold),
            ForeColor = Color.DarkBlue,
            AutoSize = true,
            Location = new Point(150, 20)
        };
        Controls.Add(lblTitle);

        var grpCaNhan = new GroupBox
        {
            Text = "Thông tin cá nhân",
            Location = new Point(35, 75),
            Size = new Size(500, 190)
        };
        Controls.Add(grpCaNhan);

        AddLabel(grpCaNhan, "Tên đăng nhập:", 25, 35);
        txtTenDangNhap.SetBounds(190, 32, 270, 28);
        grpCaNhan.Controls.Add(txtTenDangNhap);

        AddLabel(grpCaNhan, "Mật khẩu:", 25, 80);
        txtMatKhau.SetBounds(190, 77, 270, 28);
        txtMatKhau.UseSystemPasswordChar = true;
        grpCaNhan.Controls.Add(txtMatKhau);

        AddLabel(grpCaNhan, "Xác nhận mật khẩu:", 25, 125);
        txtXacNhanMatKhau.SetBounds(190, 122, 270, 28);
        txtXacNhanMatKhau.UseSystemPasswordChar = true;
        grpCaNhan.Controls.Add(txtXacNhanMatKhau);

        var grpBoSung = new GroupBox
        {
            Text = "Thông tin bổ sung",
            Location = new Point(35, 280),
            Size = new Size(500, 135)
        };
        Controls.Add(grpBoSung);

        AddLabel(grpBoSung, "Ngày sinh:", 25, 30);
        dtpNgaySinh.SetBounds(190, 27, 190, 28);
        dtpNgaySinh.Format = DateTimePickerFormat.Short;
        dtpNgaySinh.MaxDate = DateTime.Today;
        dtpNgaySinh.Value = DateTime.Today.AddYears(-18);
        grpBoSung.Controls.Add(dtpNgaySinh);

        AddLabel(grpBoSung, "Giới tính:", 25, 70);
        rdoNam.Text = "Nam";
        rdoNam.SetBounds(190, 68, 70, 25);
        rdoNam.Checked = true;
        rdoNu.Text = "Nữ";
        rdoNu.SetBounds(280, 68, 70, 25);
        grpBoSung.Controls.Add(rdoNam);
        grpBoSung.Controls.Add(rdoNu);

        chkDieuKhoan.Text = "Tôi đồng ý với điều khoản dịch vụ";
        chkDieuKhoan.AutoSize = true;
        chkDieuKhoan.Location = new Point(190, 100);
        grpBoSung.Controls.Add(chkDieuKhoan);

        btnDangKy.Text = "Đăng Ký";
        btnDangKy.SetBounds(150, 445, 115, 38);
        btnDangKy.BackColor = Color.LightSteelBlue;
        btnDangKy.Click += btnDangKy_Click;
        Controls.Add(btnDangKy);

        btnLamMoi.Text = "Làm Mới";
        btnLamMoi.SetBounds(295, 445, 115, 38);
        btnLamMoi.Click += btnLamMoi_Click;
        Controls.Add(btnLamMoi);

        epCheck.ContainerControl = this;
        epCheck.BlinkStyle = ErrorBlinkStyle.NeverBlink;
    }

    private static void AddLabel(Control parent, string text, int x, int y)
    {
        parent.Controls.Add(new Label
        {
            Text = text,
            AutoSize = true,
            Location = new Point(x, y + 4)
        });
    }

    private void btnDangKy_Click(object? sender, EventArgs e)
    {
        epCheck.Clear();
        bool hopLe = true;

        if (string.IsNullOrWhiteSpace(txtTenDangNhap.Text))
        {
            epCheck.SetError(txtTenDangNhap, "Tên đăng nhập không được để trống!");
            hopLe = false;
        }

        if (string.IsNullOrWhiteSpace(txtMatKhau.Text))
        {
            epCheck.SetError(txtMatKhau, "Mật khẩu không được để trống!");
            hopLe = false;
        }

        if (!string.IsNullOrEmpty(txtMatKhau.Text) &&
            txtMatKhau.Text != txtXacNhanMatKhau.Text)
        {
            epCheck.SetError(txtXacNhanMatKhau, "Mật khẩu xác nhận không khớp!");
            hopLe = false;
        }
        else if (string.IsNullOrEmpty(txtXacNhanMatKhau.Text) &&
                 !string.IsNullOrEmpty(txtMatKhau.Text))
        {
            epCheck.SetError(txtXacNhanMatKhau, "Vui lòng xác nhận mật khẩu!");
            hopLe = false;
        }

        int tuoi = TinhTuoi(dtpNgaySinh.Value.Date);
        if (tuoi < 18)
        {
            epCheck.SetError(dtpNgaySinh, "Bạn phải đủ 18 tuổi trở lên!");
            hopLe = false;
        }

        if (!chkDieuKhoan.Checked)
        {
            epCheck.SetError(chkDieuKhoan, "Bạn phải đồng ý với điều khoản dịch vụ!");
            hopLe = false;
        }

        if (hopLe)
        {
            string gioiTinh = rdoNam.Checked ? "Nam" : "Nữ";
            MessageBox.Show(
                $"Đăng ký tài khoản thành công!\nTên đăng nhập: {txtTenDangNhap.Text.Trim()}\nGiới tính: {gioiTinh}",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }

    private static int TinhTuoi(DateTime ngaySinh)
    {
        DateTime homNay = DateTime.Today;
        int tuoi = homNay.Year - ngaySinh.Year;
        if (ngaySinh.Date > homNay.AddYears(-tuoi))
            tuoi--;
        return tuoi;
    }

    private void btnLamMoi_Click(object? sender, EventArgs e)
    {
        txtTenDangNhap.Clear();
        txtMatKhau.Clear();
        txtXacNhanMatKhau.Clear();
        dtpNgaySinh.Value = DateTime.Today.AddYears(-18);
        rdoNam.Checked = true;
        chkDieuKhoan.Checked = false;
        epCheck.Clear();
        txtTenDangNhap.Focus();
    }
}
