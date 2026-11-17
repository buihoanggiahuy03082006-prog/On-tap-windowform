using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using HRMPayrollSystem.DTO;

namespace HRMPayrollSystem.DAL
{
    public class PhongBanDAL
    {
        public List<PhongBanDTO> GetAll()
        {
            var list = new List<PhongBanDTO>();
            const string sql = "SELECT MaPB, TenPB, Mota FROM PhongBan ORDER BY MaPB";

            using var conn = DatabaseHelper.CreateConnection();
            using var cmd = new SqlCommand(sql, conn);
            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new PhongBanDTO
                {
                    MaPB = reader.GetInt32(0),
                    TenPB = reader.GetString(1),
                    Mota = reader.IsDBNull(2) ? string.Empty : reader.GetString(2)
                });
            }
            return list;
        }

        public bool Insert(PhongBanDTO pb)
        {
            const string sql = "INSERT INTO PhongBan (TenPB, Mota) VALUES (@TenPB, @Mota)";

            using var conn = DatabaseHelper.CreateConnection();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@TenPB", SqlDbType.NVarChar, 100).Value = pb.TenPB;
            cmd.Parameters.Add("@Mota", SqlDbType.NVarChar, 255).Value = (object?)pb.Mota ?? DBNull.Value;

            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Update(PhongBanDTO pb)
        {
            const string sql = "UPDATE PhongBan SET TenPB = @TenPB, Mota = @Mota WHERE MaPB = @MaPB";

            using var conn = DatabaseHelper.CreateConnection();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@MaPB", SqlDbType.Int).Value = pb.MaPB;
            cmd.Parameters.Add("@TenPB", SqlDbType.NVarChar, 100).Value = pb.TenPB;
            cmd.Parameters.Add("@Mota", SqlDbType.NVarChar, 255).Value = (object?)pb.Mota ?? DBNull.Value;

            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Delete(int maPB)
        {
            const string sql = "DELETE FROM PhongBan WHERE MaPB = @MaPB";

            using var conn = DatabaseHelper.CreateConnection();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@MaPB", SqlDbType.Int).Value = maPB;

            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }
    }
}
