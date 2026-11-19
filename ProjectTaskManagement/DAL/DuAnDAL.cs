using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ProjectTaskManagement.DTO;

namespace ProjectTaskManagement.DAL
{
    public class DuAnDAL
    {
        public List<DuAnDTO> LayDanhSachDuAn()
        {
            var list = new List<DuAnDTO>();
            string sql = "SELECT MaDuAn, TenDuAn, NgayBatDau, NgayKetThuc, TrangThai FROM dbo.DuAn ORDER BY MaDuAn DESC";

            using (SqlConnection conn = DbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new DuAnDTO
                        {
                            MaDuAn = reader.GetInt32(0),
                            TenDuAn = reader.GetString(1),
                            NgayBatDau = reader.GetDateTime(2),
                            NgayKetThuc = reader.IsDBNull(3) ? (DateTime?)null : reader.GetDateTime(3),
                            TrangThai = reader.GetString(4)
                        });
                    }
                }
            }
            return list;
        }

        public bool ThemDuAn(DuAnDTO duAn)
        {
            string sql = @"INSERT INTO dbo.DuAn (TenDuAn, NgayBatDau, NgayKetThuc, TrangThai)
                         VALUES (@TenDuAn, @NgayBatDau, @NgayKetThuc, @TrangThai)";

            using (SqlConnection conn = DbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@TenDuAn", SqlDbType.NVarChar, 150).Value = duAn.TenDuAn;
                cmd.Parameters.Add("@NgayBatDau", SqlDbType.DateTime).Value = duAn.NgayBatDau;
                cmd.Parameters.Add("@NgayKetThuc", SqlDbType.DateTime).Value =
                    duAn.NgayKetThuc.HasValue ? (object)duAn.NgayKetThuc.Value : DBNull.Value;
                cmd.Parameters.Add("@TrangThai", SqlDbType.NVarChar, 50).Value = duAn.TrangThai;

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool SuaDuAn(DuAnDTO duAn)
        {
            string sql = @"UPDATE dbo.DuAn
                         SET TenDuAn = @TenDuAn, NgayBatDau = @NgayBatDau,
                             NgayKetThuc = @NgayKetThuc, TrangThai = @TrangThai
                         WHERE MaDuAn = @MaDuAn";

            using (SqlConnection conn = DbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@MaDuAn", SqlDbType.Int).Value = duAn.MaDuAn;
                cmd.Parameters.Add("@TenDuAn", SqlDbType.NVarChar, 150).Value = duAn.TenDuAn;
                cmd.Parameters.Add("@NgayBatDau", SqlDbType.DateTime).Value = duAn.NgayBatDau;
                cmd.Parameters.Add("@NgayKetThuc", SqlDbType.DateTime).Value =
                    duAn.NgayKetThuc.HasValue ? (object)duAn.NgayKetThuc.Value : DBNull.Value;
                cmd.Parameters.Add("@TrangThai", SqlDbType.NVarChar, 50).Value = duAn.TrangThai;

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool XoaDuAn(int maDuAn)
        {
            string sql = "DELETE FROM dbo.DuAn WHERE MaDuAn = @MaDuAn";

            using (SqlConnection conn = DbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@MaDuAn", SqlDbType.Int).Value = maDuAn;
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}
