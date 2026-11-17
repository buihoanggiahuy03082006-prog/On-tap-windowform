using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using RetailSalesSystem.DTO;

namespace RetailSalesSystem.DAL
{
    public class ThongKeDAL
    {
        /// <summary>
        /// Dùng ExecuteScalar tính tổng doanh thu trong ngày hiện tại
        /// </summary>
        public decimal GetDoanhThuNgay(DateTime ngay)
        {
            const string sql = @"
                SELECT ISNULL(SUM(TongTien), 0) 
                FROM HoaDon 
                WHERE CAST(NgayLap AS DATE) = @Ngay";

            using var conn = DatabaseHelper.CreateConnection();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@Ngay", SqlDbType.Date).Value = ngay.Date;

            conn.Open();
            object result = cmd.ExecuteScalar()!;
            return Convert.ToDecimal(result);
        }

        /// <summary>
        /// Dùng ExecuteScalar tính tổng doanh thu trong tháng/năm
        /// </summary>
        public decimal GetDoanhThuThang(int thang, int nam)
        {
            const string sql = @"
                SELECT ISNULL(SUM(TongTien), 0) 
                FROM HoaDon 
                WHERE MONTH(NgayLap) = @Thang AND YEAR(NgayLap) = @Nam";

            using var conn = DatabaseHelper.CreateConnection();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@Thang", SqlDbType.Int).Value = thang;
            cmd.Parameters.Add("@Nam", SqlDbType.Int).Value = nam;

            conn.Open();
            object result = cmd.ExecuteScalar()!;
            return Convert.ToDecimal(result);
        }

        /// <summary>
        /// Dùng ExecuteScalar đếm tổng số hóa đơn
        /// </summary>
        public int GetTongSoHoaDon()
        {
            const string sql = "SELECT COUNT(*) FROM HoaDon";

            using var conn = DatabaseHelper.CreateConnection();
            using var cmd = new SqlCommand(sql, conn);
            conn.Open();
            object result = cmd.ExecuteScalar()!;
            return Convert.ToInt32(result);
        }

        /// <summary>
        /// Dùng SqlDataReader hiển thị danh sách Top 5 sản phẩm bán chạy nhất
        /// </summary>
        public List<TopSanPhamDTO> GetTop5SanPhamBanChay()
        {
            var list = new List<TopSanPhamDTO>();
            const string sql = @"
                SELECT TOP 5 
                    sp.MaSP, 
                    sp.TenSP, 
                    ISNULL(SUM(ct.SoLuong), 0) AS TongSoLuongBan,
                    ISNULL(SUM(ct.SoLuong * ct.DonGia), 0) AS TongDoanhThu
                FROM ChiTietHoaDon ct
                JOIN SanPham sp ON ct.MaSP = sp.MaSP
                GROUP BY sp.MaSP, sp.TenSP
                ORDER BY TongSoLuongBan DESC";

            using var conn = DatabaseHelper.CreateConnection();
            using var cmd = new SqlCommand(sql, conn);
            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new TopSanPhamDTO
                {
                    MaSP = reader.GetString(0),
                    TenSP = reader.GetString(1),
                    TongSoLuongBan = reader.GetInt32(2),
                    TongDoanhThu = reader.GetDecimal(3)
                });
            }
            return list;
        }
    }
}
