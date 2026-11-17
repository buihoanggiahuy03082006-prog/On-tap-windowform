using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using HRMPayrollSystem.DTO;

namespace HRMPayrollSystem.DAL
{
    public class NhanVienDAL
    {
        public List<NhanVienDTO> GetAll()
        {
            var list = new List<NhanVienDTO>();
            const string sql = @"
                SELECT nv.MaNV, nv.HoTen, nv.NgaySinh, nv.CCCD, nv.MaPB, pb.TenPB, nv.LuongCoBan, nv.HeSoLuong
                FROM NhanVien nv
                JOIN PhongBan pb ON nv.MaPB = pb.MaPB
                ORDER BY nv.MaNV DESC";

            using var conn = DatabaseHelper.CreateConnection();
            using var cmd = new SqlCommand(sql, conn);
            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new NhanVienDTO
                {
                    MaNV = reader.GetInt32(0),
                    HoTen = reader.GetString(1),
                    NgaySinh = reader.IsDBNull(2) ? null : reader.GetDateTime(2),
                    CCCD = reader.GetString(3),
                    MaPB = reader.GetInt32(4),
                    TenPB = reader.GetString(5),
                    LuongCoBan = reader.GetDecimal(6),
                    HeSoLuong = reader.GetDouble(7)
                });
            }
            return list;
        }

        public NhanVienDTO? GetById(int maNV)
        {
            const string sql = @"
                SELECT nv.MaNV, nv.HoTen, nv.NgaySinh, nv.CCCD, nv.MaPB, pb.TenPB, nv.LuongCoBan, nv.HeSoLuong
                FROM NhanVien nv
                JOIN PhongBan pb ON nv.MaPB = pb.MaPB
                WHERE nv.MaNV = @MaNV";

            using var conn = DatabaseHelper.CreateConnection();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@MaNV", SqlDbType.Int).Value = maNV;

            conn.Open();
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new NhanVienDTO
                {
                    MaNV = reader.GetInt32(0),
                    HoTen = reader.GetString(1),
                    NgaySinh = reader.IsDBNull(2) ? null : reader.GetDateTime(2),
                    CCCD = reader.GetString(3),
                    MaPB = reader.GetInt32(4),
                    TenPB = reader.GetString(5),
                    LuongCoBan = reader.GetDecimal(6),
                    HeSoLuong = reader.GetDouble(7)
                };
            }
            return null;
        }

        public bool Insert(NhanVienDTO nv)
        {
            const string sql = @"
                INSERT INTO NhanVien (HoTen, NgaySinh, CCCD, MaPB, LuongCoBan, HeSoLuong) 
                VALUES (@HoTen, @NgaySinh, @CCCD, @MaPB, @LuongCoBan, @HeSoLuong)";

            using var conn = DatabaseHelper.CreateConnection();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@HoTen", SqlDbType.NVarChar, 100).Value = nv.HoTen;
            cmd.Parameters.Add("@NgaySinh", SqlDbType.DateTime).Value = (object?)nv.NgaySinh ?? DBNull.Value;
            cmd.Parameters.Add("@CCCD", SqlDbType.VarChar, 20).Value = nv.CCCD;
            cmd.Parameters.Add("@MaPB", SqlDbType.Int).Value = nv.MaPB;
            cmd.Parameters.Add("@LuongCoBan", SqlDbType.Decimal).Value = nv.LuongCoBan;
            cmd.Parameters.Add("@HeSoLuong", SqlDbType.Float).Value = nv.HeSoLuong;

            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Update(NhanVienDTO nv)
        {
            const string sql = @"
                UPDATE NhanVien 
                SET HoTen = @HoTen, NgaySinh = @NgaySinh, CCCD = @CCCD, 
                    MaPB = @MaPB, LuongCoBan = @LuongCoBan, HeSoLuong = @HeSoLuong 
                WHERE MaNV = @MaNV";

            using var conn = DatabaseHelper.CreateConnection();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@MaNV", SqlDbType.Int).Value = nv.MaNV;
            cmd.Parameters.Add("@HoTen", SqlDbType.NVarChar, 100).Value = nv.HoTen;
            cmd.Parameters.Add("@NgaySinh", SqlDbType.DateTime).Value = (object?)nv.NgaySinh ?? DBNull.Value;
            cmd.Parameters.Add("@CCCD", SqlDbType.VarChar, 20).Value = nv.CCCD;
            cmd.Parameters.Add("@MaPB", SqlDbType.Int).Value = nv.MaPB;
            cmd.Parameters.Add("@LuongCoBan", SqlDbType.Decimal).Value = nv.LuongCoBan;
            cmd.Parameters.Add("@HeSoLuong", SqlDbType.Float).Value = nv.HeSoLuong;

            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool ChuyenPhongBan(int maNV, int maPBMoi)
        {
            const string sql = "UPDATE NhanVien SET MaPB = @MaPBMoi WHERE MaNV = @MaNV";

            using var conn = DatabaseHelper.CreateConnection();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@MaNV", SqlDbType.Int).Value = maNV;
            cmd.Parameters.Add("@MaPBMoi", SqlDbType.Int).Value = maPBMoi;

            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Delete(int maNV)
        {
            const string sql = "DELETE FROM NhanVien WHERE MaNV = @MaNV";

            using var conn = DatabaseHelper.CreateConnection();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@MaNV", SqlDbType.Int).Value = maNV;

            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        /// <summary>
        /// Tìm kiếm nhân viên theo tên, phòng ban hoặc CCCD (Parameterized Query)
        /// </summary>
        public List<NhanVienDTO> Search(string keyword, int? maPB)
        {
            var list = new List<NhanVienDTO>();
            string sql = @"
                SELECT nv.MaNV, nv.HoTen, nv.NgaySinh, nv.CCCD, nv.MaPB, pb.TenPB, nv.LuongCoBan, nv.HeSoLuong
                FROM NhanVien nv
                JOIN PhongBan pb ON nv.MaPB = pb.MaPB
                WHERE (nv.HoTen LIKE @Keyword OR nv.CCCD LIKE @Keyword)";

            if (maPB.HasValue && maPB.Value > 0)
            {
                sql += " AND nv.MaPB = @MaPB";
            }
            sql += " ORDER BY nv.MaNV DESC";

            using var conn = DatabaseHelper.CreateConnection();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@Keyword", SqlDbType.NVarChar, 100).Value = "%" + keyword + "%";
            if (maPB.HasValue && maPB.Value > 0)
            {
                cmd.Parameters.Add("@MaPB", SqlDbType.Int).Value = maPB.Value;
            }

            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new NhanVienDTO
                {
                    MaNV = reader.GetInt32(0),
                    HoTen = reader.GetString(1),
                    NgaySinh = reader.IsDBNull(2) ? null : reader.GetDateTime(2),
                    CCCD = reader.GetString(3),
                    MaPB = reader.GetInt32(4),
                    TenPB = reader.GetString(5),
                    LuongCoBan = reader.GetDecimal(6),
                    HeSoLuong = reader.GetDouble(7)
                });
            }
            return list;
        }
    }
}
