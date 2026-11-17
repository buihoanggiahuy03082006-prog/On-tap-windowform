using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using RetailSalesSystem.DTO;

namespace RetailSalesSystem.DAL
{
    public class KhachHangDAL
    {
        public List<KhachHangDTO> GetAll()
        {
            var list = new List<KhachHangDTO>();
            const string sql = "SELECT MaKH, TenKH, DienThoai, DiemTichLuy FROM KhachHang ORDER BY MaKH DESC";

            using var conn = DatabaseHelper.CreateConnection();
            using var cmd = new SqlCommand(sql, conn);
            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new KhachHangDTO
                {
                    MaKH = reader.GetInt32(0),
                    TenKH = reader.GetString(1),
                    DienThoai = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    DiemTichLuy = reader.GetInt32(3)
                });
            }
            return list;
        }

        public KhachHangDTO? GetById(int maKH)
        {
            const string sql = "SELECT MaKH, TenKH, DienThoai, DiemTichLuy FROM KhachHang WHERE MaKH = @MaKH";

            using var conn = DatabaseHelper.CreateConnection();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@MaKH", SqlDbType.Int).Value = maKH;

            conn.Open();
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new KhachHangDTO
                {
                    MaKH = reader.GetInt32(0),
                    TenKH = reader.GetString(1),
                    DienThoai = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    DiemTichLuy = reader.GetInt32(3)
                };
            }
            return null;
        }

        public bool Insert(KhachHangDTO kh)
        {
            const string sql = "INSERT INTO KhachHang (TenKH, DienThoai, DiemTichLuy) VALUES (@TenKH, @DienThoai, @DiemTichLuy)";

            using var conn = DatabaseHelper.CreateConnection();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@TenKH", SqlDbType.NVarChar, 100).Value = kh.TenKH;
            cmd.Parameters.Add("@DienThoai", SqlDbType.VarChar, 15).Value = (object?)kh.DienThoai ?? DBNull.Value;
            cmd.Parameters.Add("@DiemTichLuy", SqlDbType.Int).Value = kh.DiemTichLuy;

            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Update(KhachHangDTO kh)
        {
            const string sql = "UPDATE KhachHang SET TenKH = @TenKH, DienThoai = @DienThoai, DiemTichLuy = @DiemTichLuy WHERE MaKH = @MaKH";

            using var conn = DatabaseHelper.CreateConnection();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@MaKH", SqlDbType.Int).Value = kh.MaKH;
            cmd.Parameters.Add("@TenKH", SqlDbType.NVarChar, 100).Value = kh.TenKH;
            cmd.Parameters.Add("@DienThoai", SqlDbType.VarChar, 15).Value = (object?)kh.DienThoai ?? DBNull.Value;
            cmd.Parameters.Add("@DiemTichLuy", SqlDbType.Int).Value = kh.DiemTichLuy;

            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Delete(int maKH)
        {
            const string sql = "DELETE FROM KhachHang WHERE MaKH = @MaKH";

            using var conn = DatabaseHelper.CreateConnection();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@MaKH", SqlDbType.Int).Value = maKH;

            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public List<KhachHangDTO> Search(string keyword)
        {
            var list = new List<KhachHangDTO>();
            const string sql = @"SELECT MaKH, TenKH, DienThoai, DiemTichLuy 
                                 FROM KhachHang 
                                 WHERE TenKH LIKE @Keyword OR DienThoai LIKE @Keyword 
                                 ORDER BY MaKH DESC";

            using var conn = DatabaseHelper.CreateConnection();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@Keyword", SqlDbType.NVarChar, 100).Value = "%" + keyword + "%";

            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new KhachHangDTO
                {
                    MaKH = reader.GetInt32(0),
                    TenKH = reader.GetString(1),
                    DienThoai = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    DiemTichLuy = reader.GetInt32(3)
                });
            }
            return list;
        }
    }
}
