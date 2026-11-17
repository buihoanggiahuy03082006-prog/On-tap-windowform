using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using HRMPayrollSystem.DTO;

namespace HRMPayrollSystem.DAL
{
    public class TinhLuongDAL
    {
        /// <summary>
        /// Lấy bảng tính lương dự kiến cho một tháng dựa trên dữ liệu chấm công và hệ số lương
        /// Công thức: ThucLinh = (LuongCoBan * HeSoLuong / 22) * SoNgayCong + (SoGioOT * 50,000)
        /// </summary>
        public List<BangLuongDTO> TinhLuongDuKien(string thangNam)
        {
            var list = new List<BangLuongDTO>();
            const string sql = @"
                SELECT c.MaNV, nv.HoTen, pb.TenPB, c.ThangNam, nv.LuongCoBan, nv.HeSoLuong, 
                       c.SoNgayCong, c.SoGioOT
                FROM BangChamCong c
                JOIN NhanVien nv ON c.MaNV = nv.MaNV
                JOIN PhongBan pb ON nv.MaPB = pb.MaPB
                WHERE c.ThangNam = @ThangNam
                ORDER BY c.MaNV";

            using var conn = DatabaseHelper.CreateConnection();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@ThangNam", SqlDbType.VarChar, 7).Value = thangNam;
            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int maNV = reader.GetInt32(0);
                string hoTen = reader.GetString(1);
                string tenPB = reader.GetString(2);
                string thgNam = reader.GetString(3);
                decimal luongCoBan = reader.GetDecimal(4);
                double heSoLuong = reader.GetDouble(5);
                double soNgayCong = reader.GetDouble(6);
                double soGioOT = reader.GetDouble(7);

                // Áp dụng chuẩn công thức đề bài:
                // ThucLinh = (LuongCoBan * HeSoLuong / 22) * SoNgayCong + (SoGioOT * 50,000)
                decimal luongMotNgay = (luongCoBan * (decimal)heSoLuong) / 22m;
                decimal tienOT = (decimal)soGioOT * 50000m;
                decimal thucLinh = Math.Round((luongMotNgay * (decimal)soNgayCong) + tienOT, 0);

                list.Add(new BangLuongDTO
                {
                    MaNV = maNV,
                    HoTen = hoTen,
                    TenPB = tenPB,
                    ThangNam = thgNam,
                    LuongCoBan = luongCoBan,
                    HeSoLuong = heSoLuong,
                    SoNgayCong = soNgayCong,
                    SoGioOT = soGioOT,
                    ThucLinh = thucLinh
                });
            }
            return list;
        }

        /// <summary>
        /// Thực thi SqlTransaction: Chốt lương tháng cho toàn bộ nhân viên và khóa dữ liệu chấm công
        /// </summary>
        public (bool Success, string Message) ChotLuongThang(string thangNam, List<BangLuongDTO> danhSachLuong)
        {
            if (danhSachLuong == null || danhSachLuong.Count == 0)
            {
                return (false, "Không có dữ liệu nhân viên để chốt lương!");
            }

            using var conn = DatabaseHelper.CreateConnection();
            conn.Open();
            using var trans = conn.BeginTransaction();

            try
            {
                // BƯỚC 1: Xóa bảng lương cũ của tháng nếu đã tồn tại trước đó để ghi đè chuẩn xác
                const string sqlDeleteOld = "DELETE FROM BangLuong WHERE ThangNam = @ThangNam";
                using (var cmdDel = new SqlCommand(sqlDeleteOld, conn, trans))
                {
                    cmdDel.Parameters.Add("@ThangNam", SqlDbType.VarChar, 7).Value = thangNam;
                    cmdDel.ExecuteNonQuery();
                }

                // BƯỚC 2: Ghi nhận từng bản ghi vào BangLuong
                const string sqlInsertLuong = @"
                    INSERT INTO BangLuong (MaNV, ThangNam, ThucLinh) 
                    VALUES (@MaNV, @ThangNam, @ThucLinh)";

                foreach (var item in danhSachLuong)
                {
                    using var cmdInsert = new SqlCommand(sqlInsertLuong, conn, trans);
                    cmdInsert.Parameters.Add("@MaNV", SqlDbType.Int).Value = item.MaNV;
                    cmdInsert.Parameters.Add("@ThangNam", SqlDbType.VarChar, 7).Value = thangNam;
                    cmdInsert.Parameters.Add("@ThucLinh", SqlDbType.Decimal).Value = item.ThucLinh;
                    cmdInsert.ExecuteNonQuery();
                }

                // BƯỚC 3: Khóa dữ liệu chấm công của tháng (DaKhoa = 1)
                const string sqlLockChamCong = "UPDATE BangChamCong SET DaKhoa = 1 WHERE ThangNam = @ThangNam";
                using (var cmdLock = new SqlCommand(sqlLockChamCong, conn, trans))
                {
                    cmdLock.Parameters.Add("@ThangNam", SqlDbType.VarChar, 7).Value = thangNam;
                    cmdLock.ExecuteNonQuery();
                }

                // BƯỚC 4: Hoàn tất giao dịch (Commit)
                trans.Commit();
                return (true, $"Đã chốt lương thành công cho {danhSachLuong.Count} nhân viên trong tháng {thangNam} và khóa bảng chấm công!");
            }
            catch (Exception ex)
            {
                try
                {
                    trans.Rollback();
                }
                catch { }

                return (false, "Giao dịch chốt lương bị hủy (Rollback): " + ex.Message);
            }
        }

        /// <summary>
        /// Lấy bảng lương chính thức đã chốt
        /// </summary>
        public List<BangLuongDTO> GetBangLuongDaChot(string thangNam)
        {
            var list = new List<BangLuongDTO>();
            const string sql = @"
                SELECT bl.MaLuong, bl.MaNV, nv.HoTen, pb.TenPB, bl.ThangNam, 
                       nv.LuongCoBan, nv.HeSoLuong, c.SoNgayCong, c.SoGioOT, bl.ThucLinh
                FROM BangLuong bl
                JOIN NhanVien nv ON bl.MaNV = nv.MaNV
                JOIN PhongBan pb ON nv.MaPB = pb.MaPB
                LEFT JOIN BangChamCong c ON bl.MaNV = c.MaNV AND bl.ThangNam = c.ThangNam
                WHERE bl.ThangNam = @ThangNam
                ORDER BY bl.MaNV";

            using var conn = DatabaseHelper.CreateConnection();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@ThangNam", SqlDbType.VarChar, 7).Value = thangNam;
            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new BangLuongDTO
                {
                    MaLuong = reader.GetInt32(0),
                    MaNV = reader.GetInt32(1),
                    HoTen = reader.GetString(2),
                    TenPB = reader.GetString(3),
                    ThangNam = reader.GetString(4),
                    LuongCoBan = reader.GetDecimal(5),
                    HeSoLuong = reader.GetDouble(6),
                    SoNgayCong = reader.IsDBNull(7) ? 0 : reader.GetDouble(7),
                    SoGioOT = reader.IsDBNull(8) ? 0 : reader.GetDouble(8),
                    ThucLinh = reader.GetDecimal(9)
                });
            }
            return list;
        }
    }
}
