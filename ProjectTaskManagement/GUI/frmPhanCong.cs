using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ProjectTaskManagement.BUS;
using ProjectTaskManagement.DTO;

namespace ProjectTaskManagement.GUI
{
    public partial class frmPhanCong : Form
    {
        private readonly int _maTask;
        private readonly PhanCongBUS _phanCongBUS = new PhanCongBUS();

        public frmPhanCong(int maTask, string tenTask)
        {
            InitializeComponent();
            _maTask = maTask;
            this.Text = $"Phân công: {tenTask}";
        }

        private void frmPhanCong_Load(object sender, EventArgs e)
        {
            List<ThanhVienDTO> dsThanhVien = _phanCongBUS.LayTatCaThanhVien();
            List<int> daPhanCong = _phanCongBUS.LayDanhSachMaThanhVienTheoTask(_maTask);

            clbThanhVien.Items.Clear();
            foreach (var tv in dsThanhVien)
            {
                int index = clbThanhVien.Items.Add(tv);
                if (daPhanCong.Contains(tv.MaThanhVien))
                {
                    clbThanhVien.SetItemChecked(index, true);
                }
            }
            clbThanhVien.DisplayMember = "HoTen";
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            var selectedIds = new List<int>();
            foreach (var item in clbThanhVien.CheckedItems)
            {
                if (item is ThanhVienDTO tv)
                {
                    selectedIds.Add(tv.MaThanhVien);
                }
            }

            if (_phanCongBUS.LuuPhanCong(_maTask, selectedIds, out string msg))
            {
                MessageBox.Show(msg, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show(msg, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
