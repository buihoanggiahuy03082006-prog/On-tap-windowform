namespace bai4
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblTieuDe = new System.Windows.Forms.Label();
            this.lblKhungGio = new System.Windows.Forms.Label();
            this.cboKhungGio = new System.Windows.Forms.ComboBox();
            this.lblSoViTri = new System.Windows.Forms.Label();
            this.lblTamTinh = new System.Windows.Forms.Label();
            this.tlpViTri = new System.Windows.Forms.TableLayoutPanel();
            this.btnXacNhan = new System.Windows.Forms.Button();
            this.btnHuyChon = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblTieuDe
            // 
            this.lblTieuDe.AutoSize = true;
            this.lblTieuDe.Location = new System.Drawing.Point(120, 63);
            this.lblTieuDe.Name = "lblTieuDe";
            this.lblTieuDe.Size = new System.Drawing.Size(118, 16);
            this.lblTieuDe.TabIndex = 0;
            this.lblTieuDe.Text = "SƠ ĐỒ ĐẶT VỊ TRÍ";
            // 
            // lblKhungGio
            // 
            this.lblKhungGio.AutoSize = true;
            this.lblKhungGio.Location = new System.Drawing.Point(120, 100);
            this.lblKhungGio.Name = "lblKhungGio";
            this.lblKhungGio.Size = new System.Drawing.Size(69, 16);
            this.lblKhungGio.TabIndex = 1;
            this.lblKhungGio.Text = "Khung giờ:";
            // 
            // cboKhungGio
            // 
            this.cboKhungGio.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboKhungGio.FormattingEnabled = true;
            this.cboKhungGio.Location = new System.Drawing.Point(341, 100);
            this.cboKhungGio.Name = "cboKhungGio";
            this.cboKhungGio.Size = new System.Drawing.Size(121, 24);
            this.cboKhungGio.TabIndex = 3;
            // 
            // lblSoViTri
            // 
            this.lblSoViTri.AutoSize = true;
            this.lblSoViTri.Location = new System.Drawing.Point(120, 264);
            this.lblSoViTri.Name = "lblSoViTri";
            this.lblSoViTri.Size = new System.Drawing.Size(129, 16);
            this.lblSoViTri.TabIndex = 4;
            this.lblSoViTri.Text = "Số vị trí đang chọn: 0";
            // 
            // lblTamTinh
            // 
            this.lblTamTinh.AutoSize = true;
            this.lblTamTinh.Location = new System.Drawing.Point(120, 323);
            this.lblTamTinh.Name = "lblTamTinh";
            this.lblTamTinh.Size = new System.Drawing.Size(106, 16);
            this.lblTamTinh.TabIndex = 5;
            this.lblTamTinh.Text = "Tạm tính tiền: 0 đ";
            // 
            // tlpViTri
            // 
            this.tlpViTri.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tlpViTri.ColumnCount = 5;
            this.tlpViTri.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpViTri.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpViTri.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpViTri.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpViTri.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpViTri.Location = new System.Drawing.Point(341, 150);
            this.tlpViTri.Name = "tlpViTri";
            this.tlpViTri.RowCount = 4;
            this.tlpViTri.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpViTri.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpViTri.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpViTri.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpViTri.Size = new System.Drawing.Size(233, 100);
            this.tlpViTri.TabIndex = 6;
            // 
            // btnXacNhan
            // 
            this.btnXacNhan.Location = new System.Drawing.Point(123, 360);
            this.btnXacNhan.Name = "btnXacNhan";
            this.btnXacNhan.Size = new System.Drawing.Size(75, 23);
            this.btnXacNhan.TabIndex = 7;
            this.btnXacNhan.Text = "Xác nhận đặt";
            this.btnXacNhan.UseVisualStyleBackColor = true;
            // 
            // btnHuyChon
            // 
            this.btnHuyChon.Location = new System.Drawing.Point(251, 360);
            this.btnHuyChon.Name = "btnHuyChon";
            this.btnHuyChon.Size = new System.Drawing.Size(75, 23);
            this.btnHuyChon.TabIndex = 8;
            this.btnHuyChon.Text = "Hủy chọn tất cả";
            this.btnHuyChon.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnHuyChon);
            this.Controls.Add(this.btnXacNhan);
            this.Controls.Add(this.tlpViTri);
            this.Controls.Add(this.lblTamTinh);
            this.Controls.Add(this.lblSoViTri);
            this.Controls.Add(this.cboKhungGio);
            this.Controls.Add(this.lblKhungGio);
            this.Controls.Add(this.lblTieuDe);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTieuDe;
        private System.Windows.Forms.Label lblKhungGio;
        private System.Windows.Forms.ComboBox cboKhungGio;
        private System.Windows.Forms.Label lblSoViTri;
        private System.Windows.Forms.Label lblTamTinh;
        private System.Windows.Forms.TableLayoutPanel tlpViTri;
        private System.Windows.Forms.Button btnXacNhan;
        private System.Windows.Forms.Button btnHuyChon;
    }
}

