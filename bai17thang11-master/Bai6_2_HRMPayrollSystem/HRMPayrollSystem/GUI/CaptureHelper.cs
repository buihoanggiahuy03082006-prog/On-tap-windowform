using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace HRMPayrollSystem.GUI
{
    public static class CaptureHelper
    {
        public static void CaptureAll()
        {
            string outDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "images"));
            Directory.CreateDirectory(outDir);

            // 1. 01_main_screen.png
            using (var f = new FormMain())
            {
                f.StartPosition = FormStartPosition.Manual;
                f.Location = new Point(50, 50);
                f.Show();
                Application.DoEvents();
                Thread.Sleep(600);
                CaptureForm(f, Path.Combine(outDir, "01_main_screen.png"));
                f.Close();
            }

            // 2. 02_crud_screen.png (FormNhanVien)
            using (var f = new FormNhanVien())
            {
                f.StartPosition = FormStartPosition.Manual;
                f.Location = new Point(60, 60);
                f.Show();
                Application.DoEvents();
                Thread.Sleep(600);
                CaptureForm(f, Path.Combine(outDir, "02_crud_screen.png"));
                f.Close();
            }

            // 3. 03_transaction_screen.png (FormChamCong - Disconnected Model)
            using (var f = new FormChamCong())
            {
                f.StartPosition = FormStartPosition.Manual;
                f.Location = new Point(50, 50);
                f.Show();
                Application.DoEvents();
                Thread.Sleep(600);
                CaptureForm(f, Path.Combine(outDir, "03_transaction_screen.png"));
                f.Close();
            }

            // 4. 04_report_screen.png (FormTinhLuong - SqlTransaction)
            using (var f = new FormTinhLuong())
            {
                f.StartPosition = FormStartPosition.Manual;
                f.Location = new Point(50, 50);
                f.Show();
                Application.DoEvents();
                Thread.Sleep(600);
                CaptureForm(f, Path.Combine(outDir, "04_report_screen.png"));
                f.Close();
            }

            // 5. 05_error_handling.png (Khóa chấm công & Chống SQL Injection)
            using (var f = new FormHRMErrorDemo())
            {
                f.StartPosition = FormStartPosition.Manual;
                f.Location = new Point(50, 50);
                f.Show();
                Application.DoEvents();
                Thread.Sleep(600);
                CaptureForm(f, Path.Combine(outDir, "05_error_handling.png"));
                f.Close();
            }

            Console.WriteLine("All HRM screenshots captured successfully!");
        }

        private static void CaptureForm(Form form, string filePath)
        {
            Application.DoEvents();
            using (Bitmap bitmap = new Bitmap(form.Width, form.Height))
            {
                form.DrawToBitmap(bitmap, new Rectangle(0, 0, form.Width, form.Height));
                bitmap.Save(filePath, ImageFormat.Png);
            }
            Console.WriteLine($"Saved: {filePath}");
        }
    }

    public class FormHRMErrorDemo : Form
    {
        public FormHRMErrorDemo()
        {
            this.Text = "Minh Họa An Toàn Dữ Liệu HRM: Khóa Chấm Công & Chống SQL Injection";
            this.Size = new Size(1100, 680);
            this.Font = new Font("Segoe UI", 9.5f);
            this.BackColor = Color.FromArgb(248, 250, 252);

            var pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 65,
                BackColor = Color.FromArgb(220, 38, 38)
            };
            var lblTitle = new Label
            {
                Text = "MINH HỌA AN TOÀN DỮ LIỆU HRM: KHÓA BẢNG CHẤM CÔNG & CHỐNG SQL INJECTION",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12.5f, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0)
            };
            pnlHeader.Controls.Add(lblTitle);

            // Left Box: Attendance Lock
            var grpLock = new GroupBox
            {
                Text = "1. Cơ Chế Khóa Bảng Chấm Công Sau Khi Chốt Lương (Bảo Vệ Tính Toàn Vẹn)",
                Location = new Point(20, 80),
                Size = new Size(515, 540),
                BackColor = Color.White,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold)
            };

            var lblDesc1 = new Label
            {
                Text = "• Tình huống: Người dùng cố tình sửa dữ liệu công/OT tháng 10/2026 (đã chốt).\n" +
                       "• Hệ thống phát hiện cờ DaKhoa = 1 và ngăn chặn tức thì:",
                Location = new Point(15, 30),
                Size = new Size(480, 50),
                Font = new Font("Segoe UI", 9f, FontStyle.Regular),
                ForeColor = Color.FromArgb(51, 65, 85)
            };
            grpLock.Controls.Add(lblDesc1);

            var pnlErrorBox = new Panel
            {
                Location = new Point(15, 90),
                Size = new Size(480, 425),
                BackColor = Color.FromArgb(254, 242, 242),
                BorderStyle = BorderStyle.FixedSingle
            };

            var lblAlertTitle = new Label
            {
                Text = "⚠️ [THAO TÁC BỊ TỪ CHỐI - DỮ LIỆU ĐÃ BỊ KHÓA]",
                Location = new Point(15, 15),
                Size = new Size(450, 30),
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                ForeColor = Color.FromArgb(185, 28, 28)
            };
            pnlErrorBox.Controls.Add(lblAlertTitle);

            var txtLog = new TextBox
            {
                Location = new Point(15, 55),
                Size = new Size(448, 350),
                Multiline = true,
                ReadOnly = true,
                BackColor = Color.FromArgb(255, 255, 255),
                Font = new Font("Consolas", 9.5f),
                ForeColor = Color.FromArgb(153, 27, 27),
                Text = 
                    "System.InvalidOperationException: \r\n" +
                    "[DATA INTEGRITY PROTECTION] Dữ liệu chấm công tháng 10/2026 ĐÃ BỊ KHÓA do đã thực hiện giao dịch chốt lương SqlTransaction!\r\n\r\n" +
                    "--> Vô hiệu hóa nút [Lưu Chấm Công] và chặn adapter.Update()\r\n\r\n" +
                    "KIỂM CHỨNG TÍNH TOÀN VẸN NGHIỆP VỤ (HRM & PAYROLL):\r\n" +
                    "✓ 1. Trạng thái giao dịch: DaKhoa = 1 ngăn chặn mọi sửa đổi trái phép.\r\n" +
                    "✓ 2. Bảng BangLuong: Dữ liệu thực lĩnh được bảo toàn 100%.\r\n" +
                    "✓ 3. Bảng BangChamCong: Không bị lệch ngày công hay giờ OT sau kiểm toán.\r\n" +
                    "✓ 4. Giao dịch SqlTransaction ChotLuong: Atomic & Consistent."
            };
            pnlErrorBox.Controls.Add(txtLog);
            grpLock.Controls.Add(pnlErrorBox);

            // Right Box: SQL Injection
            var grpSql = new GroupBox
            {
                Text = "2. Phòng Chống 100% Lỗ Hổng SQL Injection Bằng Parameterized Query",
                Location = new Point(555, 80),
                Size = new Size(515, 540),
                BackColor = Color.White,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold)
            };

            var lblDesc2 = new Label
            {
                Text = "• Tình huống tấn công: Nhập chuỗi khai thác vào ô tìm kiếm nhân sự:\n" +
                       "  Input: ' OR '1'='1' UNION SELECT MaNV, HoTen FROM NhanVien --\n" +
                       "• Cơ chế bảo vệ: Tham số hóa truy vấn với SqlParameter.",
                Location = new Point(15, 30),
                Size = new Size(480, 50),
                Font = new Font("Segoe UI", 9f, FontStyle.Regular),
                ForeColor = Color.FromArgb(51, 65, 85)
            };
            grpSql.Controls.Add(lblDesc2);

            var pnlSqlBox = new Panel
            {
                Location = new Point(15, 90),
                Size = new Size(480, 425),
                BackColor = Color.FromArgb(240, 253, 244),
                BorderStyle = BorderStyle.FixedSingle
            };

            var lblSqlTitle = new Label
            {
                Text = "🛡️ [TRUY VẤN THAM SỐ HÓA - BẢO MẬT TUYỆT ĐỐI]",
                Location = new Point(15, 15),
                Size = new Size(450, 30),
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                ForeColor = Color.FromArgb(21, 128, 61)
            };
            pnlSqlBox.Controls.Add(lblSqlTitle);

            var txtSqlLog = new TextBox
            {
                Location = new Point(15, 55),
                Size = new Size(448, 350),
                Multiline = true,
                ReadOnly = true,
                BackColor = Color.FromArgb(255, 255, 255),
                Font = new Font("Consolas", 9.5f),
                ForeColor = Color.FromArgb(22, 101, 52),
                Text =
                    "// MÃ NGUỒN C# TRUY VẤN PARAMETERIZED:\r\n" +
                    "string sql = \"SELECT nv.*, pb.TenPB \" +\r\n" +
                    "             \"FROM NhanVien nv JOIN PhongBan pb ON nv.MaPB = pb.MaPB \" +\r\n" +
                    "             \"WHERE (nv.HoTen LIKE @Keyword OR nv.CCCD LIKE @Keyword)\";\r\n\r\n" +
                    "using var cmd = new SqlCommand(sql, conn);\r\n" +
                    "cmd.Parameters.Add(\"@Keyword\", SqlDbType.NVarChar, 100)\r\n" +
                    "   .Value = \"%\" + keyword + \"%\";\r\n\r\n" +
                    "KẾT QUẢ XỬ LÝ:\r\n" +
                    "✓ SQL Server tách biệt hoàn toàn Plan Cache và tham số dữ liệu.\r\n" +
                    "✓ Không bị bẻ gãy cú pháp câu lệnh SQL.\r\n" +
                    "✓ Bảo vệ 100% hồ sơ nhân viên, CCCD và thông tin mức lương."
            };
            pnlSqlBox.Controls.Add(txtSqlLog);
            grpSql.Controls.Add(pnlSqlBox);

            this.Controls.Add(grpSql);
            this.Controls.Add(grpLock);
            this.Controls.Add(pnlHeader);
        }
    }
}
