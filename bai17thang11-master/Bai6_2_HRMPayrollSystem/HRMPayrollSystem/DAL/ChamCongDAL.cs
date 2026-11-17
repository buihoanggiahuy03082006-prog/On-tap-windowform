using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace HRMPayrollSystem.DAL
{
    public class ChamCongDAL
    {
        /// <summary>
        /// Đảm bảo mọi nhân viên hiện có đều có bản ghi chấm công trong tháng
        /// </summary>
        public void KhoiTaoChamCongThang(string thangNam)
        {
            const string sqlInit = @"
                INSERT INTO BangChamCong (MaNV, ThangNam, SoNgayCong, SoGioOT, DaKhoa)
                SELECT nv.MaNV, @ThangNam, 22, 0, 0
                FROM NhanVien nv
                WHERE NOT EXISTS (
                    SELECT 1 FROM BangChamCong c 
                    WHERE c.MaNV = nv.MaNV AND c.ThangNam = @ThangNam
                )";

            using var conn = DatabaseHelper.CreateConnection();
            using var cmd = new SqlCommand(sqlInit, conn);
            cmd.Parameters.Add("@ThangNam", SqlDbType.VarChar, 7).Value = thangNam;
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Lấy SqlDataAdapter và DataTable theo mô hình ngắt kết nối (Disconnected Model)
        /// </summary>
        public (SqlDataAdapter Adapter, DataTable Table, SqlConnection Connection) GetChamCongDisconnected(string thangNam)
        {
            KhoiTaoChamCongThang(thangNam);

            var conn = DatabaseHelper.CreateConnection();
            const string sql = @"
                SELECT c.MaCong, c.MaNV, nv.HoTen, pb.TenPB, c.ThangNam, c.SoNgayCong, c.SoGioOT, c.DaKhoa
                FROM BangChamCong c
                JOIN NhanVien nv ON c.MaNV = nv.MaNV
                JOIN PhongBan pb ON nv.MaPB = pb.MaPB
                WHERE c.ThangNam = @ThangNam
                ORDER BY c.MaNV";

            var adapter = new SqlDataAdapter(sql, conn);
            adapter.SelectCommand!.Parameters.Add("@ThangNam", SqlDbType.VarChar, 7).Value = thangNam;

            // Thiết lập UpdateCommand tường minh với SqlParameter (Đồng bộ Disconnected Model về SQL Server)
            const string sqlUpdate = @"
                UPDATE BangChamCong 
                SET SoNgayCong = @SoNgayCong, SoGioOT = @SoGioOT 
                WHERE MaCong = @MaCong AND DaKhoa = 0";

            var cmdUpdate = new SqlCommand(sqlUpdate, conn);
            cmdUpdate.Parameters.Add("@SoNgayCong", SqlDbType.Float, 8, "SoNgayCong");
            cmdUpdate.Parameters.Add("@SoGioOT", SqlDbType.Float, 8, "SoGioOT");
            cmdUpdate.Parameters.Add("@MaCong", SqlDbType.Int, 4, "MaCong");
            adapter.UpdateCommand = cmdUpdate;

            var table = new DataTable("BangChamCong");
            adapter.Fill(table);

            return (adapter, table, conn);
        }

        /// <summary>
        /// Đồng bộ dữ liệu chấm công từ DataTable ngắt kết nối về SQL Server trong 1 thao tác
        /// </summary>
        public int LuuChamCong(SqlDataAdapter adapter, DataTable table)
        {
            return adapter.Update(table);
        }

        /// <summary>
        /// Kiểm tra tháng đã chốt lương và khóa dữ liệu chấm công chưa
        /// </summary>
        public bool KiemTraDaKhoa(string thangNam)
        {
            const string sql = "SELECT COUNT(*) FROM BangChamCong WHERE ThangNam = @ThangNam AND DaKhoa = 1";
            using var conn = DatabaseHelper.CreateConnection();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@ThangNam", SqlDbType.VarChar, 7).Value = thangNam;
            conn.Open();
            int count = Convert.ToInt32(cmd.ExecuteScalar());
            return count > 0;
        }
    }
}
