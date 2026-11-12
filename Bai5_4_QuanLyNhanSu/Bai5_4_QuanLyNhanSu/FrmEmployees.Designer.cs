namespace Bai5_4_QuanLyNhanSu
{
    partial class FrmEmployees
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.imgSmall = new System.Windows.Forms.ImageList(this.components);
            this.imgLarge = new System.Windows.Forms.ImageList(this.components);
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblView = new System.Windows.Forms.Label();
            this.cboView = new System.Windows.Forms.ComboBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.tvDepartments = new System.Windows.Forms.TreeView();
            this.lsvEmployees = new System.Windows.Forms.ListView();
            this.colId = new System.Windows.Forms.ColumnHeader();
            this.colName = new System.Windows.Forms.ColumnHeader();
            this.colPosition = new System.Windows.Forms.ColumnHeader();
            this.colStartDate = new System.Windows.Forms.ColumnHeader();
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            this.SuspendLayout();

            // imgSmall / imgLarge (icon được vẽ bằng code trong FrmEmployees.cs)
            this.imgSmall.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imgSmall.ImageSize = new System.Drawing.Size(16, 16);
            this.imgLarge.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imgLarge.ImageSize = new System.Drawing.Size(32, 32);

            // pnlTop
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Height = 44;
            this.pnlTop.Controls.Add(this.lblView);
            this.pnlTop.Controls.Add(this.cboView);
            this.pnlTop.Controls.Add(this.lblStatus);

            this.lblView.Text = "Chế độ xem:";
            this.lblView.AutoSize = true;
            this.lblView.Location = new System.Drawing.Point(12, 14);

            this.cboView.Location = new System.Drawing.Point(95, 10);
            this.cboView.Size = new System.Drawing.Size(140, 23);
            this.cboView.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboView.SelectedIndexChanged += new System.EventHandler(this.cboView_SelectedIndexChanged);

            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(260, 14);
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);

            // splitMain
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.SplitterDistance = 260;
            this.splitMain.Panel1.Controls.Add(this.tvDepartments);
            this.splitMain.Panel2.Controls.Add(this.lsvEmployees);

            // tvDepartments
            this.tvDepartments.Name = "tvDepartments";
            this.tvDepartments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvDepartments.ImageList = this.imgSmall;
            this.tvDepartments.HideSelection = false;
            this.tvDepartments.ShowLines = true;
            this.tvDepartments.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvDepartments_AfterSelect);

            // lsvEmployees
            this.lsvEmployees.Name = "lsvEmployees";
            this.lsvEmployees.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lsvEmployees.View = System.Windows.Forms.View.Details;
            this.lsvEmployees.FullRowSelect = true;
            this.lsvEmployees.GridLines = true;
            this.lsvEmployees.HideSelection = false;
            this.lsvEmployees.ShowItemToolTips = true;
            this.lsvEmployees.SmallImageList = this.imgSmall;
            this.lsvEmployees.LargeImageList = this.imgLarge;
            this.lsvEmployees.TileSize = new System.Drawing.Size(240, 44);
            this.lsvEmployees.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                this.colId, this.colName, this.colPosition, this.colStartDate });

            this.colId.Text = "Mã NV";
            this.colId.Width = 90;
            this.colName.Text = "Họ Tên";
            this.colName.Width = 190;
            this.colPosition.Text = "Chức vụ";
            this.colPosition.Width = 170;
            this.colStartDate.Text = "Ngày vào làm";
            this.colStartDate.Width = 120;

            // FrmEmployees
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 560);
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.pnlTop);
            this.Text = "Bài 5.4 - Quản lý nhân sự (TreeView & ListView)";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.FrmEmployees_Load);

            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.ImageList imgSmall;
        private System.Windows.Forms.ImageList imgLarge;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblView;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ComboBox cboView;
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.TreeView tvDepartments;
        private System.Windows.Forms.ListView lsvEmployees;
        private System.Windows.Forms.ColumnHeader colId;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colPosition;
        private System.Windows.Forms.ColumnHeader colStartDate;
    }
}
