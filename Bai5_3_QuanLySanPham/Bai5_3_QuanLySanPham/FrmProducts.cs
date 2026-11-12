using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Bai5_3_QuanLySanPham
{
    public partial class FrmProducts : Form
    {
        private readonly List<Product> _products = new List<Product>();
        private readonly BindingSource _bs = new BindingSource();

        public FrmProducts()
        {
            InitializeComponent();
        }

        private void FrmProducts_Load(object sender, EventArgs e)
        {
            cboCategory.Items.AddRange(new object[] { "Điện tử", "Gia dụng", "Thời trang", "Thực phẩm", "Khác" });
            cboCategory.SelectedIndex = 0;

            // Dữ liệu mẫu
            _products.Add(new Product("SP001", "Laptop Dell", 15000000m, 5, "Điện tử"));
            _products.Add(new Product("SP002", "Nồi cơm điện", 1200000m, 20, "Gia dụng"));
            _products.Add(new Product("SP003", "Áo thun", 150000m, 100, "Thời trang"));

            _bs.DataSource = _products;
            dgvProducts.DataSource = _bs;

            dgvProducts.Columns["ProductId"].HeaderText = "Mã SP";
            dgvProducts.Columns["ProductName"].HeaderText = "Tên SP";
            dgvProducts.Columns["UnitPrice"].HeaderText = "Đơn giá";
            dgvProducts.Columns["UnitPrice"].DefaultCellStyle.Format = "N0";
            dgvProducts.Columns["Quantity"].HeaderText = "Số lượng";
            dgvProducts.Columns["Category"].HeaderText = "Danh mục";
        }

        // ---------- Tiện ích ----------
        private void ShowAll()
        {
            _bs.DataSource = _products;
            _bs.ResetBindings(false);
            dgvProducts.ClearSelection();
        }

        private void ClearInputs()
        {
            txtProductId.Clear();
            txtProductName.Clear();
            txtUnitPrice.Clear();
            nudQuantity.Value = 0;
            cboCategory.SelectedIndex = 0;
            txtProductId.ReadOnly = false;
            txtProductId.Focus();
        }

        private bool ReadInputs(out Product p)
        {
            p = null;
            string id = txtProductId.Text.Trim();
            string name = txtProductName.Text.Trim();

            if (id == "")
            {
                MessageBox.Show("Vui lòng nhập Mã SP.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtProductId.Focus();
                return false;
            }
            if (name == "")
            {
                MessageBox.Show("Vui lòng nhập Tên SP.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtProductName.Focus();
                return false;
            }
            if (!decimal.TryParse(txtUnitPrice.Text.Trim(), out decimal price) || price < 0)
            {
                MessageBox.Show("Đơn giá phải là số không âm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUnitPrice.Focus();
                return false;
            }

            p = new Product(id, name, price, (int)nudQuantity.Value, cboCategory.Text);
            return true;
        }

        // ---------- Thêm ----------
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ReadInputs(out Product p)) return;

            if (_products.Any(x => x.ProductId.Equals(p.ProductId, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Mã SP đã tồn tại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtProductId.Focus();
                return;
            }

            _products.Add(p);
            txtSearch.Clear();
            ShowAll();
            ClearInputs();
        }

        // ---------- Click dòng -> đẩy dữ liệu lên ô nhập ----------
        private void dgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dgvProducts.Rows[e.RowIndex].DataBoundItem is Product p)
            {
                txtProductId.Text = p.ProductId;
                txtProductName.Text = p.ProductName;
                txtUnitPrice.Text = p.UnitPrice.ToString();
                nudQuantity.Value = p.Quantity;
                cboCategory.Text = p.Category;
                txtProductId.ReadOnly = true; // không cho đổi khóa khi sửa
            }
        }

        // ---------- Sửa ----------
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (!(_bs.Current is Product current))
            {
                MessageBox.Show("Hãy chọn một dòng để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (!ReadInputs(out Product p)) return;

            current.ProductName = p.ProductName;
            current.UnitPrice = p.UnitPrice;
            current.Quantity = p.Quantity;
            current.Category = p.Category;

            _bs.ResetBindings(false);
        }

        // ---------- Xóa ----------
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!(_bs.Current is Product current))
            {
                MessageBox.Show("Hãy chọn một dòng để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show(
                $"Bạn có chắc muốn xóa sản phẩm \"{current.ProductName}\"?",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                _products.Remove(current);
                txtSearch.Clear();
                ShowAll();
                ClearInputs();
            }
        }

        // ---------- Tìm kiếm theo Tên SP ----------
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            if (keyword == "") { ShowAll(); return; }

            var found = _products
                .Where(p => p.ProductName.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();

            _bs.DataSource = found;
            _bs.ResetBindings(false);

            if (found.Count == 0)
                MessageBox.Show("Không tìm thấy sản phẩm nào.", "Kết quả", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnShowAll_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            ShowAll();
            ClearInputs();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnSearch_Click(sender, e);
            }
        }
    }
}
