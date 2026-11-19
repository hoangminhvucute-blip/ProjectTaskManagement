using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ProjectTaskManagement.BUS;
using ProjectTaskManagement.DTO;

namespace ProjectTaskManagement.GUI
{
    public partial class frmMain : Form
    {
        private readonly DuAnBUS _duAnBUS = new DuAnBUS();
        private readonly CongViecBUS _congViecBUS = new CongViecBUS();

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            try
            {
                LoadTrangThaiFilter();
                LoadDuAn();
                LoadCanhBaoTreHan();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối CSDL. Hãy kiểm tra SQL Server và App.config.\nChi tiết: " + ex.Message,
                    "Lỗi khởi tạo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadTrangThaiFilter()
        {
            cboTrangThaiFilter.Items.Clear();
            cboTrangThaiFilter.Items.AddRange(new object[] { "Tất cả", "To Do", "In Progress", "Review", "Done" });
            cboTrangThaiFilter.SelectedIndex = 0;

            cboTrangThaiMoi.Items.Clear();
            cboTrangThaiMoi.Items.AddRange(new object[] { "To Do", "In Progress", "Review", "Done" });
            cboTrangThaiMoi.SelectedIndex = 0;

            cboDoUuTien.Items.Clear();
            cboDoUuTien.Items.AddRange(new object[] { "Cao", "Trung bình", "Thấp" });
            cboDoUuTien.SelectedIndex = 1;
        }

        private void LoadDuAn()
        {
            List<DuAnDTO> dsDuAn = _duAnBUS.LayDanhSachDuAn();
            dgvDuAn.DataSource = dsDuAn;

            cboDuAnFilter.DataSource = null;
            var filterList = new List<DuAnDTO> { new DuAnDTO { MaDuAn = 0, TenDuAn = "-- Tất cả dự án --" } };
            filterList.AddRange(dsDuAn);
            cboDuAnFilter.DataSource = filterList;
            cboDuAnFilter.DisplayMember = "TenDuAn";
            cboDuAnFilter.ValueMember = "MaDuAn";

            if (dsDuAn.Count > 0)
            {
                LoadTasks();
                LoadThongKeThanhVien(dsDuAn[0].MaDuAn);
            }
        }

        private void btnThemDuAn_Click(object sender, EventArgs e)
        {
            var da = new DuAnDTO
            {
                TenDuAn = txtTenDuAn.Text.Trim(),
                NgayBatDau = dtpNgayBatDau.Value,
                NgayKetThuc = dtpNgayKetThuc.Value,
                TrangThai = cboTrangThaiDuAn.SelectedItem?.ToString() ?? "Đang chạy"
            };
            if (_duAnBUS.ThemDuAn(da, out string msg))
            {
                MessageBox.Show(msg, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTenDuAn.Clear();
                LoadDuAn();
            }
            else
            {
                MessageBox.Show(msg, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSuaDuAn_Click(object sender, EventArgs e)
        {
            if (dgvDuAn.CurrentRow?.DataBoundItem is DuAnDTO selected)
            {
                selected.TenDuAn = txtTenDuAn.Text.Trim();
                selected.NgayBatDau = dtpNgayBatDau.Value;
                selected.NgayKetThuc = dtpNgayKetThuc.Value;
                selected.TrangThai = cboTrangThaiDuAn.SelectedItem?.ToString() ?? "Đang chạy";

                if (_duAnBUS.SuaDuAn(selected, out string msg))
                {
                    MessageBox.Show(msg, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDuAn();
                }
                else
                {
                    MessageBox.Show(msg, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void btnXoaDuAn_Click(object sender, EventArgs e)
        {
            if (dgvDuAn.CurrentRow?.DataBoundItem is DuAnDTO selected)
            {
                if (MessageBox.Show($"Xóa dự án '{selected.TenDuAn}'?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (_duAnBUS.XoaDuAn(selected.MaDuAn, out string msg))
                    {
                        MessageBox.Show(msg, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDuAn();
                        LoadTasks();
                        LoadCanhBaoTreHan();
                    }
                }
            }
        }

        private void LoadTasks()
        {
            int? maDuAn = null;
            if (cboDuAnFilter.SelectedValue is int id && id > 0)
                maDuAn = id;

            string trangThai = cboTrangThaiFilter.SelectedItem?.ToString();
            List<CongViecDTO> tasks = _congViecBUS.LayTaskTheoDuAnVaTrangThai(maDuAn, trangThai);

            dgvTasks.DataSource = tasks;
            RenderKanbanBoard(tasks);
        }

        private void RenderKanbanBoard(List<CongViecDTO> tasks)
        {
            flpTodo.Controls.Clear();
            flpInProgress.Controls.Clear();
            flpReview.Controls.Clear();
            flpDone.Controls.Clear();

            foreach (var t in tasks)
            {
                Panel card = CreateTaskCard(t);
                switch (t.TrangThai)
                {
                    case "To Do":
                        flpTodo.Controls.Add(card);
                        break;
                    case "In Progress":
                        flpInProgress.Controls.Add(card);
                        break;
                    case "Review":
                        flpReview.Controls.Add(card);
                        break;
                    case "Done":
                        flpDone.Controls.Add(card);
                        break;
                }
            }
        }

        private Panel CreateTaskCard(CongViecDTO task)
        {
            var panel = new Panel
            {
                Width = 195,
                Height = 96,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(4),
                Tag = task
            };

            var lblTitle = new Label
            {
                Text = $"#{task.MaTask} - {task.TenTask}",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                Location = new Point(6, 5),
                Size = new Size(180, 20)
            };

            var lblMeta = new Label
            {
                Text = $"{task.DoUuTien} | Hạn: {task.HanNop:dd/MM}",
                Font = new Font("Segoe UI", 8F),
                ForeColor = task.HanNop < DateTime.Now && task.TrangThai != "Done" ? Color.Red : Color.DimGray,
                Location = new Point(6, 28),
                Size = new Size(180, 18)
            };

            var lblAssignee = new Label
            {
                Text = $"NV: {task.NguoiPhuTrach}",
                Font = new Font("Segoe UI", 8F),
                Location = new Point(6, 47),
                Size = new Size(180, 18)
            };

            var btnNext = new Button
            {
                Text = task.TrangThai == "Done" ? "Đã xong" : "Hoàn thành",
                Enabled = task.TrangThai != "Done",
                Location = new Point(6, 67),
                Size = new Size(95, 22)
            };
            btnNext.Click += (s, e) =>
            {
                if (_congViecBUS.CapNhatTrangThai(task.MaTask, "Done", out string msg))
                {
                    LoadTasks();
                    LoadCanhBaoTreHan();
                    LoadThongKeThanhVien(task.MaDuAn);
                }
            };

            panel.Controls.AddRange(new Control[] { lblTitle, lblMeta, lblAssignee, btnNext });
            return panel;
        }

        private void btnThemTask_Click(object sender, EventArgs e)
        {
            if (dgvDuAn.CurrentRow?.DataBoundItem is DuAnDTO selectedProject)
            {
                var task = new CongViecDTO
                {
                    MaDuAn = selectedProject.MaDuAn,
                    TenTask = txtTenTask.Text.Trim(),
                    MoTa = txtMoTaTask.Text.Trim(),
                    HanNop = dtpHanNop.Value,
                    DoUuTien = cboDoUuTien.SelectedItem?.ToString() ?? "Trung bình",
                    TrangThai = "To Do"
                };

                if (_congViecBUS.ThemTask(task, out string msg))
                {
                    MessageBox.Show(msg, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtTenTask.Clear();
                    txtMoTaTask.Clear();
                    LoadTasks();
                    LoadCanhBaoTreHan();
                    LoadThongKeThanhVien(selectedProject.MaDuAn);
                }
                else
                {
                    MessageBox.Show(msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void btnCapNhatTrangThai_Click(object sender, EventArgs e)
        {
            if (dgvTasks.CurrentRow?.DataBoundItem is CongViecDTO selectedTask)
            {
                string trangThaiMoi = cboTrangThaiMoi.SelectedItem?.ToString();
                if (_congViecBUS.CapNhatTrangThai(selectedTask.MaTask, trangThaiMoi, out string msg))
                {
                    MessageBox.Show(msg, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadTasks();
                    LoadCanhBaoTreHan();
                    LoadThongKeThanhVien(selectedTask.MaDuAn);
                }
            }
        }

        private void btnPhanCong_Click(object sender, EventArgs e)
        {
            if (dgvTasks.CurrentRow?.DataBoundItem is CongViecDTO selectedTask)
            {
                using (var frm = new frmPhanCong(selectedTask.MaTask, selectedTask.TenTask))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        LoadTasks();
                        LoadThongKeThanhVien(selectedTask.MaDuAn);
                    }
                }
            }
        }

        private void LoadCanhBaoTreHan()
        {
            List<CongViecDTO> overdueList = _congViecBUS.LayTaskTreHan();
            int countOverdue = _congViecBUS.DemSoTaskTreHan();

            dgvOverdue.DataSource = overdueList;
            lblCanhBaoTreHan.Text = $"Cảnh báo: Có {countOverdue} công việc trễ hạn chưa hoàn thành!";
            lblCanhBaoTreHan.ForeColor = countOverdue > 0 ? Color.Red : Color.ForestGreen;
        }

        private void LoadThongKeThanhVien(int maDuAn)
        {
            dgvStats.DataSource = _congViecBUS.ThongKeTyLeHoanThanhTheoThanhVien(maDuAn);
        }

        private void cboDuAnFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboDuAnFilter.SelectedValue != null)
                LoadTasks();
        }

        private void cboTrangThaiFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTrangThaiFilter.SelectedItem != null)
                LoadTasks();
        }

        private void dgvDuAn_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDuAn.CurrentRow?.DataBoundItem is DuAnDTO duAn)
            {
                txtTenDuAn.Text = duAn.TenDuAn;
                dtpNgayBatDau.Value = duAn.NgayBatDau;
                if (duAn.NgayKetThuc.HasValue) dtpNgayKetThuc.Value = duAn.NgayKetThuc.Value;
                cboTrangThaiDuAn.SelectedItem = duAn.TrangThai;

                LoadThongKeThanhVien(duAn.MaDuAn);
            }
        }
    }
}
