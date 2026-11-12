using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Bai5_2_TinhTienDichVu;

public class DichVu
{
    public string Ten { get; set; }
    public decimal DonGia { get; set; }
    public DichVu(string ten, decimal donGia) { Ten = ten; DonGia = donGia; }
    public override string ToString() => $"{Ten} - {DonGia:N0} đ";
}

public class FormTinhTien : Form
{
    private readonly ComboBox cboCategory = new();
    private readonly ListBox lstAvailableServices = new();
    private readonly ListBox lstSelectedServices = new();
    private readonly Label lblTotal = new();
    private readonly Label lblDiscount = new();
    private readonly Label lblPayment = new();

    private readonly Dictionary<string, List<DichVu>> danhSachDichVu = new()
    {
        ["Khám bệnh"] = new() {
            new("Khám tổng quát", 200000), new("Khám chuyên khoa", 300000),
            new("Khám ngoài giờ", 350000), new("Tư vấn sức khỏe", 100000)
        },
        ["Xét nghiệm"] = new() {
            new("Xét nghiệm máu", 150000), new("Xét nghiệm nước tiểu", 100000),
            new("Xét nghiệm đường huyết", 80000), new("Xét nghiệm mỡ máu", 180000)
        },
        ["Chụp X-Quang"] = new() {
            new("Chụp X-Quang phổi", 250000), new("Chụp X-Quang xương", 300000),
            new("Chụp X-Quang cột sống", 400000)
        },
        ["Vắc-xin"] = new() {
            new("Vắc-xin cúm", 350000), new("Vắc-xin viêm gan B", 250000),
            new("Vắc-xin uốn ván", 200000)
        }
    };

    public FormTinhTien()
    {
        InitializeComponent();
        cboCategory.SelectedIndex = 0;
        CapNhatThanhTien();
    }

    private void InitializeComponent()
    {
        Text = "Bài 5.2 - Bảng tính tiền dịch vụ";
        StartPosition = FormStartPosition.CenterScreen;
        ClientSize = new Size(900, 610);
        MinimumSize = new Size(850, 600);
        Font = new Font("Segoe UI", 10);
        BackColor = Color.WhiteSmoke;

        Controls.Add(new Label {
            Text = "BẢNG TÍNH TIỀN DỊCH VỤ", Font = new Font("Segoe UI", 18, FontStyle.Bold),
            ForeColor = Color.DarkBlue, AutoSize = true, Location = new Point(260, 20)
        });
        Controls.Add(new Label { Text = "Chọn loại dịch vụ:", AutoSize = true, Location = new Point(40, 85) });

        cboCategory.DropDownStyle = ComboBoxStyle.DropDownList;
        cboCategory.SetBounds(190, 80, 300, 32);
        cboCategory.Items.AddRange(new object[] { "Khám bệnh", "Xét nghiệm", "Chụp X-Quang", "Vắc-xin" });
        cboCategory.SelectedIndexChanged += cboCategory_SelectedIndexChanged;
        Controls.Add(cboCategory);

        var grpAvailable = new GroupBox { Text = "Dịch vụ hiện có", Location = new Point(35, 135), Size = new Size(340, 300) };
        lstAvailableServices.SetBounds(15, 30, 310, 250);
        lstAvailableServices.IntegralHeight = false;
        lstAvailableServices.DoubleClick += (_, _) => ChuyenDichVuDaChon();
        grpAvailable.Controls.Add(lstAvailableServices);
        Controls.Add(grpAvailable);

        var grpSelected = new GroupBox { Text = "Dịch vụ đã chọn", Location = new Point(525, 135), Size = new Size(340, 300) };
        lstSelectedServices.SetBounds(15, 30, 310, 250);
        lstSelectedServices.IntegralHeight = false;
        grpSelected.Controls.Add(lstSelectedServices);
        Controls.Add(grpSelected);

        AddButton(">", 410, 205, (_, _) => ChuyenDichVuDaChon());
        AddButton("<", 410, 265, (_, _) => XoaDichVuDangChon());
        AddButton("<<", 410, 325, (_, _) => { lstSelectedServices.Items.Clear(); CapNhatThanhTien(); });

        var grpPayment = new GroupBox { Text = "Thông tin thanh toán", Location = new Point(35, 455), Size = new Size(830, 125) };
        AddPaymentLabel(grpPayment, "Tổng tiền chưa giảm:", 25, 30, lblTotal);
        AddPaymentLabel(grpPayment, "Tỷ lệ chiết khấu:", 300, 30, lblDiscount);
        AddPaymentLabel(grpPayment, "Thành tiền thanh toán:", 555, 30, lblPayment);
        grpPayment.Controls.Add(new Label {
            Text = "Chiết khấu: từ 500.000đ giảm 5%; từ 1.000.000đ giảm 10%.",
            AutoSize = true, ForeColor = Color.DimGray, Location = new Point(25, 82)
        });
        Controls.Add(grpPayment);
    }

    private void AddButton(string text, int x, int y, EventHandler click)
    {
        var button = new Button { Text = text, Font = new Font("Segoe UI", 14, FontStyle.Bold) };
        button.SetBounds(x, y, 80, 45);
        button.Click += click;
        Controls.Add(button);
    }

    private static void AddPaymentLabel(GroupBox parent, string title, int x, int y, Label value)
    {
        parent.Controls.Add(new Label {
            Text = title, AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold),
            Location = new Point(x, y)
        });
        value.Text = "0 đ";
        value.AutoSize = true;
        value.ForeColor = Color.DarkBlue;
        value.Font = new Font("Segoe UI", 11, FontStyle.Bold);
        value.Location = new Point(x, y + 32);
        parent.Controls.Add(value);
    }

    private void cboCategory_SelectedIndexChanged(object? sender, EventArgs e)
    {
        lstAvailableServices.Items.Clear();
        string? category = cboCategory.SelectedItem?.ToString();
        if (category != null && danhSachDichVu.TryGetValue(category, out var services))
            foreach (var service in services) lstAvailableServices.Items.Add(service);
    }

    private void ChuyenDichVuDaChon()
    {
        if (lstAvailableServices.SelectedItem is not DichVu service)
        {
            MessageBox.Show("Vui lòng chọn một dịch vụ!", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        bool exists = lstSelectedServices.Items.Cast<DichVu>().Any(x => x.Ten == service.Ten);
        if (!exists) lstSelectedServices.Items.Add(service);
        CapNhatThanhTien();
    }

    private void XoaDichVuDangChon()
    {
        if (lstSelectedServices.SelectedItem == null)
        {
            MessageBox.Show("Vui lòng chọn dịch vụ cần xóa!", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        lstSelectedServices.Items.Remove(lstSelectedServices.SelectedItem);
        CapNhatThanhTien();
    }

    private void CapNhatThanhTien()
    {
        decimal total = 0;
        foreach (DichVu service in lstSelectedServices.Items) total += service.DonGia;

        decimal discountRate = total >= 1000000 ? 10m : total >= 500000 ? 5m : 0m;
        decimal payment = total * (1 - discountRate / 100m);

        lblTotal.Text = $"{total:N0} đ";
        lblDiscount.Text = $"{discountRate:N0}%";
        lblPayment.Text = $"{payment:N0} đ";
    }
}
