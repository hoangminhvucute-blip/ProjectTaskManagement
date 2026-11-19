using System.Collections.Generic;
using ProjectTaskManagement.DAL;
using ProjectTaskManagement.DTO;

namespace ProjectTaskManagement.BUS
{
    public class PhanCongBUS
    {
        private readonly PhanCongDAL _dal = new PhanCongDAL();

        public List<ThanhVienDTO> LayTatCaThanhVien() => _dal.LayTatCaThanhVien();

        public List<int> LayDanhSachMaThanhVienTheoTask(int maTask)
            => _dal.LayDanhSachMaThanhVienTheoTask(maTask);

        public bool LuuPhanCong(int maTask, List<int> danhSachMaThanhVien, out string message)
        {
            if (maTask <= 0)
            {
                message = "Chưa chọn Task để phân công.";
                return false;
            }
            if (danhSachMaThanhVien == null || danhSachMaThanhVien.Count == 0)
            {
                message = "Vui lòng chọn ít nhất một thành viên.";
                return false;
            }

            message = "Phân công thành viên thành công.";
            return _dal.LuuPhanCong(maTask, danhSachMaThanhVien);
        }
    }
}
