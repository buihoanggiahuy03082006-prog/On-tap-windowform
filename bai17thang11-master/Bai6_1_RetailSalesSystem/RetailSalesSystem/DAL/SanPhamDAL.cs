using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using RetailSalesSystem.DTO;

namespace RetailSalesSystem.DAL
{
    public class SanPhamDAL
    {
        public List<SanPhamDTO> GetAll()
        {
            var list = new List<SanPhamDTO>();
            const string sql = "SELECT MaSP, TenSP, GiaBan, SoLuongTon FROM SanPham ORDER BY MaSP";

            using var conn = DatabaseHelper.CreateConnection();
            using var cmd = new SqlCommand(sql, conn);
            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new SanPhamDTO
                {
                    MaSP = reader.GetString(0),
                    TenSP = reader.GetString(1),
                    GiaBan = reader.GetDecimal(2),
                    SoLuongTon = reader.GetInt32(3)
                });
            }
            return list;
        }

        public SanPhamDTO? GetById(string maSP)
        {
            const string sql = "SELECT MaSP, TenSP, GiaBan, SoLuongTon FROM SanPham WHERE MaSP = @MaSP";

            using var conn = DatabaseHelper.CreateConnection();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@MaSP", SqlDbType.VarChar, 20).Value = maSP;

            conn.Open();
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new SanPhamDTO
                {
                    MaSP = reader.GetString(0),
                    TenSP = reader.GetString(1),
                    GiaBan = reader.GetDecimal(2),
                    SoLuongTon = reader.GetInt32(3)
                };
            }
            return null;
        }

        public bool Insert(SanPhamDTO sp)
        {
            const string sql = "INSERT INTO SanPham (MaSP, TenSP, GiaBan, SoLuongTon) VALUES (@MaSP, @TenSP, @GiaBan, @SoLuongTon)";

            using var conn = DatabaseHelper.CreateConnection();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@MaSP", SqlDbType.VarChar, 20).Value = sp.MaSP;
            cmd.Parameters.Add("@TenSP", SqlDbType.NVarChar, 150).Value = sp.TenSP;
            cmd.Parameters.Add("@GiaBan", SqlDbType.Decimal).Value = sp.GiaBan;
            cmd.Parameters.Add("@SoLuongTon", SqlDbType.Int).Value = sp.SoLuongTon;

            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Update(SanPhamDTO sp)
        {
            const string sql = "UPDATE SanPham SET TenSP = @TenSP, GiaBan = @GiaBan, SoLuongTon = @SoLuongTon WHERE MaSP = @MaSP";

            using var conn = DatabaseHelper.CreateConnection();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@MaSP", SqlDbType.VarChar, 20).Value = sp.MaSP;
            cmd.Parameters.Add("@TenSP", SqlDbType.NVarChar, 150).Value = sp.TenSP;
            cmd.Parameters.Add("@GiaBan", SqlDbType.Decimal).Value = sp.GiaBan;
            cmd.Parameters.Add("@SoLuongTon", SqlDbType.Int).Value = sp.SoLuongTon;

            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Delete(string maSP)
        {
            const string sql = "DELETE FROM SanPham WHERE MaSP = @MaSP";

            using var conn = DatabaseHelper.CreateConnection();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@MaSP", SqlDbType.VarChar, 20).Value = maSP;

            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        /// <summary>
        /// Tìm kiếm sản phẩm theo tên hoặc mã bằng SqlParameter có ký tự đại diện %
        /// Chống 100% SQL Injection
        /// </summary>
        public List<SanPhamDTO> Search(string keyword)
        {
            var list = new List<SanPhamDTO>();
            const string sql = @"SELECT MaSP, TenSP, GiaBan, SoLuongTon 
                                 FROM SanPham 
                                 WHERE MaSP LIKE @Keyword OR TenSP LIKE @Keyword 
                                 ORDER BY MaSP";

            using var conn = DatabaseHelper.CreateConnection();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@Keyword", SqlDbType.NVarChar, 150).Value = "%" + keyword + "%";

            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new SanPhamDTO
                {
                    MaSP = reader.GetString(0),
                    TenSP = reader.GetString(1),
                    GiaBan = reader.GetDecimal(2),
                    SoLuongTon = reader.GetInt32(3)
                });
            }
            return list;
        }
    }
}
