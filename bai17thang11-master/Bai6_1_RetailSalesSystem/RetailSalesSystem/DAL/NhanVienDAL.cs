using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using RetailSalesSystem.DTO;

namespace RetailSalesSystem.DAL
{
    public class NhanVienDAL
    {
        public List<NhanVienDTO> GetAll()
        {
            var list = new List<NhanVienDTO>();
            const string sql = "SELECT MaNV, TenNV, SoDienThoai, ChucVu FROM NhanVien ORDER BY MaNV DESC";

            using var conn = DatabaseHelper.CreateConnection();
            using var cmd = new SqlCommand(sql, conn);
            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new NhanVienDTO
                {
                    MaNV = reader.GetInt32(0),
                    TenNV = reader.GetString(1),
                    SoDienThoai = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    ChucVu = reader.IsDBNull(3) ? string.Empty : reader.GetString(3)
                });
            }
            return list;
        }

        public NhanVienDTO? GetById(int maNV)
        {
            const string sql = "SELECT MaNV, TenNV, SoDienThoai, ChucVu FROM NhanVien WHERE MaNV = @MaNV";

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
                    TenNV = reader.GetString(1),
                    SoDienThoai = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    ChucVu = reader.IsDBNull(3) ? string.Empty : reader.GetString(3)
                };
            }
            return null;
        }

        public bool Insert(NhanVienDTO nv)
        {
            const string sql = "INSERT INTO NhanVien (TenNV, SoDienThoai, ChucVu) VALUES (@TenNV, @SoDienThoai, @ChucVu)";

            using var conn = DatabaseHelper.CreateConnection();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@TenNV", SqlDbType.NVarChar, 100).Value = nv.TenNV;
            cmd.Parameters.Add("@SoDienThoai", SqlDbType.VarChar, 15).Value = (object?)nv.SoDienThoai ?? DBNull.Value;
            cmd.Parameters.Add("@ChucVu", SqlDbType.NVarChar, 50).Value = (object?)nv.ChucVu ?? DBNull.Value;

            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Update(NhanVienDTO nv)
        {
            const string sql = "UPDATE NhanVien SET TenNV = @TenNV, SoDienThoai = @SoDienThoai, ChucVu = @ChucVu WHERE MaNV = @MaNV";

            using var conn = DatabaseHelper.CreateConnection();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@MaNV", SqlDbType.Int).Value = nv.MaNV;
            cmd.Parameters.Add("@TenNV", SqlDbType.NVarChar, 100).Value = nv.TenNV;
            cmd.Parameters.Add("@SoDienThoai", SqlDbType.VarChar, 15).Value = (object?)nv.SoDienThoai ?? DBNull.Value;
            cmd.Parameters.Add("@ChucVu", SqlDbType.NVarChar, 50).Value = (object?)nv.ChucVu ?? DBNull.Value;

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
    }
}
