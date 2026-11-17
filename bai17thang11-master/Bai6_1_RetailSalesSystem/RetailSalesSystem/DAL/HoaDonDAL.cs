using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using RetailSalesSystem.DTO;

namespace RetailSalesSystem.DAL
{
    public class HoaDonDAL
    {
        /// <summary>
        /// Thực thi giao dịch SqlTransaction đảm bảo tính toàn vẹn ACID
        /// </summary>
        public (bool Success, string Message, int MaHD) ThanhToanHoaDon(HoaDonDTO hoaDon, List<CartItemDTO> cartItems)
        {
            if (cartItems == null || cartItems.Count == 0)
            {
                return (false, "Giỏ hàng đang trống, không thể thanh toán!", 0);
            }

            using var conn = DatabaseHelper.CreateConnection();
            conn.Open();
            using var trans = conn.BeginTransaction();

            try
            {
                // BƯỚC 1: Chèn 1 bản ghi vào bảng HoaDon
                const string sqlInsertHD = @"
                    INSERT INTO HoaDon (NgayLap, MaNV, MaKH, TongTien) 
                    OUTPUT INSERTED.MaHD 
                    VALUES (@NgayLap, @MaNV, @MaKH, @TongTien)";

                int newMaHD;
                using (var cmdHD = new SqlCommand(sqlInsertHD, conn, trans))
                {
                    cmdHD.Parameters.Add("@NgayLap", SqlDbType.DateTime).Value = hoaDon.NgayLap;
                    cmdHD.Parameters.Add("@MaNV", SqlDbType.Int).Value = hoaDon.MaNV;
                    cmdHD.Parameters.Add("@MaKH", SqlDbType.Int).Value = (object?)hoaDon.MaKH ?? DBNull.Value;
                    cmdHD.Parameters.Add("@TongTien", SqlDbType.Decimal).Value = hoaDon.TongTien;

                    newMaHD = (int)cmdHD.ExecuteScalar()!;
                }

                // BƯỚC 2 & 3: Kiểm tra tồn kho, Chèn ChiTietHoaDon, Trừ SoLuongTon SanPham
                foreach (var item in cartItems)
                {
                    // 3a. Kiểm tra tồn kho trước khi bán (Khóa hàng WITH (UPDLOCK) để tránh race condition)
                    const string sqlCheckTon = "SELECT SoLuongTon, TenSP FROM SanPham WITH (UPDLOCK) WHERE MaSP = @MaSP";
                    int currentTon;
                    string tenSP;

                    using (var cmdCheck = new SqlCommand(sqlCheckTon, conn, trans))
                    {
                        cmdCheck.Parameters.Add("@MaSP", SqlDbType.VarChar, 20).Value = item.MaSP;
                        using var reader = cmdCheck.ExecuteReader();
                        if (!reader.Read())
                        {
                            throw new InvalidOperationException($"Sản phẩm mã [{item.MaSP}] không tồn tại trong hệ thống!");
                        }
                        currentTon = reader.GetInt32(0);
                        tenSP = reader.GetString(1);
                    }

                    // Nếu SoLuongTon < SoLuong bán -> Kích hoạt Rollback & báo lỗi
                    if (currentTon < item.SoLuong)
                    {
                        throw new InvalidOperationException(
                            $"[ACID TRANSACTION ROLLBACK] Sản phẩm '{tenSP}' (Mã: {item.MaSP}) hiện chỉ còn tồn {currentTon} sản phẩm, không đủ đáp ứng số lượng yêu cầu {item.SoLuong}!"
                        );
                    }

                    // 2. Chèn vào ChiTietHoaDon
                    const string sqlInsertCTHD = @"
                        INSERT INTO ChiTietHoaDon (MaHD, MaSP, SoLuong, DonGia) 
                        VALUES (@MaHD, @MaSP, @SoLuong, @DonGia)";

                    using (var cmdCTHD = new SqlCommand(sqlInsertCTHD, conn, trans))
                    {
                        cmdCTHD.Parameters.Add("@MaHD", SqlDbType.Int).Value = newMaHD;
                        cmdCTHD.Parameters.Add("@MaSP", SqlDbType.VarChar, 20).Value = item.MaSP;
                        cmdCTHD.Parameters.Add("@SoLuong", SqlDbType.Int).Value = item.SoLuong;
                        cmdCTHD.Parameters.Add("@DonGia", SqlDbType.Decimal).Value = item.DonGia;
                        cmdCTHD.ExecuteNonQuery();
                    }

                    // 3b. Trừ SoLuongTon tương ứng trong bảng SanPham
                    const string sqlUpdateTon = "UPDATE SanPham SET SoLuongTon = SoLuongTon - @SoLuong WHERE MaSP = @MaSP";
                    using (var cmdUpdateSP = new SqlCommand(sqlUpdateTon, conn, trans))
                    {
                        cmdUpdateSP.Parameters.Add("@SoLuong", SqlDbType.Int).Value = item.SoLuong;
                        cmdUpdateSP.Parameters.Add("@MaSP", SqlDbType.VarChar, 20).Value = item.MaSP;
                        cmdUpdateSP.ExecuteNonQuery();
                    }
                }

                // BƯỚC 4: Cộng điểm tích lũy cho Khách hàng (DiemTichLuy += TongTien / 100000)
                if (hoaDon.MaKH.HasValue && hoaDon.MaKH.Value > 0)
                {
                    int diemCong = (int)(hoaDon.TongTien / 100000m);
                    if (diemCong > 0)
                    {
                        const string sqlUpdateDiem = "UPDATE KhachHang SET DiemTichLuy = DiemTichLuy + @DiemCong WHERE MaKH = @MaKH";
                        using var cmdDiem = new SqlCommand(sqlUpdateDiem, conn, trans);
                        cmdDiem.Parameters.Add("@DiemCong", SqlDbType.Int).Value = diemCong;
                        cmdDiem.Parameters.Add("@MaKH", SqlDbType.Int).Value = hoaDon.MaKH.Value;
                        cmdDiem.ExecuteNonQuery();
                    }
                }

                // BƯỚC 5: Gọi trans.Commit() khi tất cả các bước thành công
                trans.Commit();
                return (true, $"Thanh toán thành công hóa đơn #{newMaHD}!", newMaHD);
            }
            catch (Exception ex)
            {
                // Khi có lỗi, rollback toàn bộ thao tác giao dịch
                try
                {
                    trans.Rollback();
                }
                catch (Exception rbEx)
                {
                    return (false, $"Giao dịch thất bại: {ex.Message} (Lỗi Rollback: {rbEx.Message})", 0);
                }

                return (false, $"[Giao dịch bị hủy - Rollback thành công]:\n{ex.Message}", 0);
            }
        }

        public List<HoaDonDTO> GetAllHoaDon()
        {
            var list = new List<HoaDonDTO>();
            const string sql = @"
                SELECT hd.MaHD, hd.NgayLap, hd.MaNV, hd.MaKH, hd.TongTien, 
                       nv.TenNV, ISNULL(kh.TenKH, N'Khách vãng lai') AS TenKH
                FROM HoaDon hd
                LEFT JOIN NhanVien nv ON hd.MaNV = nv.MaNV
                LEFT JOIN KhachHang kh ON hd.MaKH = kh.MaKH
                ORDER BY hd.MaHD DESC";

            using var conn = DatabaseHelper.CreateConnection();
            using var cmd = new SqlCommand(sql, conn);
            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new HoaDonDTO
                {
                    MaHD = reader.GetInt32(0),
                    NgayLap = reader.GetDateTime(1),
                    MaNV = reader.GetInt32(2),
                    MaKH = reader.IsDBNull(3) ? null : reader.GetInt32(3),
                    TongTien = reader.GetDecimal(4),
                    TenNV = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                    TenKH = reader.IsDBNull(6) ? string.Empty : reader.GetString(6)
                });
            }
            return list;
        }

        public List<ChiTietHoaDonDTO> GetChiTietByMaHD(int maHD)
        {
            var list = new List<ChiTietHoaDonDTO>();
            const string sql = @"
                SELECT ct.MaHD, ct.MaSP, sp.TenSP, ct.SoLuong, ct.DonGia
                FROM ChiTietHoaDon ct
                JOIN SanPham sp ON ct.MaSP = sp.MaSP
                WHERE ct.MaHD = @MaHD";

            using var conn = DatabaseHelper.CreateConnection();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@MaHD", SqlDbType.Int).Value = maHD;
            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new ChiTietHoaDonDTO
                {
                    MaHD = reader.GetInt32(0),
                    MaSP = reader.GetString(1),
                    TenSP = reader.GetString(2),
                    SoLuong = reader.GetInt32(3),
                    DonGia = reader.GetDecimal(4)
                });
            }
            return list;
        }
    }
}
