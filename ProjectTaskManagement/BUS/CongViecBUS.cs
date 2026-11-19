using System.Collections.Generic;
using System.Data;
using ProjectTaskManagement.DAL;
using ProjectTaskManagement.DTO;

namespace ProjectTaskManagement.BUS
{
    public class CongViecBUS
    {
        private readonly CongViecDAL _dal = new CongViecDAL();

        private static readonly HashSet<string> _trangThaiHopLe = new HashSet<string>
        {
            "To Do", "In Progress", "Review", "Done"
        };

        public bool ThemTask(CongViecDTO task, out string message)
        {
            if (task.MaDuAn <= 0)
            {
                message = "Vui lòng chọn dự án cho công việc.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(task.TenTask))
            {
                message = "Tên công việc không được để trống.";
                return false;
            }
            if (!_trangThaiHopLe.Contains(task.TrangThai))
            {
                message = "Trạng thái công việc không hợp lệ.";
                return false;
            }

            message = "Tạo công việc thành công.";
            return _dal.ThemTask(task);
        }

        public List<CongViecDTO> LayTaskTheoDuAnVaTrangThai(int? maDuAn, string trangThai)
            => _dal.LayTaskTheoDuAnVaTrangThai(maDuAn, trangThai);

        public bool CapNhatTrangThai(int maTask, string trangThaiMoi, out string message)
        {
            if (maTask <= 0)
            {
                message = "Vui lòng chọn công việc cần cập nhật.";
                return false;
            }
            if (!_trangThaiHopLe.Contains(trangThaiMoi))
            {
                message = "Trạng thái không hợp lệ.";
                return false;
            }

            message = $"Đã chuyển trạng thái Task #{maTask} sang '{trangThaiMoi}'.";
            return _dal.CapNhatTrangThai(maTask, trangThaiMoi);
        }

        public List<CongViecDTO> LayTaskTreHan() => _dal.LayTaskTreHan();

        public int DemSoTaskTreHan() => _dal.DemSoTaskTreHan();

        public DataTable ThongKeTyLeHoanThanhTheoThanhVien(int maDuAn)
            => _dal.ThongKeTyLeHoanThanhTheoThanhVien(maDuAn);
    }
}
