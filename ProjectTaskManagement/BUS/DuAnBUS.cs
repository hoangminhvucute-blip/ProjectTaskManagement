using System.Collections.Generic;
using ProjectTaskManagement.DAL;
using ProjectTaskManagement.DTO;

namespace ProjectTaskManagement.BUS
{
    public class DuAnBUS
    {
        private readonly DuAnDAL _dal = new DuAnDAL();

        public List<DuAnDTO> LayDanhSachDuAn() => _dal.LayDanhSachDuAn();

        public bool ThemDuAn(DuAnDTO duAn, out string message)
        {
            if (string.IsNullOrWhiteSpace(duAn.TenDuAn))
            {
                message = "Tên dự án không được để trống.";
                return false;
            }
            if (duAn.NgayKetThuc.HasValue && duAn.NgayKetThuc.Value < duAn.NgayBatDau)
            {
                message = "Ngày kết thúc không được nhỏ hơn ngày bắt đầu.";
                return false;
            }
            message = "Thêm dự án thành công.";
            return _dal.ThemDuAn(duAn);
        }

        public bool SuaDuAn(DuAnDTO duAn, out string message)
        {
            if (duAn.MaDuAn <= 0)
            {
                message = "Vui lòng chọn dự án cần sửa.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(duAn.TenDuAn))
            {
                message = "Tên dự án không được để trống.";
                return false;
            }
            message = "Cập nhật dự án thành công.";
            return _dal.SuaDuAn(duAn);
        }

        public bool XoaDuAn(int maDuAn, out string message)
        {
            if (maDuAn <= 0)
            {
                message = "Vui lòng chọn dự án cần xóa.";
                return false;
            }
            message = "Xóa dự án thành công.";
            return _dal.XoaDuAn(maDuAn);
        }
    }
}
