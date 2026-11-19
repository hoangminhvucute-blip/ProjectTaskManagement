using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ProjectTaskManagement.DTO;

namespace ProjectTaskManagement.DAL
{
    public class PhanCongDAL
    {
        public List<ThanhVienDTO> LayTatCaThanhVien()
        {
            var list = new List<ThanhVienDTO>();
            string sql = "SELECT MaThanhVien, HoTen, Email, ChuyenMon FROM dbo.ThanhVien ORDER BY HoTen";

            using (SqlConnection conn = DbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new ThanhVienDTO
                        {
                            MaThanhVien = reader.GetInt32(0),
                            HoTen = reader.GetString(1),
                            Email = reader.GetString(2),
                            ChuyenMon = reader.IsDBNull(3) ? "" : reader.GetString(3)
                        });
                    }
                }
            }
            return list;
        }

        public List<int> LayDanhSachMaThanhVienTheoTask(int maTask)
        {
            var ids = new List<int>();
            string sql = "SELECT MaThanhVien FROM dbo.PhanCong WHERE MaTask = @MaTask";

            using (SqlConnection conn = DbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@MaTask", SqlDbType.Int).Value = maTask;
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ids.Add(reader.GetInt32(0));
                    }
                }
            }
            return ids;
        }

        public bool LuuPhanCong(int maTask, List<int> danhSachMaThanhVien)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();

                try
                {
                    string deleteSql = "DELETE FROM dbo.PhanCong WHERE MaTask = @MaTask";
                    using (SqlCommand delCmd = new SqlCommand(deleteSql, conn, tran))
                    {
                        delCmd.Parameters.Add("@MaTask", SqlDbType.Int).Value = maTask;
                        delCmd.ExecuteNonQuery();
                    }

                    string insertSql = @"INSERT INTO dbo.PhanCong (MaTask, MaThanhVien, NgayGiao)
                                         VALUES (@MaTask, @MaThanhVien, GETDATE())";
                    foreach (int maTV in danhSachMaThanhVien)
                    {
                        using (SqlCommand insCmd = new SqlCommand(insertSql, conn, tran))
                        {
                            insCmd.Parameters.Add("@MaTask", SqlDbType.Int).Value = maTask;
                            insCmd.Parameters.Add("@MaThanhVien", SqlDbType.Int).Value = maTV;
                            insCmd.ExecuteNonQuery();
                        }
                    }

                    tran.Commit();
                    return true;
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
        }
    }
}
