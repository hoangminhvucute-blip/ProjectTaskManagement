using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ProjectTaskManagement.DTO;

namespace ProjectTaskManagement.DAL
{
    public class CongViecDAL
    {
        public bool ThemTask(CongViecDTO task)
        {
            string sql = @"INSERT INTO dbo.CongViec (MaDuAn, TenTask, MoTa, HanNop, DoUuTien, TrangThai)
                         VALUES (@MaDuAn, @TenTask, @MoTa, @HanNop, @DoUuTien, @TrangThai)";

            using (SqlConnection conn = DbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@MaDuAn", SqlDbType.Int).Value = task.MaDuAn;
                cmd.Parameters.Add("@TenTask", SqlDbType.NVarChar, 200).Value = task.TenTask;
                cmd.Parameters.Add("@MoTa", SqlDbType.NVarChar).Value = (object)task.MoTa ?? DBNull.Value;
                cmd.Parameters.Add("@HanNop", SqlDbType.DateTime).Value = task.HanNop;
                cmd.Parameters.Add("@DoUuTien", SqlDbType.NVarChar, 20).Value = task.DoUuTien;
                cmd.Parameters.Add("@TrangThai", SqlDbType.NVarChar, 50).Value = task.TrangThai;

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public List<CongViecDTO> LayTaskTheoDuAnVaTrangThai(int? maDuAn, string trangThai)
        {
            var list = new List<CongViecDTO>();
            string sql = @"
                SELECT cv.MaTask, cv.MaDuAn, da.TenDuAn, cv.TenTask, cv.MoTa,
                       cv.HanNop, cv.DoUuTien, cv.TrangThai,
                       ISNULL(STUFF((
                           SELECT N', ' + tv.HoTen
                           FROM dbo.PhanCong pc
                           JOIN dbo.ThanhVien tv ON pc.MaThanhVien = tv.MaThanhVien
                           WHERE pc.MaTask = cv.MaTask
                           FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, N''), N'Chưa giao') AS NguoiPhuTrach
                FROM dbo.CongViec cv
                JOIN dbo.DuAn da ON cv.MaDuAn = da.MaDuAn
                WHERE (@MaDuAn IS NULL OR cv.MaDuAn = @MaDuAn)
                  AND (@TrangThai IS NULL OR @TrangThai = N'Tất cả' OR cv.TrangThai = @TrangThai)
                ORDER BY cv.HanNop ASC";

            using (SqlConnection conn = DbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@MaDuAn", SqlDbType.Int).Value = maDuAn.HasValue ? (object)maDuAn.Value : DBNull.Value;
                cmd.Parameters.Add("@TrangThai", SqlDbType.NVarChar, 50).Value = string.IsNullOrEmpty(trangThai) ? (object)DBNull.Value : trangThai;

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new CongViecDTO
                        {
                            MaTask = reader.GetInt32(0),
                            MaDuAn = reader.GetInt32(1),
                            TenDuAn = reader.GetString(2),
                            TenTask = reader.GetString(3),
                            MoTa = reader.IsDBNull(4) ? "" : reader.GetString(4),
                            HanNop = reader.GetDateTime(5),
                            DoUuTien = reader.GetString(6),
                            TrangThai = reader.GetString(7),
                            NguoiPhuTrach = reader.GetString(8)
                        });
                    }
                }
            }
            return list;
        }

        public bool CapNhatTrangThai(int maTask, string trangThaiMoi)
        {
            string sql = "UPDATE dbo.CongViec SET TrangThai = @TrangThai WHERE MaTask = @MaTask";

            using (SqlConnection conn = DbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@TrangThai", SqlDbType.NVarChar, 50).Value = trangThaiMoi;
                cmd.Parameters.Add("@MaTask", SqlDbType.Int).Value = maTask;

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public List<CongViecDTO> LayTaskTreHan()
        {
            var list = new List<CongViecDTO>();
            string sql = @"
                SELECT cv.MaTask, cv.MaDuAn, da.TenDuAn, cv.TenTask, cv.HanNop, cv.DoUuTien, cv.TrangThai
                FROM dbo.CongViec cv
                JOIN dbo.DuAn da ON cv.MaDuAn = da.MaDuAn
                WHERE cv.HanNop < GETDATE() AND cv.TrangThai <> N'Done'
                ORDER BY cv.HanNop ASC";

            using (SqlConnection conn = DbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new CongViecDTO
                        {
                            MaTask = reader.GetInt32(0),
                            MaDuAn = reader.GetInt32(1),
                            TenDuAn = reader.GetString(2),
                            TenTask = reader.GetString(3),
                            HanNop = reader.GetDateTime(4),
                            DoUuTien = reader.GetString(5),
                            TrangThai = reader.GetString(6)
                        });
                    }
                }
            }
            return list;
        }

        public DataTable ThongKeTyLeHoanThanhTheoThanhVien(int maDuAn)
        {
            string sql = @"
                SELECT
                    tv.MaThanhVien,
                    tv.HoTen,
                    tv.ChuyenMon,
                    COUNT(cv.MaTask) AS TongTask,
                    SUM(CASE WHEN cv.TrangThai = N'Done' THEN 1 ELSE 0 END) AS TaskHoanThanh,
                    CAST(
                        CASE WHEN COUNT(cv.MaTask) = 0 THEN 0
                             ELSE (SUM(CASE WHEN cv.TrangThai = N'Done' THEN 1.0 ELSE 0.0 END) / COUNT(cv.MaTask)) * 100
                        END AS DECIMAL(5,2)
                    ) AS TyLeHoanThanh
                FROM dbo.ThanhVien tv
                JOIN dbo.PhanCong pc ON tv.MaThanhVien = pc.MaThanhVien
                JOIN dbo.CongViec cv ON pc.MaTask = cv.MaTask
                WHERE cv.MaDuAn = @MaDuAn
                GROUP BY tv.MaThanhVien, tv.HoTen, tv.ChuyenMon
                ORDER BY TyLeHoanThanh DESC";

            var dt = new DataTable();
            using (SqlConnection conn = DbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@MaDuAn", SqlDbType.Int).Value = maDuAn;
                conn.Open();
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }
            return dt;
        }

        public int DemSoTaskTreHan()
        {
            string sql = "SELECT COUNT(*) FROM dbo.CongViec WHERE HanNop < GETDATE() AND TrangThai <> N'Done'";
            using (SqlConnection conn = DbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                object result = cmd.ExecuteScalar();
                return result != null && result != DBNull.Value ? Convert.ToInt32(result) : 0;
            }
        }
    }
}
