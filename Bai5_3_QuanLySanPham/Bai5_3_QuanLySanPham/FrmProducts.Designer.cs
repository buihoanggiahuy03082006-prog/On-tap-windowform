namespace Bai5_3_QuanLySanPham
{
    partial class FrmProducts
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.grpInfo = new System.Windows.Forms.GroupBox();
            this.lblId = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.lblPrice = new System.Windows.Forms.Label();
            this.lblQty = new System.Windows.Forms.Label();
            this.lblCategory = new System.Windows.Forms.Label();
            this.txtProductId = new System.Windows.Forms.TextBox();
            this.txtProductName = new System.Windows.Forms.TextBox();
            this.txtUnitPrice = new System.Windows.Forms.TextBox();
            this.nudQuantity = new System.Windows.Forms.NumericUpDown();
            this.cboCategory = new System.Windows.Forms.ComboBox();
            this.grpActions = new System.Windows.Forms.GroupBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.lblSearch = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnShowAll = new System.Windows.Forms.Button();
            this.dgvProducts = new System.Windows.Forms.DataGridView();
            this.grpInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudQuantity)).BeginInit();
            this.grpActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).BeginInit();
            this.SuspendLayout();

            // grpInfo
            this.grpInfo.Text = "Thông tin sản phẩm";
            this.grpInfo.Location = new System.Drawing.Point(12, 12);
            this.grpInfo.Size = new System.Drawing.Size(420, 220);
            this.grpInfo.Controls.AddRange(new System.Windows.Forms.Control[] {
                lblId, txtProductId, lblName, txtProductName, lblPrice, txtUnitPrice,
                lblQty, nudQuantity, lblCategory, cboCategory });

            this.lblId.Text = "Mã SP:";
            this.lblId.Location = new System.Drawing.Point(15, 32);
            this.lblId.AutoSize = true;
            this.txtProductId.Location = new System.Drawing.Point(110, 29);
            this.txtProductId.Size = new System.Drawing.Size(290, 23);

            this.lblName.Text = "Tên SP:";
            this.lblName.Location = new System.Drawing.Point(15, 72);
            this.lblName.AutoSize = true;
            this.txtProductName.Location = new System.Drawing.Point(110, 69);
            this.txtProductName.Size = new System.Drawing.Size(290, 23);

            this.lblPrice.Text = "Đơn giá:";
            this.lblPrice.Location = new System.Drawing.Point(15, 112);
            this.lblPrice.AutoSize = true;
            this.txtUnitPrice.Location = new System.Drawing.Point(110, 109);
            this.txtUnitPrice.Size = new System.Drawing.Size(290, 23);

            this.lblQty.Text = "Số lượng:";
            this.lblQty.Location = new System.Drawing.Point(15, 152);
            this.lblQty.AutoSize = true;
            this.nudQuantity.Location = new System.Drawing.Point(110, 149);
            this.nudQuantity.Size = new System.Drawing.Size(120, 23);
            this.nudQuantity.Maximum = 1000000;

            this.lblCategory.Text = "Danh mục:";
            this.lblCategory.Location = new System.Drawing.Point(15, 188);
            this.lblCategory.AutoSize = true;
            this.cboCategory.Location = new System.Drawing.Point(110, 185);
            this.cboCategory.Size = new System.Drawing.Size(290, 23);
            this.cboCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            // grpActions
            this.grpActions.Text = "Chức năng";
            this.grpActions.Location = new System.Drawing.Point(445, 12);
            this.grpActions.Size = new System.Drawing.Size(340, 220);
            this.grpActions.Controls.AddRange(new System.Windows.Forms.Control[] {
                btnAdd, btnEdit, btnDelete, lblSearch, txtSearch, btnSearch, btnShowAll });

            this.btnAdd.Text = "Thêm";
            this.btnAdd.Location = new System.Drawing.Point(15, 30);
            this.btnAdd.Size = new System.Drawing.Size(95, 32);
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);

            this.btnEdit.Text = "Sửa";
            this.btnEdit.Location = new System.Drawing.Point(122, 30);
            this.btnEdit.Size = new System.Drawing.Size(95, 32);
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);

            this.btnDelete.Text = "Xóa";
            this.btnDelete.Location = new System.Drawing.Point(229, 30);
            this.btnDelete.Size = new System.Drawing.Size(95, 32);
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);

            this.lblSearch.Text = "Tìm theo Tên SP:";
            this.lblSearch.Location = new System.Drawing.Point(15, 90);
            this.lblSearch.AutoSize = true;
            this.txtSearch.Location = new System.Drawing.Point(15, 112);
            this.txtSearch.Size = new System.Drawing.Size(309, 23);
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);

            this.btnSearch.Text = "Tìm kiếm";
            this.btnSearch.Location = new System.Drawing.Point(15, 148);
            this.btnSearch.Size = new System.Drawing.Size(150, 32);
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);

            this.btnShowAll.Text = "Hiện tất cả";
            this.btnShowAll.Location = new System.Drawing.Point(174, 148);
            this.btnShowAll.Size = new System.Drawing.Size(150, 32);
            this.btnShowAll.Click += new System.EventHandler(this.btnShowAll_Click);

            // dgvProducts
            this.dgvProducts.Name = "dgvProducts";
            this.dgvProducts.Location = new System.Drawing.Point(12, 245);
            this.dgvProducts.Size = new System.Drawing.Size(773, 300);
            this.dgvProducts.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom
                | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.dgvProducts.AllowUserToAddRows = false;
            this.dgvProducts.AllowUserToDeleteRows = false;
            this.dgvProducts.ReadOnly = true;
            this.dgvProducts.MultiSelect = false;
            this.dgvProducts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProducts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvProducts.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProducts_CellClick);

            // FrmProducts
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 560);
            this.Controls.Add(this.grpInfo);
            this.Controls.Add(this.grpActions);
            this.Controls.Add(this.dgvProducts);
            this.Text = "Bài 5.3 - Quản lý danh sách sản phẩm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.FrmProducts_Load);

            this.grpInfo.ResumeLayout(false);
            this.grpInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudQuantity)).EndInit();
            this.grpActions.ResumeLayout(false);
            this.grpActions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.GroupBox grpInfo;
        private System.Windows.Forms.GroupBox grpActions;
        private System.Windows.Forms.Label lblId;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.Label lblQty;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtProductId;
        private System.Windows.Forms.TextBox txtProductName;
        private System.Windows.Forms.TextBox txtUnitPrice;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.NumericUpDown nudQuantity;
        private System.Windows.Forms.ComboBox cboCategory;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnShowAll;
        private System.Windows.Forms.DataGridView dgvProducts;
    }
}
